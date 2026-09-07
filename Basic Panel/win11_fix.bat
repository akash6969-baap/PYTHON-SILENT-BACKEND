@echo off
title FearMods - Windows 11 Fix
color 0C
cls

echo.
echo  ============================================
echo   FEAR MODS - WINDOWS 11 COMPATIBILITY FIX
echo  ============================================
echo.

REM ─── Check Admin Rights ───────────────────────────────────────────────────
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo  [ERROR] Admin rights required!
    echo  Right-click this file and select "Run as Administrator"
    echo.
    pause
    exit /b 1
)

echo  [OK] Running as Administrator
echo.
echo  Applying fixes for Windows 11...
echo.

REM ─── Fix 1: Disable Memory Integrity (HVCI) ──────────────────────────────
echo  [1/3] Disabling Memory Integrity (HVCI)...
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity" /v "Enabled" /t REG_DWORD /d 0 /f >nul 2>&1
if %errorlevel% equ 0 (
    echo        Done!
) else (
    echo        Already disabled or not applicable.
)

REM ─── Fix 2: Disable VBS (Virtualization Based Security) ──────────────────
echo  [2/3] Disabling Virtualization Based Security (VBS)...
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard" /v "EnableVirtualizationBasedSecurity" /t REG_DWORD /d 0 /f >nul 2>&1
if %errorlevel% equ 0 (
    echo        Done!
) else (
    echo        Already disabled or not applicable.
)

REM ─── Fix 3: Disable Credential Guard ─────────────────────────────────────
echo  [3/3] Disabling Credential Guard...
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa" /v "LsaCfgFlags" /t REG_DWORD /d 0 /f >nul 2>&1
if %errorlevel% equ 0 (
    echo        Done!
) else (
    echo        Already disabled or not applicable.
)

echo.
echo  ============================================
echo   ALL FIXES APPLIED SUCCESSFULLY!
echo  ============================================
echo.
echo  IMPORTANT: PC restart ZARURI hai!
echo  Restart ke baad FearMods normal kaam karega.
echo.

choice /C YN /M "  Abhi restart kare? (Y = Haan, N = Baad me)"
if %errorlevel% equ 1 (
    echo.
    echo  Restarting in 5 seconds...
    shutdown /r /t 5 /c "FearMods Win11 Fix - Restarting..."
) else (
    echo.
    echo  Manually restart karo jaldi se!
    echo.
    pause
)
