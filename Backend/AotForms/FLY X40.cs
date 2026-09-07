using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace AotForms
{
    internal static class FLYHACKX40
    {
        private static Task tpTask;
        private static CancellationTokenSource cts = new();
        private static bool isRunning = false;

        private static readonly float MAX_ALTITUDE = 7.5f;   
        private static float groundY = 0f;                   
        private static bool isInitialized = false;
        private static int teleportDelay = 1;

        private static bool originalSlideValue = false;
        private static bool slideValueSaved = false;

        internal static void Work()
        {
            if (isRunning) return;
            isRunning = true;

            if (cts != null && cts.IsCancellationRequested)
                cts = new CancellationTokenSource();
            else if (cts == null)
                cts = new CancellationTokenSource();

            tpTask = Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {

                    if (!Config.FLYHACKX400)
                    {
                        if (isInitialized)
                        {
                            RestoreNormalPosition();
                            isInitialized = false;
                            groundY = 0;
                            slideValueSaved = false;
                        }
                        await Task.Delay(500, cts.Token);
                        continue;
                    }

                    try
                    {
                        ulong localPlayer = Core.LocalPlayer;
                        if (localPlayer == 0)
                        {
                            await Task.Delay(10, cts.Token);
                            continue;
                        }

                        if (!InternalMemory.Read<uint>(localPlayer + (uint)Bones.Root, out uint rootPtr) || rootPtr == 0) continue;
                        if (!InternalMemory.Read<uint>(rootPtr + 0x8, out uint t1) || t1 == 0) continue;
                        if (!InternalMemory.Read<uint>(t1 + 0x8, out uint t2) || t2 == 0) continue;
                        if (!InternalMemory.Read<uint>(t2 + 0x20, out uint matrixPtr) || matrixPtr == 0) continue;

                        if (Config.FlyHack80x) Config.FlyHack80x = false;
                        if (Config.FlyHack50x) Config.FlyHack50x = false;

                        if (!InternalMemory.Read<Vector3>(matrixPtr + 0x60, out Vector3 currentPos)) continue;

                        if (!isInitialized)
                        {
                            groundY = currentPos.Y;
                            isInitialized = true;
                        }

                        if (Config.TeleportAnywhere || Config.teleportmap || Config.directteleport)
                        {
                            await Task.Delay(10, cts.Token);
                            continue;
                        }

                        float targetAltitude = groundY + MAX_ALTITUDE;
                        Vector3 targetPos = currentPos;

                        if (targetPos.Y < groundY)
                            targetPos.Y = groundY;
                        else if (targetPos.Y > targetAltitude)
                            targetPos.Y = targetAltitude;
                        else
                            targetPos.Y = targetAltitude;

                        for (int i = 0; i < 3; i++)
                        {
                            InternalMemory.Write<Vector3>(matrixPtr + 0x60, targetPos);
                        }
                        InternalMemory.Write<float>(matrixPtr + 0x60 + 0xC, 1.0f);

                        if (InternalMemory.Read<ulong>(localPlayer + 0x124C, out ulong moveComp) && moveComp != 0)
                        {
                            InternalMemory.Write<byte>(moveComp + 0x150, 0); 
                            InternalMemory.Write<float>(moveComp + 0x2C, 0.0f); 
                        }

                        if (Offsets.InSnowSlideWayDashing != 0)
                        {
                            if (!slideValueSaved)
                            {
                                if (InternalMemory.Read<bool>(localPlayer + Offsets.InSnowSlideWayDashing, out originalSlideValue))
                                {
                                    slideValueSaved = true;
                                }
                            }
                            InternalMemory.Write<bool>(localPlayer + Offsets.InSnowSlideWayDashing, true);
                        }
                    }
                    catch { }

                    await Task.Delay(teleportDelay, cts.Token);
                }
            }, cts.Token);
        }

        private static void RestoreNormalPosition()
        {
            try
            {
                ulong localPlayer = Core.LocalPlayer;
                if (localPlayer == 0) return;

                if (InternalMemory.Read<uint>(localPlayer + (uint)Bones.Root, out uint rootPtr) &&
                    InternalMemory.Read<uint>(rootPtr + 0x8, out uint t1) &&
                    InternalMemory.Read<uint>(t1 + 0x8, out uint t2) &&
                    InternalMemory.Read<uint>(t2 + 0x20, out uint matrixPtr) && matrixPtr != 0)
                {
                    if (InternalMemory.Read<Vector3>(matrixPtr + 0x60, out Vector3 cur))
                    {
                        cur.Y = groundY;
                        InternalMemory.Write<Vector3>(matrixPtr + 0x60, cur);
                    }
                }

                if (slideValueSaved && Offsets.InSnowSlideWayDashing != 0)
                {
                    InternalMemory.Write<bool>(localPlayer + Offsets.InSnowSlideWayDashing, originalSlideValue);
                }
            }
            catch { }
        }

        internal static void Stop()
        {
            if (!isRunning) return;
            try { cts.Cancel(); } catch { }
            RestoreNormalPosition();
            isRunning = false;
            isInitialized = false;
            groundY = 0;
        }

        internal static void SetTeleportDelay(int delay) => teleportDelay = delay;
    }
}