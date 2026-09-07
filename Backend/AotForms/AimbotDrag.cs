using ImGuiNET;
using System;
using System.Numerics;
using System.Threading;

namespace AotForms
{
    internal static class AimbotDrag
    {
        private static CancellationTokenSource cts = new();
        private static bool isRunning = false;
        private static WinAPI.POINT lastMousePos;
        private static DateTime lastDragTime = DateTime.MinValue;
        private static DateTime lastPollTime = DateTime.MinValue;

        private static bool IsDraggingMouse()
        {
            WinAPI.POINT currentMousePos;
            if (!WinAPI.GetCursorPos(out currentMousePos))
                return false;

            if ((DateTime.Now - lastPollTime).TotalMilliseconds < 10)
            {
                return (DateTime.Now - lastDragTime).TotalMilliseconds < 120;
            }
            lastPollTime = DateTime.Now;

            int dx = currentMousePos.X - lastMousePos.X;
            int dy = currentMousePos.Y - lastMousePos.Y;
            lastMousePos = currentMousePos;

            float dist = MathF.Sqrt(dx * dx + dy * dy);

            if (dist >= Config.DragThreshold && dist < 150f)
            {
                lastDragTime = DateTime.Now;
                return true;
            }

            return (DateTime.Now - lastDragTime).TotalMilliseconds < 120;
        }

        internal static void Work()
        {
            WinAPI.GetCursorPos(out lastMousePos);

            while (true)
            {
                if (!Config.AimbotDrag)
                {
                    Thread.Sleep(10);
                    continue;
                }

                if (Core.Width == -1 || Core.Height == -1 || !Core.HaveMatrix)
                {
                    Thread.Sleep(1);
                    continue;
                }

                if (IsDraggingMouse())
                {
                    Entity target = FindBestTarget();
                    if (target != null)
                    {
                        AimAtTarget(target);
                    }
                }

                Thread.Sleep(1);
            }
        }

        private static Entity FindBestTarget()
        {
            Entity bestTarget = null;
            float closestDistance = float.MaxValue;
            var screenCenter = new Vector2(Core.Width / 2f, Core.Height / 2f);

            foreach (var entity in Core.Entities.Values)
            {
                if (entity.IsDead) continue;

                if (Config.IgnoreKnocked && entity.IsKnocked)
                    continue;

                var head2D = W2S.WorldToScreen(Core.CameraMatrix, entity.Head, Core.Width, Core.Height);
                if (head2D.X < 1 || head2D.Y < 1) continue;

                float playerDistance = Vector3.Distance(Core.LocalMainCamera, entity.Head);
                if (playerDistance > Config.AimBotMaxDistance) continue;

                float crosshairDistance = Vector2.Distance(screenCenter, head2D);

                if (crosshairDistance < closestDistance && crosshairDistance <= Config.AimFov)
                {
                    closestDistance = crosshairDistance;
                    bestTarget = entity;
                }
            }

            return bestTarget;
        }

        private static void AimAtTarget(Entity target)
        {
            if (target == null || target.Address == 0) return;

            uint m_HeadCollider;
            var rHeadCollider = InternalMemory.Read<uint>(target.Address + 0x4a4, out m_HeadCollider);
            if (!rHeadCollider || m_HeadCollider == 0) return;

            const int repeatCount = 10;

            for (int i = 0; i < repeatCount; i++)
            {
                InternalMemory.Write(target.Address + Offsets.adarshcheat, m_HeadCollider);
            }
            InternalMemory.Write(target.Address + Offsets.adarshcheat, m_HeadCollider);
        }

        internal static void Render()
        {
            if (!Config.AimbotDrag || Core.Width == -1 || Core.Height == -1)
                return;

            var screenCenter = new Vector2(Core.Width / 2f, Core.Height / 2f);

            ImGui.GetBackgroundDrawList().AddCircle(
                screenCenter,
                Config.AimFov, 
                ImGui.GetColorU32(new Vector4(1f, 0f, 0f, 0.8f)), 
                64, 
                2f  
            );
        }

        internal static void Stop()
        {
            if (!isRunning) return;

            cts.Cancel();
            cts = new CancellationTokenSource();
            isRunning = false;
        }
    }
}
