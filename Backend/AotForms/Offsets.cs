
using System;

namespace AotForms
{
    internal static class Offsets
    {
        internal static ulong Il2Cpp;
        internal static uint InitBase = 0xA9870BC;
        internal static uint StaticClass = 0x5C;
        internal static uint ViewMatrix = 0xE8;

        internal static uint CurrentMatch = 0x50;
        internal static uint MatchStatus = 0x8c;
        internal static uint LocalPlayer = 0x94;
        internal static uint DictionaryEntities = 0x68;

        internal static uint Player_IsDead = 0x50;
        internal static uint Player_Name = 0x2dc;
        internal static uint Player_Data = 0x48;
        internal static uint Player_ShadowBase = 0x18b8;
        internal static uint XPose = 0x78;
        internal static uint AvatarManager = 0x4c0;
        internal static uint Avatar = 0xa8;
        internal static uint Avatar_IsVisible = 0x95;
        internal static uint Avatar_Data = 0x14;
        internal static uint Avatar_Data_IsTeam = 0x59;
        internal static uint PlayerID = 0x268;
        internal static uint BaseProfileInfo = 0x18cc;
        internal static uint IsClientBot = 0x2E4;
        internal static uint PlayerAttributes = 0x4bc;
        internal static uint UnlimitedAmmo = 0xE0;

        internal static uint FollowCamera = 0x450;
        internal static uint Camera = 0x18;
        internal static uint MainCameraTransform = 0x24c;
        internal static uint AimRotation = 0x400;

        internal static uint CurrentObserver = 0xb4;
        internal static uint ObserverPlayer = 0x28;

        internal static uint Weapon = 0x3f4;
        internal static uint WeaponData = 0x58;
        internal static uint WeaponRecoil = 0xc;
        internal static uint UnkPlayerWeaponInfoClass = 0x4a8;
        internal static uint IsCombineWeapon = 0xd8;
        internal static uint WeaponOnHand = 0x54;
        internal static uint CombineWeaponOnHand = 0x58;
        internal static uint WeaponInfo = 0x64;
        internal static uint WeaponID = 0x14;
        internal static uint NoReload = 0x99;

        internal static uint LastAimingInfoFromWeapon = 0x978;
        internal static uint IsFiring = 0x540;
        internal static uint IS_FIRING = 0x540;
        internal static uint sAim1 = 0x540;
        internal static uint sAim2 = 0x978;
        internal static uint sAim3 = 0x38;
        internal static uint sAim4 = 0x2c;
        internal static uint StartPosition = 0x38;
        internal static uint RayDir = 0x2c;
        internal static uint SilentAim = 0x404;  

        internal static uint LockedAimingCollider = 0x54;
        internal static uint AimbotVisible = 0x4a4;
        internal static uint NeckCollider = 0x460;  
        internal static uint LocalPlayerAttributes = 0x4BC;
        internal static uint kanishkcheat = 0x54;
        internal static uint adarshcheat  = 0x54;  

        internal static uint RunSpeedUpScale = 0x1d8;
        internal static uint FastFire = 0x18C;
        internal static uint WeaponSkinMap = 0x18D8; 
        internal static uint EmoteSlot = 0; 
        internal static uint InSnowSlideWayDashing = 0x15e8;
        internal static uint GameTimer = 0x10;
        internal static uint FixedDeltaTime = 0x24;
        internal static uint pomba = 0x540;
        internal static uint upplayeroffset = 0x494;  
        internal static uint MovementComponent = 0x124C;  

        internal static uint TransformComponent = 0x8;   
        internal static uint VisualStateMatrix = 0x20;   
        internal static uint MatrixPosition = 0x60;      
        internal static uint MatrixRotation = 0x70;      
        internal static uint MatrixIndices = 0x1C;       

        internal static uint BoneHead = 0x458;      
        internal static uint BoneLeftWrist = 0x498; 
        internal static uint BoneHip = 0x45C;       
        internal static uint BoneRoot = 0x46C;      

        enum MatchStatusEnum
        {
            MATCH_NOT_STARTED = 0,
            MATCH_RUNNING = 1,
            MATCH_PAUSED = 2,
            MATCH_ENDED = 3
        };
    }
}
