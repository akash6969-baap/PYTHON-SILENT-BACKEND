import os
import sys
import subprocess
import threading
import time
from PyQt5.QtWidgets import QFrame, QVBoxLayout, QHBoxLayout, QLabel, QWidget, QPushButton
from PyQt5.QtCore import Qt, QRectF, pyqtSignal
from PyQt5.QtGui import QPainter, QPen, QBrush, QLinearGradient, QColor, QPainterPath
from ui_widgets import theme, bold_font, font

def get_gpu_usage() -> int:
    """
    Retrieves GPU usage percentage using nvidia-smi or PowerShell performance counters. Failsafe.
    """
    try:
        startupinfo = subprocess.STARTUPINFO()
        startupinfo.dwFlags |= subprocess.STARTF_USESHOWWINDOW
        startupinfo.wShowWindow = subprocess.SW_HIDE
        res = subprocess.run(
            ["nvidia-smi", "--query-gpu=utilization.gpu", "--format=csv,noheader,nounits"],
            capture_output=True, text=True, startupinfo=startupinfo, timeout=1
        )
        if res.returncode == 0 and res.stdout.strip():
            return int(res.stdout.strip())
    except Exception:
        pass

    try:
        startupinfo = subprocess.STARTUPINFO()
        startupinfo.dwFlags |= subprocess.STARTF_USESHOWWINDOW
        startupinfo.wShowWindow = subprocess.SW_HIDE
        cmd = 'powershell -Command "(Get-Counter -Counter \\"\\GPU Engine(*)\\Utilization Percentage\\" -ErrorAction SilentlyContinue).CounterSamples | Measure-Object -Property CookedValue -Sum | Select-Object -ExpandProperty Sum"'
        res = subprocess.run(cmd, shell=True, capture_output=True, text=True, startupinfo=startupinfo, timeout=1)
        if res.returncode == 0 and res.stdout.strip():
            val = float(res.stdout.strip())
            return min(100, int(val))
    except Exception:
        pass

    return 0

class CircularProgress(QWidget):
    """
    Highly premium, perfectly round circular dial progress bar (Gauge Ring).
    """
    def __init__(self, parent=None):
        super().__init__(parent)
        self.value = 0
        self.setFixedSize(100, 100) # Fixed size to maintain perfect aspect ratio

    def setValue(self, val):
        self.value = val
        self.update()

    def paintEvent(self, event):
        th = theme()
        painter = QPainter(self)
        painter.setRenderHint(QPainter.Antialiasing)

        pen_width = 6
        margin = pen_width + 2
        width = self.width()
        height = self.height()
        
        side = min(width, height)
        rect = QRectF(margin, margin, side - margin * 2, side - margin * 2)

        # 1. Draw Background Track Ring
        pen_track = QPen()
        pen_track.setColor(QColor(th['border']))
        pen_track.setWidth(pen_width)
        pen_track.setCapStyle(Qt.RoundCap)
        painter.setPen(pen_track)
        painter.drawArc(rect, 0, 360 * 16)

        # 2. Draw Active Value Arc (Neon Gradient Sweep)
        pen_value = QPen()
        grad = QLinearGradient(rect.left(), rect.top(), rect.right(), rect.bottom())
        grad.setColorAt(0, QColor(th['accent']))
        grad.setColorAt(1, QColor(th['accent2']))
        
        pen_value.setBrush(QBrush(grad))
        pen_value.setWidth(pen_width)
        pen_value.setCapStyle(Qt.RoundCap)
        painter.setPen(pen_value)

        # Start from top (90 deg) and sweep clockwise (negative span)
        start_angle = 90 * 16
        span_angle = -int((self.value / 100.0) * 360 * 16)
        painter.drawArc(rect, start_angle, span_angle)

        # 3. Draw Center Value Percentage Text
        painter.setPen(QPen(QColor(th['text'])))
        painter.setFont(bold_font(12))
        painter.drawText(rect, Qt.AlignCenter, f"{self.value}%")

