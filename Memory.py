import pyexpat
import pymem
import pymem.pattern
import pymem.process
import time
import ctypes
import ctypes.wintypes
import os
import sys
pm = None
def scan_and_replace(process_name, search, replace):
    try:
        # Ensure the process exists before proceeding
        pm = pymem.Pymem(process_name)
        pm.open_process_from_id(pm.process_id)

        matches = pm.pattern_scan_all(search, return_multiple=True)
        if not matches:
            print("No matching patterns found.")
            return False

        print(f"Found {len(matches)} matches.")
        for match in matches:
            try:
                pm.write_bytes(match, replace, len(replace))
            except Exception as e:
                print(f"Failed to write memory at {hex(match)}: {e}")
                continue  # Skip to the next match instead of crashing

        pm.close_process()  # Ensure process is properly closed
        return True
    except pymem.exception.PymemError as e:
        print(f"Memory access error: {e}")
    except Exception as e:
        print(f"Unexpected error: {e}")
    return False  # Ensure function always returns a boolean
def get_resource_path(relative_path):
    """ Get absolute path to resource, works for both development and PyInstaller. """
    try:
        # PyInstaller creates a temp folder and stores path in _MEIPASS
        base_path = sys._MEIPASS
    except Exception:
        base_path = os.path.abspath(".")

    return os.path.join(base_path, relative_path)
def get_process(procesName):
    try:
        pm = pymem.Pymem(procesName)
        print('Process Found Please Continue')
        return pm.process_id
    except:
        print('Process Not Found Waiting for process')
        return False

def get_pid(processName):
    pm = pymem.Pymem(processName)
    return pm.process_id

def get_drive_serial_number():
    kernel32 = ctypes.windll.kernel32
    volume_name_buffer = ctypes.create_unicode_buffer(1024)
    file_system_name_buffer = ctypes.create_unicode_buffer(1024)
    serial_number = ctypes.c_ulong(0)
    max_component_length = ctypes.c_ulong(0)
    file_system_flags = ctypes.c_ulong(0)

    success = kernel32.GetVolumeInformationW(
        ctypes.c_wchar_p("C:\\"),
        volume_name_buffer,
        ctypes.sizeof(volume_name_buffer),
        ctypes.byref(serial_number),
        ctypes.byref(max_component_length),
        ctypes.byref(file_system_flags),
        file_system_name_buffer,
        ctypes.sizeof(file_system_name_buffer)
    )

    if success:
        return serial_number.value
    else:
        return None

def get_hwid():
    serial_number = get_drive_serial_number()
    if serial_number:
        return serial_number
    else:
        return None


def adjust_privileges():
    """
    Adjust token privileges to enable SeDebugPrivilege.
    This is necessary to manipulate memory of other processes.
    """
    SE_DEBUG_NAME = "SeDebugPrivilege"
    SE_PRIVILEGE_ENABLED = 0x00000002
    token_handle = ctypes.c_void_p()
    luid = ctypes.c_longlong()
    
    # Open process token
    ctypes.windll.advapi32.OpenProcessToken(
        ctypes.windll.kernel32.GetCurrentProcess(),
        0x20 | 0x8,
        ctypes.byref(token_handle)
    )

    # Lookup privilege value
    ctypes.windll.advapi32.LookupPrivilegeValueA(
        0, SE_DEBUG_NAME.encode('ascii'), ctypes.byref(luid)
    )
    
    class LUID_AND_ATTRIBUTES(ctypes.Structure):
        _fields_ = [("Luid", ctypes.c_longlong), ("Attributes", ctypes.c_ulong)]

    class TOKEN_PRIVILEGES(ctypes.Structure):
        _fields_ = [("PrivilegeCount", ctypes.c_ulong), ("Privileges", LUID_AND_ATTRIBUTES)]
    
    new_privileges = TOKEN_PRIVILEGES(1, LUID_AND_ATTRIBUTES(luid.value, SE_PRIVILEGE_ENABLED))
    
    # Adjust token privileges
    ctypes.windll.advapi32.AdjustTokenPrivileges(
        token_handle, False, ctypes.byref(new_privileges), 0, None, None
    )
    
    # Close token handle
    ctypes.windll.kernel32.CloseHandle(token_handle)


