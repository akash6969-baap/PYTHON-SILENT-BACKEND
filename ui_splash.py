"""
ui_splash.py - Circular buffering splash screen for Fear Mods Loader
"""
from PyQt5.QtWidgets import QWidget, QVBoxLayout, QLabel
from PyQt5.QtCore import Qt, QTimer, QRectF, pyqtSignal
from PyQt5.QtGui import QPainter, QPen, QColor, QConicalGradient, QFont, QBrush, QLinearGradient
import math


class SplashScreen(QWidget):
    finished = pyqtSignal()

    def __init__(self, parent=None):
        super().__init__(parent)
        self._angle = 0
        self._inner_angle = 0
        self._alpha = 255
        self._dots = 0
        self._phase = "loading"  # loading | success | fadeout
        self._progress = 0
        self._target_progress = 0
        self._timer = QTimer(self)
        self._timer.timeout.connect(self._tick)
        self._timer.start(16)  # ~60fps

        # Auto-advance loading bar
        self._progress_timer = QTimer(self)
        self._progress_timer.timeout.connect(self._advance_progress)
        self._progress_timer.start(40)

        self._dot_timer = QTimer(self)
        self._dot_timer.timeout.connect(self._advance_dots)
        self._dot_timer.start(400)

    def _advance_dots(self):
        self._dots = (self._dots + 1) % 4
        self.update()

    def _advance_progress(self):
        if self._phase != "loading":
            return
        if self._progress < 95:
            self._progress = min(95, self._progress + 1.2)
            self.update()

    def complete_loading(self):
        """Call this when auth / init is done — fills bar to 100%, shows checkmark."""
        self._progress_timer.stop()
        self._phase = "success"
        self._fill_timer = QTimer(self)
        self._fill_timer.timeout.connect(self._fill_to_100)
        self._fill_timer.start(15)

    def _fill_to_100(self):
        self._progress = min(100, self._progress + 2)
        self.update()
        if self._progress >= 100:
            self._fill_timer.stop()
            QTimer.singleShot(600, self._start_fadeout)

    def _start_fadeout(self):
        self._phase = "fadeout"
        self._fade_timer = QTimer(self)
        self._fade_timer.timeout.connect(self._do_fade)
        self._fade_timer.start(16)

    def _do_fade(self):
        self._alpha = max(0, self._alpha - 12)
        self.update()
        if self._alpha <= 0:
            self._fade_timer.stop()
            self.finished.emit()

    def _tick(self):
        self._angle = (self._angle + 2) % 360          # Clockwise outer rotation
        self._inner_angle = (self._inner_angle - 4) % 360 # Counter-clockwise inner rotation
        self.update()

    def paintEvent(self, event):
        from PyQt5.QtGui import QRadialGradient
        w, h = self.width(), self.height()
        p = QPainter(self)
        p.setRenderHint(QPainter.Antialiasing)
        p.setOpacity(self._alpha / 255)

        # Background (rounded bottom corners to match outer frame)
        p.setPen(Qt.NoPen)
        p.setBrush(QColor("#09090D"))
        p.drawRoundedRect(0, 0, w, h, 16, 16)

        cx, cy = w // 2, h // 2 - 30  # Center point of loader
        ring_r = min(w, h) * 0.22

        # 1. Outer Red Dual Arcs
        r_outer = QRectF(cx - ring_r, cy - ring_r, ring_r * 2, ring_r * 2)
        # Thin background track
        p.setPen(QPen(QColor("#1b1b22"), 2, Qt.SolidLine))
        p.drawEllipse(r_outer)
        # Rotating outer red arcs
        p.setPen(QPen(QColor("#E60000"), 4, Qt.SolidLine, Qt.RoundCap))
        p.drawArc(r_outer, (self._angle) * 16, 110 * 16)
        p.drawArc(r_outer, (self._angle + 180) * 16, 110 * 16)

        # 2. Inner White Arc (Opposite rotation)
        r_inner = QRectF(cx - ring_r * 0.65, cy - ring_r * 0.65, ring_r * 1.3, ring_r * 1.3)
        # Thin background track
        p.setPen(QPen(QColor("#181822"), 1.5, Qt.SolidLine))
        p.drawEllipse(r_inner)
        # Rotating inner white arc
        p.setPen(QPen(QColor("white"), 3, Qt.SolidLine, Qt.RoundCap))
        p.drawArc(r_inner, (self._inner_angle) * 16, 80 * 16)

        # 3. Central Glowing Circle
        if self._phase == "success":
            # Success Green glow and checkmark
            glow = QRadialGradient(cx, cy, 22)
            glow.setColorAt(0.0, QColor(0, 208, 74, 220))
            glow.setColorAt(0.3, QColor(0, 208, 74, 100))
            glow.setColorAt(1.0, QColor(0, 208, 74, 0))
            p.setBrush(QBrush(glow))
            p.setPen(Qt.NoPen)
            p.drawEllipse(cx - 22, cy - 22, 44, 44)

            p.setPen(QPen(QColor("white"), 3, Qt.SolidLine, Qt.RoundCap, Qt.RoundJoin))
            scale = ring_r * 0.3
            x1, y1 = cx - scale * 0.6, cy
            x2, y2 = cx - scale * 0.1, cy + scale * 0.5
            x3, y3 = cx + scale * 0.6, cy - scale * 0.4
            p.drawLine(int(x1), int(y1), int(x2), int(y2))
            p.drawLine(int(x2), int(y2), int(x3), int(y3))
        else:
            # Loading Red glow and central white dot
            glow = QRadialGradient(cx, cy, 22)
            glow.setColorAt(0.0, QColor(230, 0, 0, 220))
            glow.setColorAt(0.3, QColor(230, 0, 0, 100))
            glow.setColorAt(1.0, QColor(230, 0, 0, 0))
            p.setBrush(QBrush(glow))
            p.setPen(Qt.NoPen)
            p.drawEllipse(cx - 22, cy - 22, 44, 44)

            # Central White Dot
            p.setBrush(QBrush(QColor("white")))
            p.setPen(Qt.NoPen)
            p.drawEllipse(cx - 4, cy - 4, 8, 8)

        # Status text removed for pure minimal animation look
        pass
