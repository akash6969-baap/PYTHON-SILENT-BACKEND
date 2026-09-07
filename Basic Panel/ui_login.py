"""
ui_login.py - License key authentication screen with slide-to-login for Fear Mods Loader
"""
from PyQt5.QtWidgets import QWidget, QVBoxLayout, QLabel, QHBoxLayout, QFrame
from PyQt5.QtCore import Qt, QTimer, pyqtSignal
from PyQt5.QtGui import QFont, QPainter, QColor, QPen, QBrush, QPainterPath

from ui_widgets import theme, bold_font, font, FearInput, FearButton, SlideLoginButton, ToastNotification, FearCheckbox


class LoginScreen(QWidget):
    login_success = pyqtSignal()

    def __init__(self, auth_manager, parent=None):
        super().__init__(parent)
        self.auth = auth_manager
        self._app_name = "MODULE"
        self._build_ui()
        
        # Auto-load saved license
        saved_key = self.auth.load_license_key()
        if saved_key:
            self._key_input.setText(saved_key)

    def set_app_name(self, name):
        self._app_name = name
        self._title_lbl.setText(name.upper())

    def apply_theme(self):
        from ui_widgets import theme
        th = theme()
        if hasattr(self, '_card'):
            self._card.setStyleSheet(f"""
                QFrame {{
                    background: transparent;
                    border: 2.5px solid {th['accent']};
                    border-radius: 16px;
                }}
            """)
        if hasattr(self, '_title_lbl'):
            self._title_lbl.setStyleSheet(f"color: {th['text']}; letter-spacing: 3px; border: none;")
        
        for lbl in getattr(self, '_theme_labels', []):
            try:
                lbl.setStyleSheet(f"color: {th['accent']}; letter-spacing: 2px;")
            except: pass

    def _build_ui(self):
        th = theme()
        self._theme_labels = []
        layout = QVBoxLayout(self)
        layout.setContentsMargins(30, 0, 30, 16)
        layout.setSpacing(0)

        layout.addStretch(1)

        # Red bordered card area
        self._card = QFrame()
        self._card.setStyleSheet(f"""
            QFrame {{
                background: transparent;
                border: 2.5px solid {th['accent']};
                border-radius: 16px;
            }}
        """)
        card_layout = QVBoxLayout(self._card)
        card_layout.setContentsMargins(24, 24, 24, 24)
        card_layout.setSpacing(16)

        # Application title
        self._title_lbl = QLabel("MODULE")
        self._title_lbl.setFont(bold_font(16))
        self._title_lbl.setStyleSheet(f"color: {th['text']}; letter-spacing: 3px; border: none;")
        self._title_lbl.setAlignment(Qt.AlignCenter)
        card_layout.addWidget(self._title_lbl)

        card_layout.addSpacing(4)

        # Key input
        self._key_input = FearInput("XXXX-XXXX-XXXX-XXXX")
        card_layout.addWidget(self._key_input)
        card_layout.addSpacing(6)

        # Slide-to-login button
        self._slider = SlideLoginButton()
        card_layout.addWidget(self._slider)
        self._slider.add_callback(self._on_slide_complete)

        layout.addWidget(self._card)

        layout.addStretch(1)

        # Toast notification (bottom)
        self._toast = ToastNotification(self)
        self._toast.setFixedWidth(350)

    def resizeEvent(self, e):
        super().resizeEvent(e)
        if hasattr(self, '_toast'):
            tw = self._toast.width()
            self._toast.move((self.width() - tw) // 2, self.height() - 54)

    def _on_slide_complete(self):
        key = self._key_input.text().strip()
        if not key:
            self._toast.show_message("Please enter a license key", "error")
            self._slider.reset()
            return

        self._key_input.setEnabled(False)
        self._toast.show_message("Verifying license...", "warn", 10000)

        def do_verify():
            try:
                res = self.auth.verify_key(key)
            except Exception as ex:
                res = {"success": False, "message": str(ex)}

            if res.get("success"):
                # Always save license automatically on success
                self.auth.save_license_key(key)

                self._toast.show_message("Access Granted!", "success", 1500)
                QTimer.singleShot(1200, self.login_success.emit)
            else:
                msg = res.get("message", "Invalid License Key")
                self._toast.show_message(msg, "error")
                self._slider.reset()
                self._key_input.setEnabled(True)

        # Small delay to show "Verifying" state
        QTimer.singleShot(1200, do_verify)

    def reset(self):
        """Reset UI for a fresh login attempt."""
        self._key_input.clear()
        self._key_input.setEnabled(True)
        self._slider.reset()
        self._toast.hide()