def find_pattern(pm, module_name, pattern):
    # module = pymem.process.module_from_name(pm.process_handle, module_name)
  
    return pymem.pattern.pattern_scan_all(pm.process_handle, pattern,return_multiple=True)

def aimbot_load():
    try:
        adjust_privileges()
        process_name = "HD-Player.exe"
        pm = pymem.Pymem(process_name)

        pattern = b'\x00\x00\x00\x00\x00\x00\xA5\x43\x00\x00\x00\x00....\x00\x00\x00\x00\x00\x00\x00\x00....................................................................................................................\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00....\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x80\xBF' #Replace with your actual pattern bytes
        addresses = find_pattern(pm, process_name, pattern)

        if not addresses:
            print("No addresses found")
            return False
        return addresses

    except pymem.exception.MemoryReadError as e:
        print(f"Could not read memory at: {e.address} - GetLastError: {e.error_code}")
    except pymem.exception.MemoryWriteError as e:
        print(f"Could not write memory at: {e.address} - GetLastError: {e.error_code}")
    except pymem.exception.WinAPIError as e:
        print(f"Windows API error occurred: {e.error_code}")
    except Exception as e:
        print(f"An unexpected error occurred: {e}")

original_values = []
def aimbot_on(addresses):
    global orginal_values
    pm = pymem.Pymem("HD-Player.exe")
    original_values.clear()
    for address in addresses:
        addressscan = address + 0x5C
        addressrep = address + 0X28
        buffer = pm.read_int(addressscan)
        original_values.append(pm.read_int(addressrep))
        pm.write_int(addressrep, buffer)
def aimbot_off(addresses):
    global original_values
    pm = pymem.Pymem("HD-Player.exe")
    for index,address in enumerate(addresses):
        addressrep = address + 0x28
        if original_values[index]:
            pm.write_int(addressrep, original_values[index])


def drag_load():
    global drag_addresses
    try:
        adjust_privileges()
        process_name = "HD-Player.exe"
        pm = pymem.Pymem(process_name)

        pattern = b'\xFF\xFF\xFF\xFF\xFF\xFF\xFF\xFF\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00................\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\xA5\x43\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00............................................................\x00\x00\x00\x00....................................\x00\x00\x00\x00....\x00\x00\x00\x00........\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x80\xBF'
        drag_addresses = find_pattern(pm, process_name, pattern)

        if not drag_addresses:
            print("No addresses found")
            return False
        return drag_addresses
        # print(f"Found {len(addresses)} addresses")
        # for address in addresses:
        #     addressscan = address + 96
        #     addressrep = address + 0x5C
        #     buffer = pm.read_int(addressscan)
        #     pm.write_int(addressrep, buffer)
        # ctypes.windll.kernel32.Beep(500, 500)

    except pymem.exception.MemoryReadError as e:
        print(f"Could not read memory at: {e.address} - GetLastError: {e.error_code}")
    except pymem.exception.MemoryWriteError as e:
        print(f"Could not write memory at: {e.address} - GetLastError: {e.error_code}")
    except pymem.exception.WinAPIError as e:
        print(f"Windows API error occurred: {e.error_code}")
    except Exception as e:
        print(f"An unexpected error occurred: {e}")

original_drag_values = []
def aimdrag_on(drag_addresses):
    global original_drag_values
    pm = pymem.Pymem("HD-Player.exe")
    original_drag_values.clear()
    for address in drag_addresses:
        addressscan = address + 0x70
        addressrep = address +0x6C
        buffer = pm.read_int(addressscan)
        original_drag_values.append(pm.read_int(addressrep))
        pm.write_int(addressrep, buffer)


def aimdrag_off(drag_addresses):
    global original_drag_values
    pm = pymem.Pymem("HD-Player.exe")
    for index,address in enumerate(drag_addresses):
        addressrep = address + 0x6C
        if original_drag_values[index]:
            pm.write_int(addressrep, original_drag_values[index])