class SystemMonitorWidget(QFrame):
    """
    System Monitor page class. Inherits from QFrame so that stylesheet border & background paint correctly.
    """
    update_signal = pyqtSignal(int, int, int) # CPU, RAM, GPU

    def __init__(self, parent=None):
        super().__init__(parent)
        self.init_ui()

        # Update loop in a daemon thread to prevent UI lag
        self.update_signal.connect(self.update_displays)
        self.active = True
        self.thread = threading.Thread(target=self._monitor_loop, daemon=True)
        self.thread.start()

    def init_ui(self):
        th = theme()
        self.setObjectName("FeatureCard")
        # Apply style directly to QFrame selector to ensure background card displays properly
        self.setStyleSheet(f"QFrame#FeatureCard {{ border: 1px solid {th['border']}; background: {th['surface']}; border-radius: 8px; }}")

        layout = QVBoxLayout(self)
        layout.setContentsMargins(12, 12, 12, 12)
        layout.setSpacing(25)

        # Header Title
        self.title_label = QLabel("SYSTEM PERFORMANCE MONITOR")
        self.title_label.setFont(bold_font(11))
        self.title_label.setAlignment(Qt.AlignCenter)
        self.title_label.setFixedHeight(30)
        self.title_label.setStyleSheet(f"""
            QLabel {{
                background-color: {th['accent']};
                color: #FFFFFF;
                border-radius: 12px;
                font-weight: bold;
                letter-spacing: 1.5px;
                border: none;
            }}
        """)
        layout.addWidget(self.title_label)

        # Horizontal layout to show gauges side-by-side, centered inside container
        self.gauges_layout = QHBoxLayout()
        self.gauges_layout.setAlignment(Qt.AlignCenter)
        self.gauges_layout.setSpacing(14)

        self.cpu_gauge, self.cpu_card = self.create_gauge_card("CPU LOAD", "cpu")
        self.ram_gauge, self.ram_card = self.create_gauge_card("RAM USAGE", "ram")
        self.gpu_gauge, self.gpu_card = self.create_gauge_card("GPU LOAD", "gpu")

        self.gauges_layout.addWidget(self.cpu_card)
        self.gauges_layout.addWidget(self.ram_card)
        self.gauges_layout.addWidget(self.gpu_card)

        layout.addLayout(self.gauges_layout)
        layout.addStretch()

    def create_gauge_card(self, title_text, key):
        th = theme()
        card = QFrame()
        card.setObjectName(f"GaugeCard_{key}")
        card.setStyleSheet(f"QFrame#GaugeCard_{key} {{ background: {th['bg']}; border: 1px solid {th['border']}; border-radius: 8px; }}")
        card.setFixedSize(130, 155)

        layout = QVBoxLayout(card)
        layout.setContentsMargins(8, 12, 8, 12)
        layout.setSpacing(10)
        layout.setAlignment(Qt.AlignCenter)

        # Title at the top of the card
        lbl_title = QLabel(title_text)
        lbl_title.setFont(bold_font(9))
        lbl_title.setAlignment(Qt.AlignCenter)
        lbl_title.setStyleSheet(f"color: {th['text']}; border: none;")
        layout.addWidget(lbl_title)

        # Circular progress widget
        gauge = CircularProgress()
        layout.addWidget(gauge, 0, Qt.AlignCenter)

        # Store references
        setattr(self, f"lbl_title_{key}", lbl_title)
        setattr(self, f"card_{key}", card)

        return gauge, card

    def apply_theme(self):
        th = theme()
        self.setStyleSheet(f"QFrame#FeatureCard {{ border: 1px solid {th['border']}; background: {th['surface']}; border-radius: 8px; }}")
        self.title_label.setStyleSheet(f"""
            QLabel {{
                background-color: {th['accent']};
                color: #FFFFFF;
                border-radius: 12px;
                font-weight: bold;
                letter-spacing: 1.5px;
                border: none;
            }}
        """)

        for key in ["cpu", "ram", "gpu"]:
            card = getattr(self, f"card_{key}", None)
            if card:
                card.setStyleSheet(f"QFrame#GaugeCard_{key} {{ background: {th['bg']}; border: 1px solid {th['border']}; border-radius: 8px; }}")
            lbl_title = getattr(self, f"lbl_title_{key}", None)
            if lbl_title:
                lbl_title.setStyleSheet(f"color: {th['text']}; border: none;")
            
            # Repaint dial gauge widget
            gauge = getattr(self, f"{key}_gauge", None)
            if gauge:
                gauge.update()

    def _monitor_loop(self):
        import psutil
        while self.active:
            try:
                cpu = int(psutil.cpu_percent(interval=0.5))
                ram = int(psutil.virtual_memory().percent)
                gpu = get_gpu_usage()
                self.update_signal.emit(cpu, ram, gpu)
            except Exception:
                pass
            time.sleep(0.5)

    def update_displays(self, cpu, ram, gpu):
        self.cpu_gauge.setValue(cpu)
        self.ram_gauge.setValue(ram)
        self.gpu_gauge.setValue(gpu)

    def closeEvent(self, event):
        self.active = False
        super().closeEvent(event)

