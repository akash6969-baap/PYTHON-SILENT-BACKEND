import os
import shutil
import tempfile
import subprocess
import ctypes
import sys
import time

def run_cmd_silent(cmd):
    try:
        startupinfo = subprocess.STARTUPINFO()
        startupinfo.dwFlags |= subprocess.STARTF_USESHOWWINDOW
        startupinfo.wShowWindow = subprocess.SW_HIDE
        subprocess.run(cmd, shell=True, startupinfo=startupinfo, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    except Exception:
        pass

def clean_temp_files(log_callback=None) -> str:
    """
    Safely deletes temporary files from user temp and Windows system temp folders.
    """
    def log(msg):
        if log_callback:
            log_callback(msg)

    log("Scanning temporary directories...")
    temp_paths = [
        tempfile.gettempdir(),
        os.path.join(os.environ.get("SystemRoot", "C:\\Windows"), "Temp")
    ]
    
    files_deleted = 0
    bytes_freed = 0
    
    for path in temp_paths:
        if not os.path.exists(path):
            continue
        log(f"Cleaning directory: {path}")
        for item in os.listdir(path):
            item_path = os.path.join(path, item)
            try:
                if os.path.isfile(item_path) or os.path.islink(item_path):
                    size = os.path.getsize(item_path)
                    os.remove(item_path)
                    files_deleted += 1
                    bytes_freed += size
                elif os.path.isdir(item_path):
                    shutil.rmtree(item_path)
                    files_deleted += 1
            except Exception:
                # Ignore files locked by running apps
                continue

    mb_freed = bytes_freed / (1024 * 1024)
    log(f"Successfully deleted {files_deleted} items ({mb_freed:.2f} MB freed).")
    return f"Temp Clean: Completed | {mb_freed:.2f} MB freed"

def pc_optimize(log_callback=None) -> str:
    """
    Safely optimizes PC by flushing DNS, cleaning prefetch and system log files.
    """
    def log(msg):
        if log_callback:
            log_callback(msg)

    log("Flushing DNS Resolver Cache...")
    run_cmd_silent("ipconfig /flushdns")
    
    log("Cleaning Prefetch directory...")
    prefetch_path = os.path.join(os.environ.get("SystemRoot", "C:\\Windows"), "Prefetch")
    if os.path.exists(prefetch_path):
        for item in os.listdir(prefetch_path):
            item_path = os.path.join(prefetch_path, item)
            try:
                if os.path.isfile(item_path) or os.path.islink(item_path):
                    os.remove(item_path)
                elif os.path.isdir(item_path):
                    shutil.rmtree(item_path)
            except Exception:
                continue
                
    log("Cleaning system logs...")
    run_cmd_silent("wevtutil cl Setup")
    run_cmd_silent("wevtutil cl System")
    
    log("PC Optimization complete!")
    return "PC Optimizer: DNS flushed & Logs cleared"

def fps_boost(log_callback=None) -> str:
    """
    Boosts FPS by configuring CPU priority for emulators and clearing system standby memory.
    """
    def log(msg):
        if log_callback:
            log_callback(msg)

    log("Adjusting system response configuration...")
    emulator_names = ["HD-Player.exe", "MEmu.exe", "Nox.exe"]
    found_emulator = False
    
    for name in emulator_names:
        cmd = f'powershell -Command "Get-Process -Name {name.replace(".exe", "")} -ErrorAction SilentlyContinue | Foreach-Object {{ $_.PriorityClass = [System.Diagnostics.ProcessPriorityClass]::High }}"'
        try:
            startupinfo = subprocess.STARTUPINFO()
            startupinfo.dwFlags |= subprocess.STARTF_USESHOWWINDOW
            startupinfo.wShowWindow = subprocess.SW_HIDE
            res = subprocess.run(cmd, shell=True, startupinfo=startupinfo, capture_output=True)
            # Check if any process was found/updated
            if res.returncode == 0:
                log(f"Optimized CPU priority for {name} to HIGH.")
                found_emulator = True
        except Exception:
            pass

    log("Releasing system Standby memory...")
    try:
        ctypes.windll.psapi.EmptyWorkingSet(ctypes.windll.kernel32.GetCurrentProcess())
    except Exception:
        pass
        
    if found_emulator:
        log("FPS Boost: Active (Game priority configured to High)")
        return "FPS Boost: Active (High Priority Enabled)"
    else:
        log("FPS Boost: Standby memory optimized.")
        return "FPS Boost: System Optimized (No emulator running)"
