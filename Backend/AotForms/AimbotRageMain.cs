using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AotForms
{
    internal static class AimbotRageMain
    {
        private static Vector3    _lastFinalPos    = Vector3.Zero;
        private static Entity     _lastTarget      = null;
        private static int        _lockCounter     = 0;
        private const  int        LOCK_FRAMES_MIN  = 2;
        private static Quaternion _currentRot      = Quaternion.Identity;
        private static bool       _rotInitialized  = false;

        internal static void Work()
        {
            while (true)
            {
                if (!Config.AimBotRageMainId)
                {
                    Thread.Sleep(1);
                    _ResetState();
                    continue;
                }

                if ((WinAPI.GetAsyncKeyState(Config.AimbotKey) & 0x8000) == 0)
                {
                    Thread.Sleep(1);
                    _ResetState();
                    continue;
                }

                if (Core.Width == -1 || Core.Height == -1) { Thread.Sleep(1); continue; }
                if (!Core.HaveMatrix)                       { Thread.Sleep(1); continue; }

                var screenCenter = new Vector2(Core.Width / 2f, Core.Height / 2f);

                Entity  bestEntity    = null;
                Vector3 bestBonePos   = Vector3.Zero;
                float   bestCrossDist = float.MaxValue;

                var snapshot = Core.Entities.Values.ToArray();

                foreach (var entity in snapshot)
                {
                    if (!entity.IsKnown) continue;
                    if (entity.IsDead)   continue;
                    if (entity.IsTeam == Bool3.True) continue;

                    if (Config.IgnoreKnocked && entity.IsKnocked) continue;

                    float worldDist = Vector3.Distance(Core.LocalMainCamera, entity.Head);
                    if (worldDist > Config.AimBotMaxDistance) continue;

                    Vector3 aimPos = _SelectBestBone(entity);

                    var screen = W2S.WorldToScreen(Core.CameraMatrix, aimPos, Core.Width, Core.Height);
                    if (screen.X < 1 || screen.Y < 1) continue;

                    float dx   = screen.X - screenCenter.X;
                    float dy   = screen.Y - screenCenter.Y;
                    float dist = MathF.Sqrt(dx * dx + dy * dy);

                    if (dist > Config.AimFov) continue;

                    if (dist < bestCrossDist)
                    {
                        bestCrossDist = dist;
                        bestEntity    = entity;
                        bestBonePos   = aimPos;
                    }
                }

                if (bestEntity != null)
                {
                    if (bestEntity != _lastTarget)
                    {
                        _lockCounter++;
                        if (_lockCounter < LOCK_FRAMES_MIN)
                        {
                            if (_lastTarget != null && _lastTarget.IsKnown && !_lastTarget.IsDead)
                            {
                                bestEntity  = _lastTarget;
                                bestBonePos = _SelectBestBone(_lastTarget);
                            }
                        }
                        else
                        {
                            _lastTarget  = bestEntity;
                            _lockCounter = 0;
                        }
                    }
                    else
                    {
                        _lockCounter = 0;
                    }
                }
                else
                {
                    _ResetState();
                    Thread.Sleep(1);
                    continue;
                }

                if (bestEntity != null)
                {
                    _lastFinalPos = bestBonePos;

                    var targetRot = MathUtils.GetRotationToLocation(bestBonePos, 0.1f, Core.LocalMainCamera);

                    float lerpT = _GetLerpFactor();

                    if (InternalMemory.Read<Quaternion>(Core.LocalPlayer + Offsets.AimRotation, out var actualRot))
                    {
                        _currentRot = Quaternion.Slerp(actualRot, targetRot, lerpT);
                    }
                    else
                    {
                        _currentRot = targetRot;
                    }

                    InternalMemory.Write(Core.LocalPlayer + Offsets.AimRotation, _currentRot);

                    int sleepMs = Math.Max(1, Config.Smoothness);
                    Thread.Sleep(sleepMs);
                }
            }
        }

        private static Vector3 _SelectBestBone(Entity e)
        {
            return Config.AimTargetPart switch
            {
                "HEAD"      => e.Head,
                "NECK"      => e.Spine,  
                "CHEST"     => e.Hip,    
                "LSHOULDER" => e.LeftShoulder,
                "RSHOULDER" => e.RightShoulder,
                _           => e.Head
            };
        }

        private static float _GetLerpFactor()
        {
            float smooth = Math.Clamp(Config.AimBotSmooth, 1f, 50f);
            return 1f / smooth;
        }

        private static void _ResetState()
        {
            _lastTarget     = null;
            _lockCounter    = 0;
            _lastFinalPos   = Vector3.Zero;
            _rotInitialized = false;
        }
    }
}