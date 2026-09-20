using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public struct Circle
    {
        public Point center;

        public float radius;


        public Circle(float x, float y, float radius)
        {
            center = new Point(x, y);
            this.radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle(in Point center, float radius)
            : this(center.x, center.y, radius)
        {
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Circle a, in Circle b)
        {
            return (a.center == b.center) && (a.radius == b.radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Circle a, in Circle b)
        {
            return (a.center != b.center) || (a.radius != b.radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle operator +(in Circle a, in Point b)
        {
            return a.GetTranslated(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle operator -(in Circle a, in Point b)
        {
            return a.GetTranslated(-b);
        }


        public float x
        {
            readonly get => center.x;
            set => center.x = value;
        }

        public float y
        {
            readonly get => center.y;
            set => center.y = value;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(float xTranslation, float yTranslation)
        {
            center.Translate(xTranslation, yTranslation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Point translation)
        {
            center.Translate(translation.x, translation.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Direction direction, float length)
        {
            Translate(direction.GetTranslation(length));
        }

        public readonly void GetBounds(out float xMin, out float yMin, out float xMax, out float yMax)
        {
            // X and width.
            xMin = center.x - radius;
            xMax = center.x + radius;

            // Y and height.
            yMin = center.y - radius;
            yMax = center.y + radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds GetBounds()
        {
            GetBounds(out float xMin, out float yMin, out float xMax, out float yMax);

            return new Bounds(xMin, yMin, xMax, yMax);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Circle GetTranslated(float xTranslation, float yTranslation)
        {
            return new Circle(center.GetTranslated(xTranslation, yTranslation), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Circle GetTranslated(in Point translation)
        {
            return new Circle(center.GetTranslated(translation), radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Circle GetTranslated(in Direction direction, float length)
        {
            return GetTranslated(direction.GetTranslation(length));
        }
    
        public readonly override bool Equals(object obj)
        {
            if (obj is Circle objCircle)
            {
                return this == objCircle;
            }

            return false;
        }

        public readonly override int GetHashCode()
        {
            return center.GetHashCode() ^ radius.GetHashCode();
        }
    }
}