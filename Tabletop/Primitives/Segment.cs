using System;
using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public struct Segment
    {
        public Point a;

        public Point b;


        public Segment(float ax, float ay, float bx, float by)
        {
            a = new Point(ax, ay);
            b = new Point(bx, by);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Segment(in Point a, in Point b)
            : this(a.x, a.y, b.x, b.y)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Segment(in Point source, in Direction direction, float length)
            : this(source, source.GetTranslated(direction, length))
        {
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Segment a, in Segment b)
        {
            return (a.a == b.a) && (a.b == b.b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Segment a, in Segment b)
        {
            return (a.a != b.a) || (a.b != b.b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Segment operator +(in Segment a, in Point b)
        {
            return a.GetTranslated(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Segment operator -(in Segment a, in Point b)
        {
            return a.GetTranslated(-b);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(float xTranslation, float yTranslation)
        {
            a.Translate(xTranslation, yTranslation);
            b.Translate(xTranslation, yTranslation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Point translation)
        {
            a.Translate(translation);
            b.Translate(translation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Direction direction, float length)
        {
            Translate(direction.GetTranslation(length));
        }

        public void RotateRadians(in Point pivot, float rotationRadians)
        {
            if (rotationRadians == 0f)
            {
                return;
            }

            float sin = MathF.Sin(rotationRadians);
            float cos = MathF.Cos(rotationRadians);

            var aTemp = a - pivot;
            var bTemp = b - pivot;

            a.x = pivot.x + ((cos * aTemp.x) - (sin * aTemp.y));
            a.y = pivot.y + ((sin * aTemp.x) + (cos * aTemp.y));

            b.x = pivot.x + ((cos * bTemp.x) - (sin * bTemp.y));
            b.y = pivot.y + ((sin * bTemp.x) + (cos * bTemp.y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegrees(in Point pivot, float rotationDegrees)
        {
            RotateRadians(pivot, AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateRadiansFromA(float rotationRadians)
        {
            RotateRadians(a, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegreesFromA(float rotationDegrees)
        {
            RotateRadiansFromA(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateRadiansFromB(float rotationRadians)
        {
            RotateRadians(b, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegreesFromB(float rotationDegrees)
        {
            RotateRadiansFromB(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        public void Scale(Point pivot, float xFactor, float yFactor)
        {
            a.x = pivot.x + ((a.x - pivot.x) * xFactor);
            a.y = pivot.y + ((a.y - pivot.y) * yFactor);

            b.x = pivot.x + ((b.x - pivot.x) * xFactor);
            b.y = pivot.y + ((b.y - pivot.y) * yFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScaleFromA(float xFactor, float yFactor)
        {
            Scale(a, xFactor, yFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScaleFromB(float xFactor, float yFactor)
        {
            Scale(b, xFactor, yFactor);
        }

        public readonly void GetBounds(out float xMin, out float yMin, out float xMax, out float yMax)
        {
            // X and width.
            if (a.x < b.x)
            {
                xMin = a.x;
                xMax = b.x;
            }
            else
            {
                xMin = b.x;
                xMax = a.x;
            }

            // Y and height.
            if (a.y < b.y)
            {
                yMin = a.y;
                yMax = b.y;
            }
            else
            {
                yMin = b.y;
                yMax = a.y;
            }
        }

        public readonly Bounds GetBounds()
        {
            GetBounds(out float xMin, out float yMin, out float xMax, out float yMax);

            return new Bounds(xMin, yMin, xMax, yMax);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Segment GetTranslated(float xTranslation, float yTranslation)
        {
            return new Segment(
                a.x + xTranslation,
                a.y + yTranslation,
                b.x + xTranslation,
                b.y + yTranslation
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Segment GetTranslated(in Point translation)
        {
            return GetTranslated(translation.x, translation.y);
        }

        public readonly Segment GetTranslated(in Direction direction, float length)
        {
            var translation = direction.GetTranslation(length);

            return GetTranslated(translation);
        }

        public readonly Segment GetRotatedRadians(in Point pivot, float rotationRadians)
        {
            if (rotationRadians == 0f)
            {
                return this;
            }

            float sin = MathF.Sin(rotationRadians);
            float cos = MathF.Cos(rotationRadians);

            var aPivot = a - pivot;
            var bPivot = b - pivot;

            return new Segment(
                pivot.x + ((cos * aPivot.x) - (sin * aPivot.y)),
                pivot.y + ((sin * aPivot.x) + (cos * aPivot.y)),
                pivot.x + ((cos * bPivot.x) - (sin * bPivot.y)),
                pivot.y + ((sin * bPivot.x) + (cos * bPivot.y))
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Segment GetRotatedDegrees(in Point pivot, float rotationDegrees)
        {
            return GetRotatedRadians(pivot, AngleUtility.DegreesToRadians(rotationDegrees));
        }

        public readonly Segment GetRotatedRadiansFromA(float rotationRadians)
        {
            return GetRotatedRadians(a, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Segment GetRotatedDegreesFromA(float rotationDegrees)
        {
            return GetRotatedRadiansFromA(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Segment GetRotatedRadiansFromB(float rotationRadians)
        {
            return GetRotatedRadians(b, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Segment GetRotatedDegreesFromB(float rotationDegrees)
        {
            return GetRotatedRadiansFromB(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        public readonly Segment GetScaled(in Point pivot, float xFactor, float yFactor)
        {
            return new Segment(
                pivot.x + ((a.x - pivot.x) * xFactor),
                pivot.y + ((a.y - pivot.y) * yFactor),
                pivot.x + ((b.x - pivot.x) * xFactor),
                pivot.y + ((b.y - pivot.y) * yFactor)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Segment GetScaledFromA(float xFactor, float yFactor)
        {
            return GetScaled(a, xFactor, yFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Segment GetScaledFromB(float xFactor, float yFactor)
        {
            return GetScaled(b, xFactor, yFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetPointInSegment(float abInterpolant)
        {
            return a + ((b - a) * abInterpolant);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Direction GetDirectionFromA()
        {
            return new Direction(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Direction GetDirectionFromB()
        {
            return new Direction(b, a);
        }
    
        public readonly override bool Equals(object obj)
        {
            if (obj is Segment objSegment)
            {
                return this == objSegment;
            }

            return false;
        }

        public readonly override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode();
        }
    }

}