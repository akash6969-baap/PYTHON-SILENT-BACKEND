@echo off
title Frontend Runner (Admin)
cd /d "%~dp0"

:: ─── Auto-Elevate to Administrator Rights ───────────────────────────────
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo [!] Requesting Administrative Privileges via PowerShell...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)
:: ─────────────────────────────────────────────────────────────────────────

echo === Checking Python ===
python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: Python not found! Install Python first.
    pause
    exit /b 1
)

echo === Checking packages ===
python -c "import PyQt5" >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing PyQt5...
    pip install PyQt5
)
python -c "import pymem" >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing pymem...
    pip install pymem
)
python -c "import psutil" >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing psutil...
    pip install psutil
)
python -c "import win32gui" >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing pywin32...
    pip install pywin32
)
python -c "import pydivert" >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing pydivert...
    pip install pydivert
)
python -c "import flask" >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing flask + flask_cors...
    pip install flask flask-cors
)
python -c "import requests" >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing requests...
    pip install requests
)

echo.
echo === Starting Frontend ===
python FearMods.py
if %errorlevel% neq 0 (
    echo.
    echo ERROR: Frontend crashed! Check above for errors.
    pause
)
