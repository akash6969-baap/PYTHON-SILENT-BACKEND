@echo off
title Building EXE with Nuitka
setlocal

echo === CHECKING REQUIREMENTS ===

REM Check if Nuitka is installed
python -m nuitka --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Nuitka not found. Installing Nuitka and dependencies...
    pip install nuitka ordered-set zstandard
)

REM Check if main Python file exists
if not exist "FearMods.py" (
    echo ERROR: FearMods.py not found in current directory!
    pause
    exit /b 1
)

REM Check Succes.wav file
if not exist "Succes.wav" (
    echo WARNING: Succes.wav not found! Build may lack custom completion audio.
)

echo.
echo === CLEANING OLD FILES ===
rmdir /s /q dist_nuitka >nul 2>&1
del /f /q SilentAim_RELEASE.zip >nul 2>&1

echo.
echo === COMPILING WITH NUITKA (STANDALONE MODE) ===
echo This will compile Python code directly to C and produce a protected native EXE.
echo Please wait, this process can take 5 to 15 minutes...
echo.

python -m nuitka --standalone --windows-console-mode=disable --windows-uac-admin --enable-plugin=pyqt5 --include-data-files=auth_config.json=auth_config.json --include-data-files=theme.cfg=theme.cfg --include-data-files=Succes.wav=Succes.wav --output-dir=dist_nuitka --output-filename=SilentAim.exe FearMods.py

if %errorlevel% neq 0 (
    echo.
    echo ERROR: Nuitka compilation failed!
    pause
    exit /b %errorlevel%
)

echo.
echo === CREATING DISTRIBUTION ZIP ===
powershell -Command "Compress-Archive -Path '.\dist_nuitka\FearMods.dist\*' -DestinationPath '.\SilentAim_RELEASE.zip' -Force"

echo.
echo === BUILD DONE! ===
echo.
echo Output Folder: dist_nuitka\FearMods.dist
echo Output ZIP:    SilentAim_RELEASE.zip
echo.
echo NOTE: Share 'SilentAim_RELEASE.zip' with your users.
echo       They must extract the ZIP and run 'SilentAim.exe' as Administrator.
echo       The source code is 100%% protected and compiled to C!
echo.
pause
