using System;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace AotForms
{
    internal static class AimbotV2
    {

        private static Quaternion _smoothRot      = Quaternion.Identity;
        private static Entity     _lockedTarget   = null;
        private static int        _lockFrames     = 0;
        private const  int        LOCK_MIN_FRAMES = 2;   

        internal static void Work()
        {
            while (true)
            {
                if (!Config.enableAimBot)
                {
                    Thread.Sleep(1);
                    _Reset();
                    continue;
                }

                if ((WinAPI.GetAsyncKeyState(Config.AimbotKey) & 0x8000) == 0)
                {
                    Thread.Sleep(1);
                    _Reset();
                    continue;
                }

                if (Core.Width == -1 || Core.Height == -1 || !Core.HaveMatrix)
                {
                    Thread.Sleep(1);
                    continue;
                }

                Entity best = _GetBestTarget();

                if (best != null && best != _lockedTarget)
                {
                    _lockFrames++;
                    if (_lockFrames < LOCK_MIN_FRAMES && _lockedTarget != null
                        && _lockedTarget.IsKnown && !_lockedTarget.IsDead)
                    {
                        best = _lockedTarget;   
                    }
                    else
                    {
                        _lockedTarget = best;
                        _lockFrames   = 0;
                    }
                }
                else if (best != null)
                {
                    _lockFrames = 0;
                }

                if (best == null)
                {
                    _Reset();
                    Thread.Sleep(1);
                    continue;
                }

                var targetRot = MathUtils.GetRotationToLocation(best.Head, 0.1f, Core.LocalMainCamera);

                float lerpT = _GetLerpFactor();

                if (InternalMemory.Read<Quaternion>(Core.LocalPlayer + Offsets.AimRotation, out var curRot))
                    _smoothRot = Quaternion.Slerp(curRot, targetRot, lerpT);
                else
                    _smoothRot = targetRot;

                InternalMemory.Write(Core.LocalPlayer + Offsets.AimRotation, _smoothRot);

                Thread.Sleep(Math.Max(1, (int)Config.AimBotSmooth));
            }
        }

        private static Entity _GetBestTarget()
        {
            Entity best      = null;
            float  bestDist  = float.MaxValue;
            var    center    = new Vector2(Core.Width / 2f, Core.Height / 2f);
            var    snapshot  = Core.Entities.Values.ToArray();

            foreach (var e in snapshot)
            {
                if (!e.IsKnown)                                      continue;
                if (e.IsDead)                                        continue;
                if (Config.IgnoreKnocked && e.IsKnocked)             continue;
                if (Config.AimbotVisible  && !e.isVisible)           continue;

                float worldDist = Vector3.Distance(Core.LocalMainCamera, e.Head);
                if (worldDist > Config.AimBotMaxDistance)            continue;

                var screen = W2S.WorldToScreen(Core.CameraMatrix, e.Head, Core.Width, Core.Height);
                if (screen.X < 1 || screen.Y < 1)                   continue;

                float dx   = screen.X - center.X;
                float dy   = screen.Y - center.Y;
                float dist = MathF.Sqrt(dx * dx + dy * dy);
                if (dist > Config.AimFov)                            continue;

                if (dist < bestDist)
                {
                    bestDist = dist;
                    best     = e;
                }
            }

            return best;
        }

        private static float _GetLerpFactor()
        {
            float smooth = Math.Clamp(Config.AimBotSmooth, 1f, 50f);
            return 1f / smooth;
        }

        private static void _Reset()
        {
            _lockedTarget = null;
            _lockFrames   = 0;
            _smoothRot    = Quaternion.Identity;
        }
    }
}