using System;
using System.Numerics;
using System.Threading;

namespace AotForms
{
    internal class SilentAim
    {
        // Tracks whether gun was firing in the previous frame
        // Rising-edge detection: only write memory on the FIRST frame of firing
        private static bool _wasShootingLastFrame = false;
        private static readonly Random _rng = new Random();

        internal static void Work()
        {
            while (true)
            {
                if (!Config.SilentAim && !Config.SilentAimbody)
                {
                    Thread.Sleep(1);
                    continue;
                }

                if (Core.Width == -1 || Core.Height == -1 || !Core.HaveMatrix || Core.LocalPlayer == 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                // --- Pre-read fire state (rising-edge guard) ---
                // Read isShooting early; if not firing, reset edge flag and skip
                bool isShooting = false;
                InternalMemory.Read<bool>(Core.LocalPlayer + Offsets.sAim1, out isShooting);

                if (!isShooting)
                {
                    _wasShootingLastFrame = false;
                    Thread.Sleep(1);
                    continue;
                }

                // Already processed this fire-press — wait for next shot
                if (_wasShootingLastFrame)
                {
                    Thread.Sleep(1);
                    continue;
                }
                _wasShootingLastFrame = true;

                // --- Find closest target within FOV ---
                Entity target = null;
                float minDistance = float.MaxValue;
                var screenCenter = new Vector2(Core.Width / 2f, Core.Height / 2f);
                float fovLimit = Config.AimFov;

                foreach (var entity in Core.Entities.Values)
                {
                    if (!entity.IsKnown || entity.IsDead) continue;
                    if (Config.IgnoreKnocked && entity.IsKnocked) continue;

                    Vector3 targetPos = Config.SilentAim ? entity.Head : entity.Hip;
                    var screen2D = W2S.WorldToScreen(Core.CameraMatrix, targetPos, Core.Width, Core.Height);
                    if (screen2D.X < 1 || screen2D.Y < 1) continue;

                    float dist = Vector2.Distance(screenCenter, new Vector2(screen2D.X, screen2D.Y));
                    if (dist > fovLimit) continue;

                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        target = entity;
                    }
                }

                // --- Write aim direction (once per shot only) ---
                if (target != null)
                {
                    if (InternalMemory.Read<uint>(Core.LocalPlayer + Offsets.sAim2, out uint weaponData) && weaponData != 0)
                    {
                        Vector3 aimPos = Config.SilentAim
                            ? (target.Head + new Vector3(0, 0.1f, 0))
                            : target.Hip;

                        // Micro-jitter: tiny random offset per shot to break pattern detection
                        float jx = (float)(_rng.NextDouble() * 0.04 - 0.02);
                        float jy = (float)(_rng.NextDouble() * 0.04 - 0.02);
                        float jz = (float)(_rng.NextDouble() * 0.04 - 0.02);
                        aimPos += new Vector3(jx, jy, jz);

                        if (InternalMemory.Read<Vector3>(weaponData + Offsets.sAim3, out Vector3 startPos))
                        {
                            InternalMemory.Write<Vector3>(weaponData + Offsets.sAim4, aimPos - startPos);
                        }
                    }
                }

                Thread.Sleep(1);
            }
        }
    }
}