class SystemMonitorButton(QPushButton):
    """
    Sleek premium vector button displaying 3 vertical graph columns.
    """
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setFixedSize(30, 30)
        self.setCursor(Qt.PointingHandCursor)
        
    def paintEvent(self, event):
        th = theme()
        painter = QPainter(self)
        painter.setRenderHint(QPainter.Antialiasing)
        
        is_hovered = self.underMouse()
        bg_color = QColor(th['surface'])
        border_color = QColor(th['accent']) if is_hovered else QColor(th['border'])
        pen_color = QColor(th['accent']) if is_hovered else QColor(th['subtext'])
        
        # Draw base card container
        painter.setPen(QPen(border_color, 1))
        painter.setBrush(QBrush(bg_color))
        painter.drawRoundedRect(QRectF(1, 1, 28, 28), 8, 8)
        
        # Draw 3 vertical load bar indicator blocks
        painter.setBrush(QBrush(pen_color))
        painter.setPen(Qt.NoPen)
        painter.drawRoundedRect(QRectF(8, 16, 3, 6), 1, 1)    # CPU (short)
        painter.drawRoundedRect(QRectF(13, 10, 3, 12), 1, 1)  # RAM (tall)
        painter.drawRoundedRect(QRectF(18, 14, 3, 8), 1, 1)   # GPU (medium)

class ThemeToggleButton(QPushButton):
    """
    Vector button switching dynamically between Sun (Dark theme active) and Moon (Light theme active) shapes.
    """
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setFixedSize(30, 30)
        self.setCursor(Qt.PointingHandCursor)
        
    def paintEvent(self, event):
        from ui_widgets import _current_theme
        th = theme()
        painter = QPainter(self)
        painter.setRenderHint(QPainter.Antialiasing)
        
        is_hovered = self.underMouse()
        bg_color = QColor(th['surface'])
        border_color = QColor(th['accent']) if is_hovered else QColor(th['border'])
        pen_color = QColor(th['accent']) if is_hovered else QColor(th['subtext'])
        
        # Draw base card container
        painter.setPen(QPen(border_color, 1))
        painter.setBrush(QBrush(bg_color))
        painter.drawRoundedRect(QRectF(1, 1, 28, 28), 8, 8)
        
        if _current_theme == "dark":
            # Draw premium flat Sun icon (central ring + 8 radiating beams)
            painter.setBrush(QBrush(pen_color))
            painter.setPen(Qt.NoPen)
            painter.drawEllipse(11, 11, 8, 8)
            
            painter.setPen(QPen(pen_color, 1.5, Qt.SolidLine, Qt.RoundCap))
            painter.drawLine(15, 6, 15, 8)
            painter.drawLine(15, 22, 15, 24)
            painter.drawLine(6, 15, 8, 15)
            painter.drawLine(22, 15, 24, 15)
            painter.drawLine(9, 9, 11, 11)
            painter.drawLine(19, 19, 21, 21)
            painter.drawLine(9, 21, 11, 19)
            painter.drawLine(19, 9, 21, 11)
        else:
            # Draw premium flat Crescent Moon shape using PainterPath path clipping
            path = QPainterPath()
            path.moveTo(20, 9)
            path.arcTo(QRectF(9, 9, 12, 12), 60, 240)
            path.arcTo(QRectF(11, 9, 10, 12), 300, -240)
            path.closeSubpath()
            
            painter.setBrush(QBrush(pen_color))
            painter.setPen(Qt.NoPen)
            painter.drawPath(path)
