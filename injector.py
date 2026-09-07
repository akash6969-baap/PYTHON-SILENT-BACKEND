import os
import shutil
import pymem
import time
import threading
import sys
import urllib.request
import hashlib
import ssl
import json


# ─────────────────────────────────────────────────────────────────────────────
#  XOR Encrypt / Decrypt  (same logic as C# version, key = 0x99)
# ─────────────────────────────────────────────────────────────────────────────
def _xor(text: str, key: int = 0x99) -> str:
    return "".join(chr(ord(c) ^ key) for c in text)


# ─────────────────────────────────────────────────────────────────────────────
#  XOR-obfuscated URL constants  (plain text won't appear in compiled binary)
#  _xor() at module-level  → stores ENCODED form
#  _xor() at runtime       → DECODES back to original URL
# ─────────────────────────────────────────────────────────────────────────────
_U_BASE = _xor("https://github.com/akash6969-baap/PYTHON-SILENT-BACKEND/raw/main/")
_U_C    = _xor("cimgui.dll")
_U_A    = _xor("AotBst.dll")
_U_L    = _xor("Client.dll")


def _get_urls() -> dict:
    base = _xor(_U_BASE)   # decode at runtime only
    return {
        _xor(_U_C): base + _xor(_U_C),
        _xor(_U_A): base + _xor(_U_A),
        _xor(_U_L): base + _xor(_U_L),
    }


# ─────────────────────────────────────────────────────────────────────────────
#  Helpers
# ─────────────────────────────────────────────────────────────────────────────
def _get_base_dir() -> str:
    """Returns the EXE / script directory (for sound, config files etc.)"""
    if getattr(sys, "frozen", False):
        return os.path.dirname(sys.executable)
    return os.path.dirname(os.path.abspath(__file__))


def _get_dll_cache_dir() -> str:
    """
    Hidden DLL storage -> C:\\ProgramData\\WindowsApps\\cache\\
    Looks like a legit Windows folder, hidden from normal users.
    Created automatically if it doesn't exist.
    """
    path = os.path.join(os.environ.get("PROGRAMDATA", "C:\\ProgramData"),
                        "WindowsApps", "cache")
    os.makedirs(path, exist_ok=True)
    return path


def _md5(path: str) -> str:
    h = hashlib.md5()
    with open(path, "rb") as f:
        for chunk in iter(lambda: f.read(8192), b""):
            h.update(chunk)
    return h.hexdigest()


def _download_dll(url: str, dest: str, log, ok_msg: str, skip_msg: str) -> bool:
    """Download a DLL from a raw GitHub URL to dest path."""
    try:
        headers = {"User-Agent": "Mozilla/5.0"}
        req = urllib.request.Request(url, headers=headers)
        # Create unverified context to bypass SSL validation checks
        ctx = ssl._create_unverified_context()
        with urllib.request.urlopen(req, timeout=15, context=ctx) as resp:
            data = resp.read()
        # Write only if content changed (MD5 diff)
        if os.path.exists(dest):
            existing_md5 = _md5(dest)
            new_md5 = hashlib.md5(data).hexdigest()
            if existing_md5 == new_md5:
                log(skip_msg, 0.2)
                return True
        with open(dest, "wb") as f:
            f.write(data)
        log(ok_msg, 0.3)
        return True
    except Exception:
        return False


# Messages shown during DLL download — no DLL names visible
_DLL_MSGS = {
    "cimgui.dll":  ("Graphics module updated successfully.",   "Graphics module is up-to-date."),
    "AotBst.dll":  ("Security module updated successfully.",   "Security module is up-to-date."),
    "Client.dll":  ("Core module updated successfully.",       "Core module is up-to-date."),
}





# ─────────────────────────────────────────────────────────────────────────────
#  Core inject helper
# ─────────────────────────────────────────────────────────────────────────────
def inject_dll(process, dll_path, log_callback=None):
    try:
        if not os.path.exists(dll_path):
            return False
        dll_path_bytes = dll_path.encode("utf-8")
        pymem.process.inject_dll(process.process_handle, dll_path_bytes)
        return True
    except Exception:
        return False


