using AotForms;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;

namespace AotForms
{
    internal static class Core
    {
        internal static IntPtr Handle;
        internal static IntPtr Overlay;
        internal static int Width = -1;
        internal static int Height = -1;
        internal static bool HaveMatrix = false;
        internal static Matrix4x4 CameraMatrix;
        internal static ulong LocalPlayer;
        internal static Vector3 LocalMainCamera;
        public static ConcurrentDictionary<long, Entity> Entities = new();
        internal static Vector4 tantanviscoaim;

        internal static IntPtr BaseAddress;

        public static Vector3 ForwardVector = new Vector3(0, 0, 1);

        public static void UpdateForwardVector()
        {
            if (!InternalMemory.Read(LocalPlayer + Offsets.AimRotation, out float yaw))
                return;

            if (!InternalMemory.Read(LocalPlayer + Offsets.sAim4, out float pitch)) 
                pitch = 0f;

            Vector3 forward = new(
                MathF.Cos(pitch) * MathF.Cos(yaw),
                MathF.Sin(pitch),
                MathF.Cos(pitch) * MathF.Sin(yaw)
            );

            ForwardVector = Vector3.Normalize(forward);
        }

        public static bool AttachProcess(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                return false;

            Process proc = processes[0];
            Handle = proc.Handle;
            BaseAddress = proc.MainModule.BaseAddress; 

            return true;
        }

        internal static Matrix4x4 GetCameraMatrix()
        {
            return _cameraMatrix;
        }

        internal static void SetCameraMatrix(Matrix4x4 value)
        {
            CameraMatrix = value;
            UpdateCameraVectors();
        }
        private static void UpdateCameraVectors()
        {
            _cameraForward = new Vector3(_cameraMatrix.M13, _cameraMatrix.M23, _cameraMatrix.M33);
            _cameraForward = Vector3.Normalize(_cameraForward);

            _cameraRight = new Vector3(_cameraMatrix.M11, _cameraMatrix.M21, _cameraMatrix.M31);
            _cameraRight = Vector3.Normalize(_cameraRight);
        }
        private static Matrix4x4 _cameraMatrix;
        internal static Vector3 CameraForward => _cameraForward;
        internal static Vector3 CameraRight => _cameraRight;
        private static Vector3 _cameraForward;
        private static Vector3 _cameraRight;
    }
}