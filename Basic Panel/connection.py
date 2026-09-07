# connection.py
import os
import sys
import psutil
import win32gui
import win32process
import ctypes
import subprocess
from offset import Offsets

class EmulatorConnection:
    def __init__(self, emulator_name="HD-Player", adb_name="HD-Adb.exe"):
        self.emulator_name = emulator_name
        self.adb_name = adb_name
        self.handle = None
        self.adb_path = None
        self.module_addr = None

    def find_process(self):
        for proc in psutil.process_iter(['pid', 'name']):
            if proc.info['name'] and self.emulator_name in proc.info['name']:
                return proc
        return None

    def find_render_window(self, parent_handle):
        render_hwnd = None

        def callback(hwnd, _):
            text = win32gui.GetWindowText(hwnd)
            if text and text != self.emulator_name:
                nonlocal render_hwnd
                render_hwnd = hwnd
            return True

        win32gui.EnumChildWindows(parent_handle, callback, None)
        return render_hwnd

    def get_adb_path(self, process):
        try:
            exe_path = process.exe()
            base_dir = os.path.dirname(exe_path)
            adb_path = os.path.join(base_dir, self.adb_name)
            if os.path.exists(adb_path):
                self.adb_path = adb_path
                return adb_path
        except Exception:
            return None
        return None

    def run_adb(self, args):
        if not self.adb_path:
            raise Exception("ADB not found")
        return subprocess.check_output([self.adb_path] + args, shell=True)

    def connect(self):
        proc = self.find_process()
        if not proc:
            print("❌ Emulator not found. Please start emulator.")
            sys.exit(0)

        print(f"✅ Found emulator: {proc.name()} (PID: {proc.pid})")

        hwnd = win32gui.FindWindow(None, self.emulator_name)
        if not hwnd:
            print("❌ Emulator main window not found.")
            sys.exit(0)

        render_hwnd = self.find_render_window(hwnd)
        if not render_hwnd:
            print("❌ Render window not found.")
            sys.exit(0)

        print(f"✅ Found render window: {render_hwnd}")

        adb = self.get_adb_path(proc)
        if not adb:
            print("❌ ADB not found in emulator folder.")
            sys.exit(0)

        print(f"✅ Using ADB: {adb}")

        try:
            self.run_adb(["kill-server"])
            self.run_adb(["start-server"])
        except Exception as e:
            print("❌ Error starting ADB:", e)
            sys.exit(0)

        try:
            out = self.run_adb(["shell", "su -c 'cat /proc/$(pidof com.dts.freefireth)/maps | grep libil2cpp.so'"])
            line = out.decode().split("\n")[0]
            base_addr = int(line.split("-")[0], 16)
            Offsets.Il2Cpp = base_addr
            self.module_addr = base_addr
            print(f"✅ libil2cpp.so base: {hex(base_addr)}")
        except Exception as e:
            print("❌ Could not find libil2cpp.so:", e)
            sys.exit(0)

        self.handle = render_hwnd
        return True
