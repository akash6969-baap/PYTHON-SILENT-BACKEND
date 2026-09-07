"""
auth.py - Safe wrapper around FearAuth API for the Fear Mods Loader
"""
import os
import threading
import json


class SafeAuth:
    """Thread-safe auth manager that wraps FearAuth and prevents os._exit crashes."""

    def __init__(self):
        from config import FEARAUTH_APP_NAME, FEARAUTH_OWNER_ID, FEARAUTH_SECRET, FEARAUTH_VERSION
        self.app_name = FEARAUTH_APP_NAME
        self.owner_id = FEARAUTH_OWNER_ID
        self.secret   = FEARAUTH_SECRET
        self.version  = FEARAUTH_VERSION

        self.api = None
        self.is_mock = False
        self._lock = threading.Lock()
        self._config_file = "auth_config.json"

    def save_license_key(self, key):
        """Save the license key to a local config file."""
        try:
            with open(self._config_file, "w") as f:
                json.dump({"license_key": key}, f)
            return True
        except Exception as e:
            print(f"[Auth] Save error: {e}")
            return False

    def load_license_key(self):
        """Load the saved license key from local config."""
        try:
            if os.path.exists(self._config_file):
                with open(self._config_file, "r") as f:
                    data = json.load(f)
                    return data.get("license_key", "")
        except Exception as e:
            print(f"[Auth] Load error: {e}")
        return ""

    def forget_license_key(self):
        """Delete the saved license key."""
        try:
            if os.path.exists(self._config_file):
                os.remove(self._config_file)
        except Exception:
            pass

    def initialize(self):
        """Attempt to connect to FearAuth. Safe to call from any thread."""
        with self._lock:
            try:
                from FearAuth import api as FearApi
                
                # We still monkey-patch os._exit just in case of any uncaptured prints in newer versions
                real_exit = os._exit
                os._exit = lambda c: None

                self.api = FearApi(
                    name=self.app_name,
                    ownerid=self.owner_id,
                    secret=self.secret,
                    version=self.version,
                    auto_init=False
                )
                
                # Check for empty credentials
                if not self.owner_id or not self.secret:
                    print("[Auth] Missing credentials in .env! Running in Mock Mode.")
                    self.is_mock = True
                else:
                    try:
                        self.api.init()
                        print(f"[Auth] Initialized: {self.app_name}")
                        self.is_mock = False
                    except Exception as e:
                        print(f"[Auth] Initialization failed: {e}")
                        self.is_mock = False  # DO NOT fallback to mock if real credentials provided!
                        self.init_error = str(e)

                os._exit = real_exit
            except ImportError:
                print("[Auth] FearAuth.py not found! Mock mode enabled.")
                self.is_mock = True

    def verify_key(self, key):
        """Verify a license key. Returns dict with success/message."""
        with self._lock:
            if self.is_mock:
                # Preview / offline mode (if credentials missing)
                if len(key) >= 5:
                    return {"success": True, "message": "Preview Mode Active"}
                return {"success": False, "message": "Invalid Key (Offline Mode)"}

            if self.api is None or not getattr(self.api, "initialized", False):
                err = getattr(self, "init_error", "FearAuth Not Initialized")
                return {"success": False, "message": f"Init Error: {err}"}

            # Use the safe method we added to FearAuth.py
            # This avoids hard exits even if verification fails.
            res = self.api.license_safe(key)
            return res


# Singleton
fear_auth = SafeAuth()
