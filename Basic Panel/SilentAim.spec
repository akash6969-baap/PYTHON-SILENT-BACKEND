# -*- mode: python ; coding: utf-8 -*-

PYDIVERT_DLL_DIR = 'C:\\Users\\akash\\AppData\\Local\\Programs\\Python\\Python314\\Lib\\site-packages\\pydivert\\windivert_dll'

a = Analysis(
    ['FearMods.py'],
    pathex=['.'],  # Include current dir so all .py modules are found
    binaries=[
        (PYDIVERT_DLL_DIR + '\\WinDivert64.dll', '.'),
        (PYDIVERT_DLL_DIR + '\\WinDivert64.sys', '.'),
        (PYDIVERT_DLL_DIR + '\\WinDivert32.dll', '.'),
        (PYDIVERT_DLL_DIR + '\\WinDivert32.sys', '.'),
    ],
    datas=[
        # Only NON-CODE assets — no .py files here (prevents source exposure)
        ('Succes.wav', '.'),
        ('auth_config.json', '.'),
        ('theme.cfg', '.'),
    ],
    hiddenimports=[
        # Our own modules — compiled to bytecode, not plain text
        'internal', 'injector', 'keyauth', 'Memory', 'connection',
        'offset', 'FearAuth', 'auth', 'config',
        'ui_login', 'ui_splash', 'ui_widgets', 'optimizer', 'sys_monitor',
        # Third party
        'PyQt5', 'PyQt5.QtWidgets', 'PyQt5.QtCore', 'PyQt5.QtGui',
        'pymem', 'requests', 'psutil',
        'win32gui', 'win32process', 'win32security', 'win32con',
        'pywintypes', 'flask', 'flask_cors',
        'pydivert', 'winsound', 'dotenv',
        'qrcode', 'qrcode.image.base', 'qrcode.image.pure',
        'PIL', 'PIL.Image', 'PIL.ImageDraw', 'PIL.ImageFont',
        'keyboard', 'binascii', 'platform', 'hashlib',
        'threading', 'math', 'json', 'ctypes', 'ctypes.wintypes',
    ],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    noarchive=False,
    optimize=2,  # Strips docstrings too for extra obfuscation
)
pyz = PYZ(a.pure)

exe = EXE(
    pyz,
    a.scripts,
    [],
    exclude_binaries=True,
    name='BasicPanel',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=False,
    console=False,
    uac_admin=True,
)

coll = COLLECT(
    exe,
    a.binaries,
    a.datas,
    strip=False,
    upx=False,
    upx_exclude=[],
    name='BasicPanel',
)