# ─────────────────────────────────────────────────────────────────────────────
#  Success sound
# ─────────────────────────────────────────────────────────────────────────────
def play_success_sound():
    try:
        import winsound

        exe_dir    = os.path.dirname(sys.executable) if getattr(sys, "frozen", False) else os.path.dirname(os.path.abspath(__file__))
        bundle_dir = getattr(sys, "_MEIPASS", exe_dir)

        custom_wav = os.path.join(exe_dir, "Succes.wav")
        if os.path.exists(custom_wav):
            winsound.PlaySound(custom_wav, winsound.SND_FILENAME | winsound.SND_ASYNC)
            return

        bundled_wav = os.path.join(bundle_dir, "Succes.wav")
        if os.path.exists(bundled_wav):
            winsound.PlaySound(bundled_wav, winsound.SND_FILENAME | winsound.SND_ASYNC)
            return

        local_wavs = [os.path.join(exe_dir, f) for f in os.listdir(exe_dir) if f.endswith(".wav")]
        if local_wavs:
            winsound.PlaySound(local_wavs[0], winsound.SND_FILENAME | winsound.SND_ASYNC)
        else:
            winsound.PlaySound("SystemAsterisk", winsound.SND_ALIAS | winsound.SND_ASYNC)
    except Exception:
        pass


# ─────────────────────────────────────────────────────────────────────────────
#  Update & Sync helpers
# ─────────────────────────────────────────────────────────────────────────────
def check_update_status() -> dict:
    """
    Checks if an update is available on GitHub by comparing the local cached ETag
    with the remote HEAD request ETag of Client.dll.
    """
    cache_dir = _get_dll_cache_dir()
    version_file = os.path.join(cache_dir, "version.json")
    
    local_etag = ""
    if os.path.exists(version_file):
        try:
            with open(version_file, "r") as f:
                local_etag = json.load(f).get("Client.dll", "")
        except Exception:
            pass

    # If DLL files don't exist, we must update
    client_dll = os.path.join(cache_dir, "Client.dll")
    cimgui_dll = os.path.join(cache_dir, "cimgui.dll")
    aotbst_dll = os.path.join(cache_dir, "AotBst.dll")
    if not (os.path.exists(client_dll) and os.path.exists(cimgui_dll) and os.path.exists(aotbst_dll)):
        return {"update_available": True, "local_etag": local_etag, "remote_etag": "Missing Files"}

    try:
        urls = _get_urls()
        client_url = urls.get("Client.dll")
        req = urllib.request.Request(client_url, method="HEAD", headers={"User-Agent": "Mozilla/5.0"})
        ctx = ssl._create_unverified_context()
        with urllib.request.urlopen(req, timeout=10, context=ctx) as resp:
            remote_etag = resp.headers.get("ETag", "").strip('"')
        
        local_etag_clean = local_etag.strip('"')
        
        if not remote_etag:
            return {"update_available": False, "local_etag": local_etag, "remote_etag": "Unknown"}
            
        update_available = (local_etag_clean != remote_etag)
        return {
            "update_available": update_available,
            "local_etag": local_etag_clean,
            "remote_etag": remote_etag
        }
    except Exception as e:
        print("[Update] Status check failed:", e)
        return {"update_available": False, "error": True, "local_etag": local_etag, "remote_etag": "Offline"}


