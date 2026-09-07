using AotForms;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace AotForms
{
    public static class RapidSpin
    {
        private static Task? _spinLoopTask;
        private static CancellationTokenSource? _cts;
        private static volatile bool _enabled;

        private const int LoopDelayMs = 5;

        public static bool IsActive => _enabled;

        public static void Activate()
        {
            if (_enabled)
                return;

            _enabled = true;

            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            _spinLoopTask = Task.Run(() => SpinLoopAsync(_cts.Token), _cts.Token);
        }

        public static void Deactivate()
        {
            if (!_enabled)
                return;

            try
            {
                _cts?.Cancel();
            }
            catch
            {
            }

            _enabled = false;
            _cts?.Dispose();
            _cts = null;
        }

        private static async Task SpinLoopAsync(CancellationToken cancelToken)
        {
            float currentYaw = 0f;

            while (!cancelToken.IsCancellationRequested)
            {
                try
                {
                    if (!Config.spinbot)
                    {
                        currentYaw = 0f;
                        await Task.Delay(LoopDelayMs, cancelToken);
                        continue;
                    }

                    ulong rawLocal = Core.LocalPlayer;
                    if (rawLocal == 0)
                    {
                        await Task.Delay(12, cancelToken);
                        continue;
                    }

                    uint localPlayer = (uint)rawLocal;
                    if (!InternalMemory.Read(localPlayer + (uint)Bones.Root, out uint root) || root == 0)
                        goto next;

                    if (!InternalMemory.Read(root + 0x8, out uint t1) || t1 == 0)
                        goto next;

                    if (!InternalMemory.Read(t1 + 0x8, out uint t2) || t2 == 0)
                        goto next;
                    if (!InternalMemory.Read(t2 + 0x24, out uint indexValue))
                        goto next;

                    if (!InternalMemory.Read(t2 + 0x20, out uint matrixValue) || matrixValue == 0)
                        goto next;
                    if (!InternalMemory.Read(matrixValue + 0x18, out uint matrixListValue) || matrixListValue == 0)
                        goto next;
                    uint targetAddress = matrixListValue + (indexValue * 0x30);
                    if (!InternalMemory.Read(targetAddress, out Vector4 currentPos))
                        goto next;
                    currentYaw += Config.SpinSpeed;
                    if (currentYaw >= 360f)
                        currentYaw -= 360f;

                    Quaternion rot = Quaternion.CreateFromAxisAngle(
                        Vector3.UnitY,
                        currentYaw * (MathF.PI / 180f)
                    );
                    InternalMemory.Write(targetAddress + 0x10, rot);
                }
                catch
                {
                }

            next:
                await Task.Delay(LoopDelayMs, cancelToken);
            }
        }

        public static void Shutdown()
        {
            Deactivate();
            _spinLoopTask?.Wait(250);
            _spinLoopTask = null;
        }
    }
}