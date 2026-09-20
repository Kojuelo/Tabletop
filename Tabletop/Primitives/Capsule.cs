using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public struct Capsule
    {
        public Segment segment;

        public float radius;


        public Capsule(float ax, float ay, float bx, float by, float radius)
        {
            segment = new Segment(ax, ay, bx, by);
            this.radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule(in Segment segment, float radius)
            : this(segment.a.x, segment.a.y, segment.b.x, segment.b.y, radius)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule(in Point a, in Point b, float radius)
            : this(a.x, a.y, b.x, b.y, radius)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule(in Point source, in Direction direction, float length, float radius)
            : this(source, source.GetTranslated(direction, length), radius)
        {
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Capsule a, in Capsule b)
        {
            return (a.segment == b.segment) && (a.radius == b.radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Capsule a, in Capsule b)
        {
            return (a.segment != b.segment) || (a.radius != b.radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Capsule operator +(in Capsule a, in Point b)
        {
            return a.GetTranslated(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Capsule operator -(in Capsule a, in Point b)
        {
            return a.GetTranslated(-b);
        }


        public Point a
        {
            readonly get => segment.a;
            set
            {
                segment.a = value;
            }
        }

        public Point b
        {
            readonly get => segment.b;
            set
            {
                segment.b = value;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(float xTranslation, float yTranslation)
        {
            segment.Translate(xTranslation, yTranslation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Point translation)
        {
            segment.Translate(translation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Direction direction, float length)
        {
            segment.Translate(direction, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateRadians(in Point pivot, float rotationRadians)
        {
            segment.RotateRadians(pivot, rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegrees(in Point pivot, float rotationDegrees)
        {
            segment.RotateDegrees(pivot, rotationDegrees);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateRadiansFromA(float rotationRadians)
        {
            segment.RotateRadiansFromA(rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegreesFromA(float rotationDegrees)
        {
            segment.RotateDegreesFromA(rotationDegrees);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateRadiansFromB(float rotationRadians)
        {
            segment.RotateRadiansFromB(rotationRadians);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateDegreesFromB(float rotationDegrees)
        {
            segment.RotateDegreesFromB(rotationDegrees);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(Point pivot, float xFactor, float yFactor, float radiusFactor)
        {
            segment.Scale(pivot, xFactor, yFactor);

            radius *= radiusFactor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScaleFromA(float xFactor, float yFactor, float radiusFactor)
        {
            segment.ScaleFromA(xFactor, yFactor);

            radius *= radiusFactor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScaleFromB(float xFactor, float yFactor, float radiusFactor)
        {
            segment.ScaleFromB(xFactor, yFactor);

            radius *= radiusFactor;
        }

        public readonly void GetBounds(out float xMin, out float yMin, out float xMax, out float yMax)
        {
            // X and width.
            if (a.x < b.x)
            {
                xMin = a.x - radius;
                xMax = b.x + radius;
            }
            else
            {
                xMin = b.x - radius;
                xMax = a.x + radius;
            }

            // Y and height.
            if (a.y < b.y)
            {
                yMin = a.y - radius;
                yMax = b.y + radius;
            }
            else
            {
                yMin = b.y - radius;
                yMax = a.y + radius;
            }
        }

        public readonly Bounds GetBounds()
        {
            GetBounds(out float xMin, out float yMin, out float xMax, out float yMax);

            return new Bounds(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetTranslated(float xTranslation, float yTranslation)
        {
            return new Capsule(segment.GetTranslated(xTranslation, yTranslation), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetTranslated(in Point translation)
        {
            return new Capsule(segment.GetTranslated(translation), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetTranslated(in Direction direction, float length)
        {
            return new Capsule(segment.GetTranslated(direction, length), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetRotatedRadians(in Point pivot, float rotationRadians)
        {
            return new Capsule(segment.GetRotatedRadians(pivot, rotationRadians), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetRotatedDegrees(in Point pivot, float rotationDegrees)
        {
            return new Capsule(segment.GetRotatedDegrees(pivot, rotationDegrees), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetRotatedRadiansFromA(float rotationRadians)
        {
            return new Capsule(segment.GetRotatedRadiansFromA(rotationRadians), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetRotatedDegreesFromA(float rotationDegrees)
        {
            return new Capsule(segment.GetRotatedDegreesFromA(rotationDegrees), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetRotatedRadiansFromB(float rotationRadians)
        {
            return new Capsule(segment.GetRotatedDegreesFromB(rotationRadians), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetRotatedDegreesFromB(float rotationDegrees)
        {
            return new Capsule(segment.GetRotatedDegreesFromB(rotationDegrees), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetScaled(in Point pivot, float xFactor, float yFactor, float radiusFactor)
        {
            return new Capsule(segment.GetScaled(pivot, xFactor, yFactor), radiusFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetScaledFromA(float xFactor, float yFactor, float radiusFactor)
        {
            return new Capsule(segment.GetScaledFromA(xFactor, yFactor), radiusFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Capsule GetScaledFromB(float xFactor, float yFactor, float radiusFactor)
        {
            return new Capsule(segment.GetScaledFromB(xFactor, yFactor), radiusFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Point GetPointInSegment(float abInterpolant)
        {
            return segment.GetPointInSegment(abInterpolant);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Direction GetDirectionFromA()
        {
            return segment.GetDirectionFromA();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Direction GetDirectionFromB()
        {
            return segment.GetDirectionFromB();
        }
    
        public readonly override bool Equals(object obj)
        {
            if (obj is Capsule objCapsule)
            {
                return this == objCapsule;
            }

            return false;
        }

        public readonly override int GetHashCode()
        {
            return segment.GetHashCode() ^ radius.GetHashCode();
        }
    }

}