

namespace AotForms
{
    internal static class EnemyPullHelpers
    {
        internal static bool IsLocalFiring()
        {
            if (Core.LocalPlayer == 0)
                return false;

            if (InternalMemory.Read(Core.LocalPlayer + Offsets.IsFiring, out bool isFiring) && isFiring)
                return true;

            return false;
        }

        internal static float GetPullFovRadius()
        {
            return Config.AimFov > 0f ? Config.AimFov : 200f;
        }
    }
}