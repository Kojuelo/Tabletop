using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public struct Triangle
    {
        public Point a;

        public Point b;

        public Point c;


        public Triangle(in Point a, in Point b, in Point c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Triangle(float ax, float ay, float bx, float by, float cx, float cy)
        {
            a = new Point(ax, ay);
            b = new Point(bx, by);
            c = new Point(cx, cy);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Triangle a, in Triangle b)
        {
            return (a.a == b.a) && (a.b == b.b) && (a.c == b.c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Triangle a, in Triangle b)
        {
            return (a.a != b.a) || (a.b != b.b) || (a.c != b.c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle operator +(in Triangle a, in Point b)
        {
            return a.GetTranslated(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle operator -(in Triangle a, in Point b)
        {
            return a.GetTranslated(-b);
        }


        public Segment ab
        { 
            readonly get => new Segment(a, b);
            set
            {
                a = value.a;
                b = value.b;
            }
        }

        public Segment ac
        { 
            readonly get => new Segment(a, c);
            set
            {
                a = value.a;
                c = value.b;
            }
        }

        public Segment ba
        { 
            readonly get => new Segment(b, a);
            set
            {
                b = value.a;
                a = value.b;
            }
        }

        public Segment bc
        { 
            readonly get => new Segment(b, c);
            set
            {
                b = value.a;
                c = value.b;
            }
        }

        public Segment ca
        { 
            readonly get => new Segment(c, a);
            set
            {
                c = value.a;
                a = value.b;
            }
        }

        public Segment cb
        { 
            readonly get => new Segment(c, b);
            set
            {
                c = value.a;
                b = value.b;
            }
        }

        public Point center
        {
            readonly get => new Point((a.x + b.x + c.x) * (1f/3f), (a.y + b.y + c.y) * (1f/3f));
            set
            {
                var offset = value - center;

                a.Translate(offset);
                b.Translate(offset);
                c.Translate(offset);
            }
        }


        public void Translate(float xTranslation, float yTranslation)
        {
            a.Translate(xTranslation, yTranslation);
            b.Translate(xTranslation, yTranslation);
            c.Translate(xTranslation, yTranslation);
        }

        public void Translate(in Point translation)
        {
            a.Translate(translation);
            b.Translate(translation);
            c.Translate(translation);
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
            var cTemp = c - pivot;
            
            a.x = pivot.x + ((cos * aTemp.x) - (sin * aTemp.y));
            a.y = pivot.y + ((sin * aTemp.x) + (cos * aTemp.y));

            b.x = pivot.x + ((cos * bTemp.x) - (sin * bTemp.y));
            b.y = pivot.y + ((sin * bTemp.x) + (cos * bTemp.y));

            c.x = pivot.x + ((cos * cTemp.x) - (sin * cTemp.y));
            c.y = pivot.y + ((sin * cTemp.x) + (cos * cTemp.y));
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateRadiansFromC(float rotationRadians)
        {
            RotateRadians(c, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegreesFromC(float rotationDegrees)
        {
            RotateRadiansFromC(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        public void Scale(in Point pivot, float xFactor, float yFactor)
        {
            a.x = pivot.x + ((a.x - pivot.x) * xFactor);
            a.y = pivot.y + ((a.y - pivot.y) * yFactor);

            b.x = pivot.x + ((b.x - pivot.x) * xFactor);
            b.y = pivot.y + ((b.y - pivot.y) * yFactor);

            c.x = pivot.x + ((c.x - pivot.x) * xFactor);
            c.y = pivot.y + ((c.y - pivot.y) * yFactor);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScaleFromC(float xFactor, float yFactor)
        {
            Scale(c, xFactor, yFactor);
        }

        public readonly void GetBounds(out float xMin, out float yMin, out float xMax, out float yMax)
        {
            xMin = a.x;
            yMin = a.y;

            xMax = xMin;
            yMax = yMin;

            if (b.x < xMin)
            {
                xMin = b.x;
            }
            else if (b.x > xMax)
            {
                xMax = b.x;
            }

            if (b.y < yMin)
            {
                yMin = b.y;
            }
            else if (b.y > yMax)
            {
                yMax = b.y;
            }

            if (c.x < xMin)
            {
                xMin = c.x;
            }
            else if (c.x > xMax)
            {
                xMax = c.x;
            }

            if (c.y < yMin)
            {
                yMin = c.y;
            }
            else if (c.y > yMax)
            {
                yMax = c.y;
            }
        }

        public readonly Bounds GetBounds()
        {
            GetBounds(out float xMin, out float yMin, out float xMax, out float yMax);

            return new Bounds(xMin, yMin, xMax, yMax);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetTranslated(in Point translation)
        {
            return new Triangle(a + translation, b + translation, c + translation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetTranslated(float xTranslation, float yTranslation)
        {
            return new Triangle(
                new Point(a.x + xTranslation, a.y + yTranslation),
                new Point(b.x + xTranslation, b.y + yTranslation),
                new Point(c.x + xTranslation, c.y + yTranslation)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetTranslated(in Direction direction, float length)
        {
            return GetTranslated(direction.GetTranslation(length));
        }

        public readonly Triangle GetRotatedRadians(in Point pivot, float rotationRadians)
        {
            if (rotationRadians == 0f)
            {
                return this;
            }

            float sin = MathF.Sin(rotationRadians);
            float cos = MathF.Cos(rotationRadians);

            var aTemp = a - pivot;
            var bTemp = b - pivot;
            var cTemp = c - pivot;

            return new Triangle(
                pivot.x + ((cos * aTemp.x) - (sin * aTemp.y)),
                pivot.y + ((sin * aTemp.x) + (cos * aTemp.y)),
                pivot.x + ((cos * bTemp.x) - (sin * bTemp.y)),
                pivot.y + ((sin * bTemp.x) + (cos * bTemp.y)),
                pivot.x + ((cos * cTemp.x) - (sin * cTemp.y)),
                pivot.y + ((sin * cTemp.x) + (cos * cTemp.y))
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetRotatedDegrees(in Point pivot, float rotationDegrees)
        {
            return GetRotatedRadians(pivot, AngleUtility.DegreesToRadians(rotationDegrees));
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetRotatedRadiansFromA(float rotationRadians)
        {
            return GetRotatedRadians(a, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetRotatedDegreesFromA(float rotationDegrees)
        {
            return GetRotatedRadiansFromA(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetRotatedRadiansFromB(float rotationRadians)
        {
            return GetRotatedRadians(b, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetRotatedDegreesFromB(float rotationDegrees)
        {
            return GetRotatedRadiansFromB(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetRotatedRadiansFromC(float rotationRadians)
        {
            return GetRotatedRadians(c, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetRotatedDegreesFromC(float rotationDegrees)
        {
            return GetRotatedRadiansFromC(AngleUtility.DegreesToRadians(rotationDegrees));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Triangle GetScaled(in Point pivot, float xFactor, float yFactor)
        {
            return new Triangle(
                pivot.x + ((a.x - pivot.x) * xFactor),
                pivot.y + ((a.y - pivot.y) * yFactor),
                pivot.x + ((b.x - pivot.x) * xFactor),
                pivot.y + ((b.y - pivot.y) * yFactor),
                pivot.x + ((c.x - pivot.x) * xFactor),
                pivot.y + ((c.y - pivot.y) * yFactor)
            );
        }
    
        public readonly IEnumerable<Segment> IterateSides()
        {
            yield return ab;
            yield return bc;
            yield return ca;
        }

        public readonly override bool Equals(object obj)
        {
            if (obj is Triangle objTriangle)
            {
                return this == objTriangle;
            }

            return false;
        }

        public readonly override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode() ^ c.GetHashCode();
        }
    }
}