using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public struct Line
    {
        public float yMultiplier;

        public float xMultiplier;

        public float constant;


        public Line(float yMultiplier, float xMultiplier, float constant)
        {
            this.yMultiplier = yMultiplier;
            this.xMultiplier = xMultiplier;
            this.constant = constant;
        }

        public Line(float ax, float ay, float bx, float by)
        {
            yMultiplier = ay - by;
            xMultiplier = ax - bx;
            constant = (ax * by) - (bx * ay);
        }

        public Line(in Point a, in Point b)
            : this(a.x, a.y, b.x, b.y)
        {
        }

        public Line(in Segment segment)
            : this(segment.a, segment.b)
        {
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Line a, in Line b)
        {
            return (a.yMultiplier == b.yMultiplier) && (a.xMultiplier == b.xMultiplier) && (a.constant == b.constant);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Line a, in Line b)
        {
            return (a.yMultiplier != b.yMultiplier) || (a.xMultiplier != b.xMultiplier) || (a.constant != b.constant);
        }


        public readonly float GetYAtX(float x)
        {
            if (xMultiplier == 0f)
            {
                return 0f;
            }

            // Formula: y = (-ax - c) / b;
            return ((-(yMultiplier * x)) - constant) / xMultiplier;
        }

        public readonly float GetXAtY(float y)
        {
            if (yMultiplier == 0.0f)
            {
                return 0f;
            }

            // Formula: x = (-by - c) / a;
            return ((-(xMultiplier * y)) - constant) / yMultiplier;
        }

        public readonly Point GetPointAtX(float x)
        {
            if (xMultiplier == 0f)
            {
                return default;
            }

            return new Point(x, GetYAtX(x));
        }

        public readonly Point GetPointAtY(float y)
        {
            if (yMultiplier == 0f)
            {
                return default;
            }

            return new Point(GetXAtY(y), y);
        }

        public readonly override string ToString()
        {
            return $"{{{yMultiplier}, {xMultiplier}, {constant}}}";
        }
    
        public readonly override bool Equals(object obj)
        {
            if (obj is Line objLine)
            {
                return this == objLine;
            }

            return false;
        }

        public readonly override int GetHashCode()
        {
            return yMultiplier.GetHashCode() ^ xMultiplier.GetHashCode() ^ constant.GetHashCode();
        }
    }
}