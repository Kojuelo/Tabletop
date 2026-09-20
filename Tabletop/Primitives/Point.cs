using System;
using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public struct Point
    {
        public float x;

        public float y;


        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point a, Point b)
        {
            return (MathF.Abs(a.x - b.x) < Geometry.EPSILON) && (MathF.Abs(a.y - b.y) < Geometry.EPSILON);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point a, Point b)
        {
            return (MathF.Abs(a.x - b.x) >= Geometry.EPSILON) || (MathF.Abs(a.y - b.y) >= Geometry.EPSILON);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator +(in Point a, in Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator -(in Point a, in Point b)
        {
            return new Point(a.x - b.x, a.y - b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator *(in Point p, float s)
        {
            return new Point(p.x * s, p.y * s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator *(float s, in Point p)
        {
            return new Point(p.x * s, p.y * s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator -(in Point p)
        {
            return new Point(-p.x, -p.y);
        }


        public bool isZero => (x == 0f) && (y == 0f);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(float xTranslation, float yTranslation)
        {
            x += xTranslation;
            y += yTranslation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Point translation)
        {
            Translate(translation.x, translation.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Direction direction, float length)
        {
            Translate(direction.GetTranslation(length));
        }

        public void RotateRadians(float rotationRadians)
        {
            if (rotationRadians == 0f)
            {
                return;
            }

            float sin = MathF.Sin(rotationRadians);
            float cos = MathF.Cos(rotationRadians);

            var xTemp = x;
            var yTemp = y;

            x = (cos * xTemp) - (sin * yTemp);
            y = (sin * xTemp) + (cos * yTemp);
        }

        public void RotateRadians(in Point pivot, float rotationRadians)
        {
            var temp = this - pivot;
            temp.RotateRadians(rotationRadians);

            x = pivot.x + temp.x;
            y = pivot.y + temp.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegrees(float rotationDegrees)
        {
            RotateRadians(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegrees(in Point pivot, float rotationDegrees)
        {
            RotateRadians(pivot, AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetTranslated(float xTranslation, float yTranslation)
        {
            return new Point(x + xTranslation, y + yTranslation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetTranslated(in Point translation)
        {
            return GetTranslated(translation.x, translation.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetTranslated(in Direction direction, float length)
        {
            return GetTranslated(direction.GetTranslation(length));
        }

        public readonly Point GetRotatedRadians(float rotationRadians)
        {
            if (rotationRadians == 0f)
            {
                return this;
            }

            float sin = MathF.Sin(rotationRadians);
            float cos = MathF.Cos(rotationRadians);

            return new Point(
                (cos * x) - (sin * y),
                (sin * x) + (cos * y)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetRotatedRadians(in Point pivot, float rotationRadians)
        {
            return pivot + (this - pivot).GetRotatedRadians(rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetRotatedDegrees(float rotationDegrees)
        {
            return GetRotatedRadians(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetRotatedDegrees(in Point pivot, float rotationDegrees)
        {
            return GetRotatedRadians(pivot, AngleUtility.DegreesToRadians(rotationDegrees));
        }

        public readonly override string ToString()
        {
            return $"{{{x}, {y}}}";
        }

        public readonly override bool Equals(object obj)
        {
            if (obj is Point objPoint)
            {
                return this == objPoint;
            }

            return false;
        }

        public readonly override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }
    }
}