using AotForms;
using System.Numerics;

namespace AotForms
{
    internal static class Data
    {
        private static bool lastRapidFireState = false;
        private static Dictionary<ulong, float> originalFireIntervalValues = new Dictionary<ulong, float>();

        private static DateTime _lastFullReset = DateTime.UtcNow;
        private const int AUTO_RESET_INTERVAL_SEC = 5; 

        internal static void Work()
        {
            while (true)
            {
                if (Offsets.Il2Cpp == 0 || !InternalMemory.IsReady)
                {
                    Thread.Sleep(40);
                    continue;
                }

                if ((DateTime.UtcNow - _lastFullReset).TotalSeconds >= AUTO_RESET_INTERVAL_SEC)
                {
                    Core.Entities.Clear();
                    InternalMemory.ClearCache();
                    _lastFullReset = DateTime.UtcNow;
                }

                var rBaseGameFacade = InternalMemory.Read<uint>(Offsets.Il2Cpp + Offsets.InitBase, out var baseGameFacade);
                if (!rBaseGameFacade || baseGameFacade == 0)
                {
                    ResetCache();
                    Thread.Sleep(30);
                    continue;
                }

                var rGameFacade = InternalMemory.Read<uint>(baseGameFacade, out var gameFacade);
                if (!rGameFacade || gameFacade == 0)
                {
                    ResetCache();
                    Thread.Sleep(30);
                    continue;
                }

                var rStaticGameFacade = InternalMemory.Read<uint>(gameFacade + Offsets.StaticClass, out var staticGameFacade);
                if (!rStaticGameFacade || staticGameFacade == 0)
                {
                    ResetCache();
                    Thread.Sleep(30);
                    continue;
                }

                var rCurrentGame = InternalMemory.Read<uint>(staticGameFacade, out var currentGame);
                if (!rCurrentGame || currentGame == 0)
                {
                    ResetCache();
                    Thread.Sleep(30);
                    continue;
                }

                var rCurrentMatch = InternalMemory.Read<uint>(currentGame + Offsets.CurrentMatch, out var currentMatch);
                if (!rCurrentMatch || currentMatch == 0)
                {
                    ResetCache();
                    Thread.Sleep(30);
                    continue;
                }

                var rLocalPlayer = InternalMemory.Read<uint>(currentMatch + Offsets.LocalPlayer, out var localPlayer);
                if (!rLocalPlayer || localPlayer == 0)
                {
                    Thread.Sleep(25);
                    continue;
                }

                Core.LocalPlayer = localPlayer;

                var rMainTransform = InternalMemory.Read<uint>(localPlayer + Offsets.MainCameraTransform, out var mainTransform);
                if (!rMainTransform || mainTransform == 0)
                {
                    continue;
                }

                var rMainTransformPos = Transform.GetPosition(mainTransform, out var mainPos);
                if (rMainTransformPos)
                {
                    Core.LocalMainCamera = mainPos;
                }

                var rFollowCamera = InternalMemory.Read<uint>(localPlayer + Offsets.FollowCamera, out var followCamera);
                if (!rFollowCamera || followCamera == 0)
                {
                    continue;
                }

                var rCamera = InternalMemory.Read<uint>(followCamera + Offsets.Camera, out var camera);
                if (!rCamera || camera == 0)
                {
                    continue;
                }

                var rCameraBase = InternalMemory.Read<uint>(camera + 0x8, out var cameraBase);
                if (!rCameraBase || cameraBase == 0)
                {
                    Core.HaveMatrix = false;
                    continue;
                }

                var rViewMatrix = InternalMemory.Read<Matrix4x4>(cameraBase + Offsets.ViewMatrix, out var viewMatrix);
                if (!rViewMatrix)
                {
                    Core.HaveMatrix = false;
                    continue;
                }

                Core.CameraMatrix = viewMatrix;
                Core.HaveMatrix = true;

                if (Config.FastReload)
                {
                    if (InternalMemory.Read<uint>(localPlayer + Offsets.PlayerAttributes, out var reload))
                    {
                        InternalMemory.Write<bool>(reload + Offsets.NoReload, true);
                    }
                }
                else
                {
                    if (InternalMemory.Read<uint>(localPlayer + Offsets.PlayerAttributes, out var reload))
                    {
                        InternalMemory.Write<bool>(reload + Offsets.NoReload, false);
                    }
                }

                if (Config.UnlimitedAmmo)
                {
                    if (InternalMemory.Read<uint>(localPlayer + Offsets.PlayerAttributes, out var ammoAddr))
                    {
                        if (ammoAddr != 0)
                        {

                            InternalMemory.Write<bool>(ammoAddr + 0xE0, true);
                        }
                    }
                }
                else
                {
                    if (InternalMemory.Read<uint>(localPlayer + Offsets.PlayerAttributes, out var ammoAddr))
                    {
                        if (ammoAddr != 0)
                        {
                            InternalMemory.Write<bool>(ammoAddr + 0xE0, false);
                        }
                    }
                }

                if (InternalMemory.Read<uint>(currentGame + Offsets.GameTimer, out var speedHack) && speedHack != 0
                    && InternalMemory.Read<float>(speedHack + Offsets.FixedDeltaTime, out var speed))
                {
                    if (Config.speedint)
                    {
                        if (speed == 0.033000f)
                            InternalMemory.Write(speedHack + Offsets.FixedDeltaTime, 0.065000f);
                    }
                    else if (speed != 0.033000f)
                    {
                        InternalMemory.Write(speedHack + Offsets.FixedDeltaTime, 0.033000f);
                    }
                }

                if (Config.NoRecoil)
                {
                    var readWeapon = InternalMemory.Read<uint>(localPlayer + Offsets.Weapon, out var weapon);

                    if (readWeapon && weapon != 0)
                    {
                        var readWeaponData = InternalMemory.Read<uint>(weapon + Offsets.WeaponData, out var weaponData);

                        if (readWeaponData && weaponData != 0)
                        {

                            var readRecoil = InternalMemory.Read<float>(weaponData + Offsets.WeaponRecoil, out var recoil);
                            if (readRecoil && recoil != 0)
                            {
                                InternalMemory.Write(weaponData + Offsets.WeaponRecoil, 0f);
                            }
                        }
                    }
                }

                if (InternalMemory.Read<uint>(localPlayer + Offsets.PlayerAttributes, out var attributesAddr2))
                {
                    ulong addrKey = (ulong)attributesAddr2;
                    if (Config.RapidFire)
                    {
                        if (InternalMemory.Read<float>(attributesAddr2 + Offsets.FastFire, out var currentFireInterval))
                        {
                            if (!originalFireIntervalValues.ContainsKey(addrKey))
                            {
                                originalFireIntervalValues[addrKey] = currentFireInterval;
                            }

                            float rapidFireValue = 1.1f - Config.RapidFireSpeed;
                            if (Math.Abs(currentFireInterval - rapidFireValue) > 0.001f)
                            {
                                InternalMemory.Write(attributesAddr2 + Offsets.FastFire, rapidFireValue);
                            }
                        }
                        lastRapidFireState = true;
                    }
                    else if (lastRapidFireState)
                    {
                        if (originalFireIntervalValues.TryGetValue(addrKey, out var savedValue))
                        {
                            InternalMemory.Write(attributesAddr2 + Offsets.FastFire, savedValue);
                        }
                        else
                        {
                            InternalMemory.Write(attributesAddr2 + Offsets.FastFire, 1.0f);
                        }
                        lastRapidFireState = false;
                    }
                }

                var liveEntitySet = new HashSet<uint>();
                foreach (var entity in GetEntities(currentGame, Offsets.DictionaryEntities))
                {
                    if (entity != 0 && entity != localPlayer)
                        liveEntitySet.Add(entity);
                }

                var staleKeys = Core.Entities.Keys
                    .Where(k => !liveEntitySet.Contains((uint)k))
                    .ToList();
                foreach (var staleKey in staleKeys)
                    Core.Entities.TryRemove(staleKey, out _);

                foreach (var entity in liveEntitySet)
                {
                    if (entity == 0) continue;
                    if (entity == localPlayer) continue;

                    Entity player;

                    if (Core.Entities.TryGetValue(entity, out player))
                    {
                        player.Address = entity;

                        if (player.IsTeam == Bool3.True) continue;

                        if (player.IsTeam == Bool3.Unknown)
                        {
                            var rAvatarManager = InternalMemory.Read<uint>(entity + Offsets.AvatarManager, out var avatarManager);

                            if (rAvatarManager && avatarManager != 0)
                            {
                                var rAvatar = InternalMemory.Read<uint>(avatarManager + Offsets.Avatar, out var avatar);

                                if (rAvatar && avatar != 0)
                                {
                                    var rIsVisible = InternalMemory.Read<bool>(avatar + Offsets.Avatar_IsVisible, out var isVisible);

                                    if (rIsVisible && isVisible)
                                    {
                                        var rAvatarData = InternalMemory.Read<uint>(avatar + Offsets.Avatar_Data, out var avatarData);

                                        if (rAvatarData && avatarData != 0)
                                        {
                                            var rIsTeam = InternalMemory.Read<bool>(avatarData + Offsets.Avatar_Data_IsTeam, out var isTeam);
                                            if (rIsTeam)
                                            {
                                                if (isTeam)
                                                {
                                                    player.IsTeam = Bool3.True;
                                                }
                                                else
                                                {
                                                    player.IsTeam = Bool3.False;
                                                    player.IsKnown = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (!player.IsKnown) continue;

                        if (Config.IgnoreKnocked)
                        {
                            if (InternalMemory.Read<uint>(entity + Offsets.Player_ShadowBase, out var shadowBase))
                            {
                                if (shadowBase != 0)
                                {
                                    if (InternalMemory.Read<int>(shadowBase + Offsets.XPose, out var xpose))
                                    {
                                        player.IsKnocked = xpose == 8;
                                    }
                                }
                            }
                        }

                        var rIsDead = InternalMemory.Read<bool>(entity + Offsets.Player_IsDead, out var isDead);

                        if (rIsDead)
                        {
                            player.IsDead = isDead;
                        }

                        if (Config.ESPName || Config.ESPName)
                        {
                            var rNameAddr = InternalMemory.Read<uint>(entity + Offsets.Player_Name, out var nameAddr);
                            if (rNameAddr && nameAddr != 0)
                            {
                                var rNameLen = InternalMemory.Read<int>(nameAddr + 0x8, out var nameLen);
                                if (rNameLen)
                                {
                                    if (nameLen > 0)
                                    {

                                        int charCount = Math.Min(nameLen, 64);
                                        var rawName = InternalMemory.ReadString(nameAddr + 0xC, charCount * 2);
                                        if (!string.IsNullOrEmpty(rawName) && player != null)
                                        {
                                            var cleanChars = rawName.Where(c => c >= 32 && c <= 126).ToArray();
                                            var name = new string(cleanChars).Trim();
                                            if (!string.IsNullOrEmpty(name))
                                            {
                                                player.Name = name;
                                            }
                                        }
                                    }
                                }
                            }
                            if (Config.espweapon || Config.espweapon)
                            {
                                var rDataPool2 = InternalMemory.Read<uint>(entity + Offsets.Player_Data, out var dataPool2);
                                if (rDataPool2 && dataPool2 != 0)
                                {
                                    var rPoolObj2 = InternalMemory.Read<uint>(dataPool2 + 0x8, out var poolObj2);
                                    if (rPoolObj2 && poolObj2 != 0)
                                    {
                                        var rPool2 = InternalMemory.Read<uint>(poolObj2 + 0x20, out var pool2);
                                        if (rPool2 && pool2 != 0)
                                        {
                                            var rweaponid = InternalMemory.Read<short>(pool2 + 0x10, out var weaponid);
                                            if (rweaponid && weaponid != 0)
                                            {
                                                var weaponName = WeaponIconData.GetIconForWeaponId((uint)weaponid);
                                                player.WeaponName = weaponName;
                                            }
                                        }
                                    }
                                }
                            }

                            var rDataPool = InternalMemory.Read<uint>(entity + Offsets.Player_Data, out var dataPool);
                            if (rDataPool && dataPool != 0)
                            {
                                var rPoolObj = InternalMemory.Read<uint>(dataPool + 0x8, out var poolObj);
                                if (rPoolObj && poolObj != 0)
                                {
                                    var rPool = InternalMemory.Read<uint>(poolObj + 0x10, out var pool);
                                    if (rPool && pool != 0)
                                    {
                                        var rHealthAddr = InternalMemory.Read<uint>(pool + 0xC, out var healthAddr);
                                        if (rHealthAddr && healthAddr != 0)
                                        {
                                            var rHealth = InternalMemory.Read<short>(healthAddr, out var health);

                                            if (rHealth && player != null)
                                            {
                                                player.Health = health;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        bool iisdone = false;
                        var rSpeedHack = InternalMemory.Read<uint>(currentGame + Offsets.GameTimer, out var SpeedHackk);
                        var rSpeed = InternalMemory.Read<float>(SpeedHackk + Offsets.FixedDeltaTime, out var Speedd);

                        if (Config.ESPHealth)
                        {
                            var rDataPool = InternalMemory.Read<uint>(entity + Offsets.Player_Data, out var dataPool);
                            if (rDataPool && dataPool != 0)
                            {
                                var rPoolObj = InternalMemory.Read<uint>(dataPool + 0x8, out var poolObj);
                                if (rPoolObj && poolObj != 0)
                                {
                                    var rPool = InternalMemory.Read<uint>(poolObj + 0x10, out var pool);
                                    if (rPool && pool != 0)
                                    {
                                        var rHealthAddr = InternalMemory.Read<short>(pool + 0x10, out var Health);
                                        if (rHealthAddr && Health != 0)
                                        {
                                            if (player != null)
                                            {
                                                player.Health = Health;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var rHeadBone = InternalMemory.Read<uint>(entity + (uint)Bones.Head, out var headBone);

                        if (rHeadBone && headBone != 0)
                        {
                            var rHeadTrans = Transform.GetNodePosition(headBone, out var headTransform);

                            if (rHeadTrans)
                            {
                                player.Head = headTransform;
                                player.Distance = Vector3.Distance(mainPos, headTransform);
                            }
                        }

                        var rRootBone = InternalMemory.Read<uint>(entity + (uint)Bones.Root, out var rootBone);
                        if (rRootBone && rootBone != 0)
                        {
                            var rRootTrans = Transform.GetNodePosition(rootBone, out var rootTransform);

                            if (rRootTrans)
                            {
                                player.Root = rootTransform;
                            }
                        }
                        var boneOffsets = new[]
                        {
                            Bones.Head, Bones.LeftWrist, Bones.Spine,
                            Bones.Hip, Bones.Root, Bones.RightCalf, Bones.LeftCalf,
                            Bones.RightFoot, Bones.LeftFoot, Bones.RightWrist, Bones.LeftHand,
                            Bones.LeftShoulder, Bones.RightShoulder, Bones.LeftWristJoint,
                            Bones.LeftElbow, Bones.RightElbow
                        };
                        foreach (var offset in boneOffsets)
                        {
                            var rBone = InternalMemory.Read<uint>(entity + (uint)offset, out var bone);
                            if (rBone && bone != 0)
                            {
                                var rBonePos = Transform.GetNodePosition(bone, out var boneTransform);
                                if (rBonePos)
                                {
                                    switch (offset)
                                    {
                                        case Bones.Head:
                                            player.Head = boneTransform;
                                            break;
                                        case Bones.LeftWrist:
                                            player.LeftWrist = boneTransform;
                                            break;
                                        case Bones.Spine:
                                            player.Spine = boneTransform;
                                            break;
                                        case Bones.Hip:
                                            player.Hip = boneTransform;
                                            break;
                                        case Bones.Root:
                                            player.Root = boneTransform;
                                            break;
                                        case Bones.RightCalf:
                                            player.RightCalf = boneTransform;
                                            break;
                                        case Bones.LeftCalf:
                                            player.LeftCalf = boneTransform;
                                            break;
                                        case Bones.RightFoot:
                                            player.RightFoot = boneTransform;
                                            break;
                                        case Bones.LeftFoot:
                                            player.LeftFoot = boneTransform;
                                            break;
                                        case Bones.RightWrist:
                                            player.RightWrist = boneTransform;
                                            break;
                                        case Bones.LeftHand:
                                            player.LeftHand = boneTransform;
                                            break;
                                        case Bones.LeftShoulder:
                                            player.LeftShoulder = boneTransform;
                                            break;
                                        case Bones.RightShoulder:
                                            player.RightShoulder = boneTransform;
                                            break;
                                        case Bones.LeftWristJoint:
                                            player.LeftWristJoint = boneTransform;
                                            break;
                                        case Bones.RightElbow:
                                            player.RightElbow = boneTransform;
                                            break;
                                        case Bones.LeftElbow:
                                            player.LeftElbow = boneTransform;
                                            break;
                                    }
                                    player.Distance = Vector3.Distance(Core.LocalMainCamera, player.Head);
                                }
                            }
                        }
                    }
                    else
                    {
                        Core.Entities[entity] = new Entity
                        {
                            IsTeam = Bool3.Unknown,
                            IsKnown = false,
                            IsDead = false,
                            Health = 0,
                            IsKnocked = false,
                            Head = Vector3.Zero,
                            LeftWrist = Vector3.Zero,

                            Spine = Vector3.Zero,

                            Root = Vector3.Zero,
                            Hip = Vector3.Zero,
                            RightCalf = Vector3.Zero,
                            LeftCalf = Vector3.Zero,
                            RightFoot = Vector3.Zero,
                            LeftFoot = Vector3.Zero,
                            RightWrist = Vector3.Zero,
                            LeftHand = Vector3.Zero,
                            RightShoulder = Vector3.Zero,
                            RightWristJoint = Vector3.Zero,
                            LeftWristJoint = Vector3.Zero,
                            RightElbow = Vector3.Zero,
                            LeftElbow = Vector3.Zero,
                            Name = ""         
                        };
                    }
                }

                Thread.Sleep(Core.HaveMatrix ? 8 : 22);
            }
        }
        static List<uint> GetEntities(uint baseGame, uint offset)
        {
            List<uint> entityList = new List<uint>();

            if (!InternalMemory.Read<uint>(baseGame + offset, out uint dict) || dict == 0)
                return entityList;

            if (!InternalMemory.Read<int>(dict + 0x10, out int count) || count < 1 || count > 10000)
                return entityList;

            if (!InternalMemory.Read<uint>(dict + 0xC, out uint entries) || entries == 0)
                return entityList;

            uint start = entries + 0x10;

            for (uint i = 0; i < count; i++)
            {
                uint entry = start + (i * 0x10);

                if (!InternalMemory.Read<int>(entry + 0x0, out int hash) || hash < 0)
                    continue;

                if (!InternalMemory.Read<uint>(entry + 0xC, out uint entity) || entity == 0)
                    continue;

                entityList.Add(entity);
            }

            return entityList;
        }
        static void ResetCache()
        {
            Core.HaveMatrix = false;
            Core.LocalPlayer = 0;
            Core.Entities.Clear();
            InternalMemory.ClearCache();
        }
    }
}