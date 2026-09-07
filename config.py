import os
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

# FearAuth Configuration
FEARAUTH_APP_NAME = os.getenv("FEARAUTH_APP_NAME", "PYTHON AIMSILENT EXE")
FEARAUTH_OWNER_ID = os.getenv("FEARAUTH_OWNER_ID", "a6ae1e06-c701-435e-8e06-c87edc79319b")
FEARAUTH_SECRET = os.getenv("FEARAUTH_SECRET", "06ccae20de3e34b1722350640ca9154c39964661552cbf56")
FEARAUTH_VERSION = os.getenv("FEARAUTH_VERSION", "1.0")
