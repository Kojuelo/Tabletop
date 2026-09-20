using System;
using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public static class AngleUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RadiansToDegrees(float radians)
        {
            return radians * (180f / MathF.PI);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DegreesToRadians(float degrees)
        {
            return degrees * (MathF.PI / 180f);
        }

        public static float RadiansModuloPI(float radians)
        {
            if (radians >= 0f)
            {
                if (radians > MathF.PI)
                {
                    return (radians % MathF.PI) - MathF.PI;
                }
            }
            else
            {
                if (radians <= -MathF.PI)
                {
                    return radians % MathF.PI;
                }
            }

            return radians;
        }

        public static float DegreesModulo180(float degrees)
        {
            if (degrees >= 0f)
            {
                if (degrees > 180f)
                {
                    return (degrees % 180f) - 180f;
                }
            }
            else
            {
                if (degrees <= -180f)
                {
                    return degrees % 180f;
                }
            }

            return degrees;
        }
    }
}