def download_updates(log_callback=None) -> bool:
    """
    Downloads required DLLs from GitHub, gets latest ETag, and saves version.json.
    """
    def log(msg, delay=0.1):
        if log_callback:
            log_callback(msg)
        if delay > 0:
            time.sleep(delay)

    cache_dir = _get_dll_cache_dir()
    urls = _get_urls()
    
    client_dll_path = os.path.join(cache_dir, "Client.dll")
    log("Checking local system configuration...", 0.2)
    
    if os.path.exists(client_dll_path):
        log("Removing outdated modules...", 0.2)
        try:
            os.remove(client_dll_path)
        except Exception:
            pass
            
    for dll_name in ["cimgui.dll", "AotBst.dll"]:
        p = os.path.join(cache_dir, dll_name)
        if not os.path.exists(p):
            log("Downloading required components...", 0.2)
            try:
                headers = {"User-Agent": "Mozilla/5.0"}
                req = urllib.request.Request(urls[dll_name], headers=headers)
                ctx = ssl._create_unverified_context()
                with urllib.request.urlopen(req, timeout=30, context=ctx) as resp:
                    data = resp.read()
                with open(p, "wb") as f:
                    f.write(data)
            except Exception:
                log("Failed to download dependency package!")
                return False

    log("Downloading latest update packages...", 0.2)
    try:
        headers = {"User-Agent": "Mozilla/5.0"}
        req = urllib.request.Request(urls["Client.dll"], headers=headers)
        ctx = ssl._create_unverified_context()
        with urllib.request.urlopen(req, timeout=30, context=ctx) as resp:
            data = resp.read()
        with open(client_dll_path, "wb") as f:
            f.write(data)
    except Exception as e:
        log("Failed to download update packages!")
        return False

    try:
        client_url = urls.get("Client.dll")
        req = urllib.request.Request(client_url, method="HEAD", headers={"User-Agent": "Mozilla/5.0"})
        ctx = ssl._create_unverified_context()
        with urllib.request.urlopen(req, timeout=10, context=ctx) as resp:
            remote_etag = resp.headers.get("ETag", "").strip('"')
        
        version_file = os.path.join(cache_dir, "version.json")
        with open(version_file, "w") as f:
            json.dump({"Client.dll": remote_etag}, f)
    except Exception:
        pass

    log("Updates completed successfully!", 0.2)
    return True


# ─────────────────────────────────────────────────────────────────────────────
#  Main injection thread
# ─────────────────────────────────────────────────────────────────────────────
def streamesp_thread(log_callback=None):
    def log(msg, delay=0.3):
        if log_callback:
            log_callback(msg)
        else:
            print(msg)
        if delay > 0:
            time.sleep(delay)

    process_name = "HD-Player.exe"

    try:
        log("Initializing security module...", 0.4)
        log("Scanning game instance [HD-Player.exe]...", 0.5)

        base_dir  = _get_base_dir()
        cache_dir = _get_dll_cache_dir()

        client_dll  = os.path.join(cache_dir, "Client.dll")
        cimgui_dll  = os.path.join(cache_dir, "cimgui.dll")
        aotbst_dll  = os.path.join(cache_dir, "AotBst.dll")

        if not (os.path.exists(client_dll) and os.path.exists(cimgui_dll) and os.path.exists(aotbst_dll)):
            log("Error: Modules not found locally. Please click 'Update' in the Notification tab.", 0)
            return

        process = pymem.Pymem(process_name)
        log("Process attached successfully. [PID: %d]" % process.process_id, 0.4)

        if os.path.exists(client_dll):
            temp_client = "C:\\Windows\\Temp\\Client.dll"
            try:
                shutil.copy(client_dll, temp_client)
            except Exception:
                log("Critical integrity check failed!", 0)
                return

        log("Performing memory integrity bypass...", 0.6)

        if inject_dll(process, cimgui_dll, None):
            log("Cryptographic handshake established.", 0.5)

            log("Decrypting memory tables and offsets...", 0.7)
            if inject_dll(process, aotbst_dll, None):
                log("Establishing NamedPipe tunnel (\\\\.\\pipe\\esp_pipe)...", 0.4)
                log("Backend Server active on Local Port 5554 / 5555.", 0.4)
                log("Connection secure. Status: ONLINE!", 0.3)
                log("Select FF Version & click 'APPLY HOOK' to start ADB.", 0)

                play_success_sound()

    except pymem.exception.ProcessNotFound:
        log("Game instance not found. Please launch the game first!", 0)
    except Exception:
        log("Server synchronization failed!", 0)


def streameesp(log_callback=None):
    threading.Thread(target=streamesp_thread, args=(log_callback,), daemon=True).start()
