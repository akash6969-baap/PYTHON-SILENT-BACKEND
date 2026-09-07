using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace AotForms
{
    internal static class DownPlayer
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static Task tpTask = null;
        private static float teleportDownDistance = 3.5f;
        private static int teleportDelay = 20;
        private static bool _originalSlideValue;
        private static bool _slideValueSaved;

        private static ConcurrentDictionary<uint, Vector3> originalEnemyPositions = new ConcurrentDictionary<uint, Vector3>();
        private static ConcurrentDictionary<uint, uint> enemyMatrixPtrs = new ConcurrentDictionary<uint, uint>();
        private static Entity currentEnemyTarget = null;

        public static void Work()
        {
            if (Config.DownPlayer || Config.undergroundkill)
            {
                StartTeleportTask();
            }
            else
            {
                StopTeleportTask();
            }
        }

        public static void SetTeleportDown(float distance)
        {
            teleportDownDistance = distance;
        }

        public static void SetTeleportDelay(int delay)
        {
            teleportDelay = delay;
        }

        private static void StartTeleportTask()
        {
            if (cts != null && cts.IsCancellationRequested)
                cts = new CancellationTokenSource();
            else if (cts == null)
                cts = new CancellationTokenSource();

            if (tpTask != null && !tpTask.IsCompleted)
                return; 

            tpTask = Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    if (!Config.DownPlayer && !Config.undergroundkill)
                    {
                        await Task.Delay(50, cts.Token);
                        continue;
                    }

                    try
                     {
                        ulong lp = Core.LocalPlayer;
                        if (lp == 0)
                        {
                            await Task.Delay(50, cts.Token);
                            continue;
                        }

                        if (InternalMemory.Read<bool>(lp + Offsets.Player_IsDead, out bool dead) && dead)
                        {
                            RestoreSlideState(lp);
                            await Task.Delay(100, cts.Token);
                            continue;
                        }

                        if (Config.undergroundkill && Offsets.InSnowSlideWayDashing != 0)
                        {
                            if (!_slideValueSaved)
                            {
                                if (InternalMemory.Read<bool>(lp + Offsets.InSnowSlideWayDashing, out _originalSlideValue))
                                    _slideValueSaved = true;
                            }
                            InternalMemory.Write<bool>(lp + Offsets.InSnowSlideWayDashing, true);
                        }

                        if (InternalMemory.Read<uint>(lp + (uint)Bones.Root, out uint rootPtr) && rootPtr != 0)
                        {
                            if (InternalMemory.Read<uint>(rootPtr + 0x8, out uint t1) && t1 != 0)
                            {
                                if (InternalMemory.Read<uint>(t1 + 0x8, out uint t2) && t2 != 0)
                                {
                                    if (InternalMemory.Read<uint>(t2 + 0x20, out uint matrixPtr) && matrixPtr != 0)
                                    {
                                        if (InternalMemory.Read<Vector3>(matrixPtr + 0x60, out Vector3 currentPos))
                                        {
                                            Vector3 downPos = currentPos;

                                            downPos.Y -= Config.undergroundkill ? 0.3f : 2.2f;
                                            InternalMemory.Write<Vector3>(matrixPtr + 0x60, downPos);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch { }

                    await Task.Delay(16, cts.Token);
                }
            }, cts.Token);
        }

        private static void RestoreSlideState(ulong lp)
        {
            if (_slideValueSaved && Offsets.InSnowSlideWayDashing != 0)
            {
                InternalMemory.Write<bool>(lp + Offsets.InSnowSlideWayDashing, _originalSlideValue);
                _slideValueSaved = false;
            }
        }

        private static void StopTeleportTask()
        {
            try
            {
                ulong lp = Core.LocalPlayer;
                if (lp != 0) RestoreSlideState(lp);
            }
            catch { }
            try { cts?.Cancel(); } catch { }
        }
    }
}