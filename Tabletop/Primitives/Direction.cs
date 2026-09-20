using System;
using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public struct Direction
    {
        private Point _normal;

        private float _radians;

        private float _degrees;
    
    
        public Direction(float x, float y)
        {
            var distance = MathF.Sqrt((x * x) + (y * y));
            if (distance <= 0f)
            {
                _normal = default;
                _radians = default;
                _degrees = default;
                return;
            }

            _normal = new Point(x, y);
            CalculateAnglesFromNormal(_normal, out _radians, out _degrees);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Direction(in Point point)
            : this(point.x, point.y)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Direction(float xFrom, float yFrom, float xTo, float yTo)
            : this(xTo - xFrom, yTo - yFrom)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Direction(in Point from, in Point to)
            : this(from.x, from.y, to.x, to.y)
        {
        }
    
        private Direction(in Point normal, float radians, float degrees)
        {
            _normal = normal;
            _radians = radians;
            _degrees = degrees;
        }

    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Direction a, in Direction b)
        {
            return (a._radians == b._radians) && (a.isZero == b.isZero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Direction a, in Direction b)
        {
            return (a._radians != b._radians) || (a.isZero != b.isZero);
        }


        public static Direction zero => default;

        public static Direction right => new Direction(new Point(1f, 0f), 0f, 0f);

        public static Direction left => new Direction(new Point(-1f, 0f), MathF.PI, 180f);

        public static Direction up => new Direction(new Point(0f, 1f), MathF.PI * 0.5f, 90f);

        public static Direction down => new Direction(new Point(1f, 0f), MathF.PI * -0.5f, -90f);

        public Point normal
        {
            readonly get => _normal;
            set
            {
                if (_normal == value)
                {
                    return;
                }

                _normal = value;
                CalculateAnglesFromNormal(_normal, out _radians, out _degrees);
            }
        }

        public float radians
        {
            readonly get => _radians;
            set
            {
                if (_radians == value)
                {
                    return;
                }

                _radians = value;
                CalculateDegreesAndNormalFromRadians(_radians, out _degrees, out _normal);
            }
        }

        public float degrees
        {
            readonly get => _degrees;
            set
            {
                if (_degrees == value)
                {
                    return;
                }

                _degrees = value;
                CalculateRadiansAndNormalFromDegrees(_degrees, out _radians, out _normal);
            }
        }

        public readonly bool isZero => _normal.isZero;


        public static Direction FromAngleRadians(float radians)
        {
            radians = AngleUtility.RadiansModuloPI(radians);

            CalculateDegreesAndNormalFromRadians(radians, out var degrees, out var normal);
            return new Direction(normal, radians, degrees);
        }

        public static Direction FromAngleDegrees(float degrees)
        {
            degrees = AngleUtility.DegreesModulo180(degrees);

            CalculateDegreesAndNormalFromRadians(degrees, out var radians, out var normal);
            return new Direction(normal, radians, degrees);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Direction GetClosestNormalDirection(in Direction a, in Direction b)
        {
            return a.GetRotatedRadians(a.GetDifferenceRadians(b) * 0.5f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Direction GetFurthestNormalDirection(in Direction a, in Direction b)
        {
            return GetClosestNormalDirection(a, b).GetInverse();
        }

        public void SetZero()
        {
            _normal = default;
            _radians = 0f;
            _degrees = 0f;
        }

        public void RotateRadians(float radians)
        {
            if (isZero)
            {
                return;
            }

            _radians = AngleUtility.RadiansModuloPI(_radians + radians);
            CalculateDegreesAndNormalFromRadians(_radians, out _degrees, out _normal);
        }

        public void RotateDegrees(float degrees)
        {
            if (isZero)
            {
                return;
            }

            _degrees = AngleUtility.DegreesModulo180(_degrees + degrees);
            CalculateRadiansAndNormalFromDegrees(_degrees, out _radians, out _normal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetTranslation(float length)
        {
            return new Point(normal.x * length, normal. y * length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetTranslation(in Point source, float length)
        {
            return new Point(source.x + (normal.x * length), source.y + (normal.y * length));
        }

        public readonly Direction GetRotatedRadians(float radians)
        {
            if (isZero)
            {
                return default;
            }

            return FromAngleRadians(_radians + radians);
        }

        public readonly Direction GetRotatedDegrees(float degrees)
        {
            if (isZero)
            {
                return default;
            }

            return FromAngleDegrees(_degrees + degrees);
        }

        public readonly Direction GetInverse()
        {
            if (isZero)
            {
                return default;
            }

            if (_radians < MathF.PI)
            {
                return new Direction(-_normal, _radians + MathF.PI, _degrees + 180f);
            }
            else
            {
                return new Direction(-_normal, _radians - MathF.PI, _degrees - 180f);
            }
        }

        /// <remarks>
        /// Goes <b>counter-clockwise</b>.<br/>
        /// For clockwise, use <see cref="GetPerpendicularInverse"/>.
        /// </remarks>
        public readonly Direction GetPerpendicular()
        {
            if (isZero)
            {
                return default;
            }

            const float RADIAN_SHIFT = MathF.PI * 1.5f;
            const float RADIAN_SHIFT_NEGATIVE = MathF.PI * 0.5f;
            const float DEGREES_SHIFT = 270f;
            const float DEGREES_SHIFT_NEGATIVE = 90f;

            if (radians < RADIAN_SHIFT)
            {
                return new Direction(new Point(-_normal.y, _normal.x), _radians - RADIAN_SHIFT, _degrees - DEGREES_SHIFT);
            }
            else
            {
                return new Direction(new Point(-_normal.y, _normal.x), radians + RADIAN_SHIFT_NEGATIVE, degrees + DEGREES_SHIFT_NEGATIVE);
            }
        }

        /// <remarks>
        /// Goes <b>clockwise</b>.<br/>
        /// For counter-clockwise, use <see cref="GetPerpendicular"/>
        /// </remarks>
        public readonly Direction GetPerpendicularInverse()
        {
            if (isZero)
            {
                return default;
            }

            const float RADIAN_SHIFT = MathF.PI * 0.5f;
            const float RADIAN_SHIFT_NEGATIVE = MathF.PI * 1.5f;
            const float DEGREES_SHIFT = 90f;
            const float DEGREES_SHIFT_NEGATIVE = 270f;

            if (radians >= RADIAN_SHIFT)
            {
                return new Direction(new Point(_normal.y, -_normal.x), _radians - RADIAN_SHIFT, _degrees - DEGREES_SHIFT);
            }
            else
            {
                return new Direction(new Point(_normal.y, -_normal.x), radians + RADIAN_SHIFT_NEGATIVE, degrees + DEGREES_SHIFT_NEGATIVE);
            }
        }
    
        public readonly float GetDifferenceRadians(Direction target)
        {
            var rawDifference = target.radians - radians;

            if (rawDifference >= 0f)
            {
                if (rawDifference > MathF.PI)
                {
                    return -(MathF.PI - (rawDifference - MathF.PI));
                }
                else
                {
                    return rawDifference;
                }
            }
            else
            {
                if (rawDifference <= (-MathF.PI))
                {
                    return MathF.PI + (rawDifference + MathF.PI);
                }
                else
                {
                    return rawDifference;
                }
            }
        }

        public readonly float GetDifferenceDegrees(Direction target)
        {
            var rawDifference = target.degrees - degrees;

            if (rawDifference >= 0f)
            {
                if (rawDifference > 180f)
                {
                    return -(180f - (rawDifference - 180f));
                }
                else
                {
                    return rawDifference;
                }
            }
            else
            {
                if (rawDifference <= (-180f))
                {
                    return 180f + (rawDifference + 180f);
                }
                else
                {
                    return rawDifference;
                }
            }
        }

        public readonly override bool Equals(object obj)
        {
            if (obj is Direction objDirection)
            {
                return this == objDirection;
            }

            return false;
        }

        public readonly override int GetHashCode()
        {
            return _radians.GetHashCode() ^ isZero.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Point NormalFromRadians(float radians)
        {
            return new Point(MathF.Cos(radians), MathF.Sin(radians));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CalculateAnglesFromNormal(in Point normal, out float radians, out float degrees)
        {
            radians = MathF.Atan2(normal.y, normal.x);
            degrees = AngleUtility.RadiansToDegrees(radians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CalculateDegreesAndNormalFromRadians(in float radians, out float degrees, out Point normal)
        {
            degrees = AngleUtility.RadiansToDegrees(radians);
            normal = NormalFromRadians(radians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CalculateRadiansAndNormalFromDegrees(in float degrees, out float radians, out Point normal)
        {
            radians = AngleUtility.DegreesToRadians(degrees);
            normal = NormalFromRadians(radians);
        }
    }
}