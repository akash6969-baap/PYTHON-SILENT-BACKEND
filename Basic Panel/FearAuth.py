import os
import json as jsond
import time
import binascii
import platform
import subprocess
import qrcode
from datetime import datetime, timezone, timedelta
from PIL import Image


try:
    if os.name == 'nt':
        import win32security
    import requests
except ModuleNotFoundError:
    print("Exception when importing modules")
    print("Installing necessary modules....")
    if os.path.isfile("requirements.txt"):
        os.system("pip install -r requirements.txt")
    else:
        if os.name == 'nt':
            os.system("pip install pywin32")
        os.system("pip install requests")
    print("Modules installed!")
    time.sleep(1.5)
    os._exit(1)


class api:

    name = ownerid = version = hash_to_check = ""
    API_URL = "https://api.fearauth.online/api/1.2"

    def __init__(self, name, ownerid, secret, version, hash_to_check=None, auto_init=True):
        self.name = name
        self.ownerid = ownerid
        self.secret = secret
        self.version = version
        self.hash_to_check = hash_to_check or ""
        self.initialized = False
        if auto_init:
            self.init()

    def init_safe(self):
        try:
            self.init()
            return True, None
        except Exception as e:
            return False, str(e)

    def login_safe(self, user, password, code=None, hwid=None):
        try:
            self.login(user, password, code, hwid)
            return {"success": True, "message": "Logged in", "info": {"username": self.user_data.username, "subscriptions": self.user_data.subscriptions}}
        except Exception as e:
            return {"success": False, "message": str(e)}

    def register_safe(self, user, password, license, hwid=None):
        try:
            self.register(user, password, license, hwid)
            return {"success": True, "message": "Registered"}
        except Exception as e:
            return {"success": False, "message": str(e)}

    def license_safe(self, key, code=None, hwid=None):
        try:
            self.license(key, code, hwid)
            return {"success": True, "message": "Authenticated"}
        except Exception as e:
            return {"success": False, "message": str(e)}

    sessionid = enckey = ""
    initialized = False

    def init(self):
        if self.sessionid != "":
            print("You've already initialized!")
            time.sleep(3)
            os._exit(1)
        
        post_data = {
            "type": "init",
            "ver": self.version,
            "hash": self.hash_to_check,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        if response == "FearAuth_Invalid":
            print("The application doesn't exist")
            time.sleep(3)
            os._exit(1)

        json = jsond.loads(response)

        if json.get("message") == "invalidver":
            if json.get("download") != "":
                print("New Version Available")
                download_link = json["download"]
                os.system(f"start {download_link}")
                time.sleep(3)
                os._exit(1)
            else:
                print("Invalid Version, Contact owner to add download link to latest app version")
                time.sleep(3)
                os._exit(1)

        if not json.get("success"):
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

        self.sessionid = json.get("sessionid", "")
        self.initialized = True

    def register(self, user, password, license, hwid=None):
        self.checkinit()
        if hwid is None:
            hwid = others.get_hwid()

        post_data = {
            "type": "register",
            "username": user,
            "pass": password,
            "key": license,
            "hwid": hwid,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            print(json.get("message"))
            self.__load_user_data(json.get("info", {}))
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

    def upgrade(self, user, license):
        self.checkinit()

        post_data = {
            "type": "upgrade",
            "username": user,
            "key": license,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            print(json.get("message"))
            print("Please restart program and login")
            time.sleep(3)
            os._exit(1)
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

    def login(self, user, password, code=None, hwid=None):
        self.checkinit()
        if hwid is None:
            hwid = others.get_hwid()

        post_data = {
            "type": "login",
            "username": user,
            "pass": password,
            "hwid": hwid,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid,
        }
        
        if code is not None:
            post_data["code"] = code

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            self.__load_user_data(json.get("info", {}))
            print(json.get("message"))
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

    def license(self, key, code=None, hwid=None):
        self.checkinit()
        if hwid is None:
            hwid = others.get_hwid()

        post_data = {
            "type": "license",
            "key": key,
            "hwid": hwid,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }
        
        if code is not None:
            post_data["code"] = code

        response = self.__do_request(post_data)
        
        try:
            json = jsond.loads(response)
        except Exception:
            # Fallback to string processing like in C# if JSON decode fails due to extra unexpected fields
            if '"success":true' in response.replace(" ", ""):
                json = {"success": True, "message": "Logged in"}
            else:
                json = {"success": False, "message": "Failed to login"}

        if json.get("success"):
            # Attempt to decode full info, or use empty dict
            info = json.get("info", {})
            if info:
                 self.__load_user_data(info)
            base_msg = json.get("message")
            if base_msg:
                 print(base_msg)
        else:
            msg = json.get("message", "Error")
            if self.initialized: # If it's used as a library, we might prefer exceptions
                 raise Exception(msg)
            print(msg)
            time.sleep(3)
            os._exit(1)

    def var(self, name):
        self.checkinit()

        post_data = {
            "type": "var",
            "varid": name,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            return json.get("message")
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

    def getvar(self, var_name):
        self.checkinit()

        post_data = {
            "type": "getvar",
            "var": var_name,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }
        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            return json.get("response")
        else:
            print(f"NOTE: This is commonly misunderstood. This is for user variables, not the normal variables.\nUse FearAuthapp.var(\"{var_name}\") for normal variables")
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

    def setvar(self, var_name, var_data):
        self.checkinit()

        post_data = {
            "type": "setvar",
            "var": var_name,
            "data": var_data,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }
        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            return True
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

    def ban(self):
        self.checkinit()

        post_data = {
            "type": "ban",
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }
        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            return True
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

    def file(self, fileid):
        self.checkinit()

        post_data = {
            "type": "file",
            "fileid": fileid,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if not json.get("success"):
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)
        return binascii.unhexlify(json.get("contents"))

    def webhook(self, webid, param, body = "", conttype = ""):
        self.checkinit()

        post_data = {
            "type": "webhook",
            "webid": webid,
            "params": param,
            "body": body,
            "conttype": conttype,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            return json.get("message")
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)

    def check(self):
        self.checkinit()

        post_data = {
            "type": "check",
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }
        response = self.__do_request(post_data)

        json = jsond.loads(response)
        if json.get("success"):
            return True
        else:
            return False

    def checkblacklist(self):
        self.checkinit()
        hwid = others.get_hwid()

        post_data = {
            "type": "checkblacklist",
            "hwid": hwid,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }
        response = self.__do_request(post_data)

        json = jsond.loads(response)
        if json.get("success"):
            return True
        else:
            return False

    def log(self, message):
        self.checkinit()

        post_data = {
            "type": "log",
            "pcuser": os.getenv('username'),
            "message": message,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        self.__do_request(post_data)

    def fetchOnline(self):
        self.checkinit()

        post_data = {
            "type": "fetchOnline",
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            if len(json.get("users", [])) == 0:
                return None
            else:
                return json.get("users")
        else:
            return None
            
    def fetchStats(self):
        self.checkinit()

        post_data = {
            "type": "fetchStats",
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            self.__load_app_data(json.get("appinfo", {}))
            
    def chatGet(self, channel):
        self.checkinit()

        post_data = {
            "type": "chatget",
            "channel": channel,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            return json.get("messages")
        else:
            return None

    def chatSend(self, message, channel):
        self.checkinit()

        post_data = {
            "type": "chatsend",
            "message": message,
            "channel": channel,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            return True
        else:
            return False

    def checkinit(self):
        if not self.initialized:
            print("Initialize first, in order to use the functions")
            time.sleep(3)
            os._exit(1)

    def changeUsername(self, username):
        self.checkinit()

        post_data = {
            "type": "changeUsername",
            "newUsername": username,
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            print("Successfully changed username")
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)  

    def logout(self):
        self.checkinit()

        post_data = {
            "type": "logout",
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid
        }

        response = self.__do_request(post_data)

        json = jsond.loads(response)

        if json.get("success"):
            print("Successfully logged out")
            time.sleep(3)
            os._exit(1)
        else:
            print(json.get("message"))
            time.sleep(3)
            os._exit(1)  
            
    def enable2fa(self, code=None):
        self.checkinit()
        
        post_data = {
            "type": "2faenable",
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid,
            "code": code
        }       
        
        response = self.__do_request(post_data)
        
        json = jsond.loads(response)
        
        if json.get("success"):
            if code is None:
                print(f"Your 2FA secret code is: {json.get('2fa', {}).get('secret_code')}")
                qr_code = json.get('2fa', {}).get('QRCode')
                self.display_qr_code(qr_code)
                code_input = input("Enter the 6 digit 2fa code to enable 2fa: ")
                self.enable2fa(code_input)
            else:
                print("2FA has been successfully enabled!")
                time.sleep(3)
        else:
            print(f"Error: {json.get('message')}")
            time.sleep(3)
            os._exit(1)
            
    def disable2fa(self, code=None):
        self.checkinit()
        
        code = input("Enter the 6 digit 2fa code to disable 2fa: ")
        
        post_data = {
            "type": "2fadisable",
            "sessionid": self.sessionid,
            "name": self.name,
            "ownerid": self.ownerid,
            "code": code
        }
        
        response = self.__do_request(post_data)
        
        json = jsond.loads(response)
        
        print(json.get('message'))
        time.sleep(3)
        
            
    def display_qr_code(self, qr_code_url):
            qr = qrcode.QRCode(
                version=1,
                error_correction=qrcode.constants.ERROR_CORRECT_L,
                box_size=10,
                border=4,
            )

            qr.add_data(qr_code_url)
            qr.make(fit=True)

            img = qr.make_image(fill='black', back_color='white')

            img.show()            
            
    def __do_request(self, post_data):
        try:
            # We use timeout=60 because Render free tier needs ~30-50 seconds to wake up if asleep
            response = requests.post(
                self.API_URL, data=post_data, timeout=60
            )

            # Removed signature checking mechanics completely
            # as our backend does not use Ed25519 signatures
            return response.text

        except requests.exceptions.Timeout: 
            print("Request timed out. Render server might be waking up... Try running it again.")
            return '{"success": false, "message": "Connection Timeout"}'
        except Exception as e:
            print(f"Connection error: {e}")
            return '{"success": false, "message": "Connection Error"}'
                
            
    class application_data_class:
        numUsers = numKeys = app_ver = customer_panel = onlineUsers = ""

    class user_data_class:
        username = ip = hwid = expires = createdate = lastlogin = subscription = subscriptions = ""

    user_data = user_data_class()
    app_data = application_data_class()

    def __load_app_data(self, data):
        self.app_data.numUsers = data.get("numUsers", "")
        self.app_data.numKeys = data.get("numKeys", "")
        self.app_data.app_ver = data.get("version", "")
        self.app_data.customer_panel = data.get("customerPanelLink", "")
        self.app_data.onlineUsers = data.get("numOnlineUsers", "")

    def __load_user_data(self, data):
        self.user_data.username = data.get("username", "")
        self.user_data.ip = data.get("ip", "")
        self.user_data.hwid = data.get("hwid", "N/A")
        try:
            subs = data.get("subscriptions", [])
            if subs and len(subs) > 0:
                self.user_data.expires = subs[0].get("expiry", "")
                self.user_data.subscription = subs[0].get("subscription", "")
            else:
                self.user_data.expires = ""
                self.user_data.subscription = ""
            self.user_data.subscriptions = subs
        except Exception:
            pass
        self.user_data.createdate = data.get("createdate", getattr(data, 'create_date', ''))
        self.user_data.lastlogin = data.get("lastlogin", getattr(data, 'last_login', ''))


class others:
    @staticmethod
    def get_hwid():
        if platform.system() == "Linux":
            with open("/etc/machine-id") as f:
                hwid = f.read()
                return hwid.strip()
        elif platform.system() == 'Windows':
            try:
                # Try getting UUID via wmic (more robust for hardware locking)
                cmd = 'wmic csproduct get uuid'
                uuid = subprocess.check_output(cmd, shell=True).decode().split('\n')[1].strip()
                if not uuid or "Not Available" in uuid:
                     raise Exception("UUID Not Available")
                return uuid
            except Exception:
                # Fallback to SID if wmic fails
                try:
                    winuser = os.getlogin()
                    sid = win32security.LookupAccountName(None, winuser)[0]
                    hwid = win32security.ConvertSidToStringSid(sid)
                    return hwid
                except:
                    return "UNKNOWN_HWID"
        elif platform.system() == 'Darwin':
            output = subprocess.Popen("ioreg -l | grep IOPlatformSerialNumber", stdout=subprocess.PIPE, shell=True).communicate()[0]
            serial = output.decode().split('=', 1)[1].replace(' ', '')
            hwid = serial[1:-2]
            return hwid
