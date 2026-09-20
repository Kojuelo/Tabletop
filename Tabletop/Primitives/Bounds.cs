using System;
using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public struct Bounds
    {
        public float xMin;

        public float yMin;

        public float xMax;

        public float yMax;


        public Bounds(float xMin, float yMin, float xMax, float yMax)
        {
            this.xMin = xMin;
            this.yMin = yMin;

            this.xMax = xMax;
            this.yMax = yMax;
        }

        public Bounds(in Point min, in Point max)
            : this(min.x, min.y, max.x, max.y)
        {
        }


        public Point min
        {
            readonly get => new Point(xMin, yMin);
            set
            {
                xMin = value.x;
                yMin = value.y;
            }
        }

        public Point max
        {
            readonly get => new Point(xMax, yMax);
            set
            {
                xMax = value.x;
                yMax = value.y;
            }
        }

        public Point bottomLeft
        {
            readonly get => new Point(xMin, yMin);
            set
            {
                xMin = value.x;
                yMin = value.y;
            }
        }

        public Point topLeft
        {
            readonly get => new Point(xMin, yMax);
            set
            {
                xMin = value.x;
                yMax = value.y;
            }
        }

        public Point topRight
        {
            readonly get => new Point(xMax, yMax);
            set
            {
                xMax = value.x;
                yMax = value.y;
            }
        }

        public Point bottomRight
        {
            readonly get => new Point(xMax, yMin);
            set
            {
                xMax = value.x;
                yMin = value.y;
            }
        }

        public Point center
        {
            get => new Point((xMin + xMax) * 0.5f, (yMin + yMax) * 0.5f);
            set
            {
                var offset = value - center;

                xMin += offset.x;
                xMax += offset.x;

                yMin += offset.y;
                yMax += offset.y;
            }
        }

        public readonly float width => xMax - xMin;

        public readonly float height => yMax - yMin;

        public readonly bool isValid => xIsValid && yIsValid;

        public readonly bool xIsValid => xMin <= xMax;

        public readonly bool yIsValid => yMin <= yMax;


        public static Bounds CalculateBoundsFromCorner(in Point corner, float xSize, float ySize)
        {
            Bounds bounds = default;

            if (xSize >= 0f)
            {
                bounds.xMin = corner.x;
                bounds.xMax = corner.x + xSize;
            }
            else
            {
                bounds.xMin = corner.x - xSize;
                bounds.xMax = corner.x;
            }

            if (ySize >= 0f)
            {
                bounds.yMin = corner.y;
                bounds.yMax = corner.y + ySize;
            }
            else
            {
                bounds.yMin = corner.y - ySize;
                bounds.yMax = corner.y;
            }

            return bounds;
        }

        public static Bounds CalculateBoundsFromCenter(in Point center, float xSize, float ySize)
        {
            var halfSizeX = xSize * .5f;
            var halfSizeY = ySize * .5f;

            Bounds bounds = default;

            bounds.xMin = center.x - halfSizeX;
            bounds.xMax = center.x + halfSizeX;

            bounds.yMin = center.y - halfSizeY;
            bounds.yMax = center.y + halfSizeY;

            return bounds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds Combine(in Bounds a, in Bounds b)
        {
            return new Bounds(MathF.Min(a.xMin, b.xMin), MathF.Min(a.yMin, b.yMin), MathF.Max(a.xMax, b.xMax), MathF.Max(a.yMax, b.yMax));
        }

        public void Validate()
        {
            if (xMin > xMax)
            {
                var xMinTemp = xMin;

                xMin = xMax;
                xMax = xMinTemp;
            }

            if (yMin > yMax)
            {
                var yMinTemp = yMin;

                yMin = yMax;
                yMax = yMinTemp;
            }
        }

        public void Translate(float xTranslation, float yTranslation)
        {
            xMin += xTranslation;
            xMax += xTranslation;

            yMin += yTranslation;
            yMax += yTranslation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(in Point translation)
        {
            Translate(translation.x, translation.y);
        }

        public void ScaleFromCenter(float xFactor, float yFactor)
        {
            GetGrowthHalf(xFactor, yFactor, out var widthGrowthHalf, out var heightGrowthHalf);

            xMin -= widthGrowthHalf;
            xMax += widthGrowthHalf;

            yMin -= heightGrowthHalf;
            yMax += heightGrowthHalf;
        }

        public void ScaleFromMin(float xFactor, float yFactor)
        {
            GetGrowth(xFactor, yFactor, out var widthGrowth, out var heightGrowth);

            xMax += widthGrowth;
            yMax += heightGrowth;
        }

        public void ScaleFromMax(float xFactor, float yFactor)
        {
            GetGrowth(xFactor, yFactor, out var widthGrowth, out var heightGrowth);

            xMin -= widthGrowth;
            yMin -= heightGrowth;
        }

        public readonly bool Contains(in Point point)
        {
            if (point.x < xMin)
            {
                return false;
            }

            if (point.x > xMax)
            {
                return false;
            }

            if (point.y < yMin)
            {
                return false;
            }

            if (point.y > yMax)
            {
                return false;
            }

            return true;
        }

        public readonly bool Overlaps(in Bounds bounds)
        {
            if (xMin > bounds.xMax)
            {
                return false;
            }

            if (xMax < bounds.xMin)
            {
                return false;
            }

            if (yMin > bounds.yMax)
            {
                return false;
            }

            if (yMax < bounds.yMin)
            {
                return false;
            }

            return true;
        }

        public readonly Bounds GetValidated()
        {
            Point min = this.min;
            Point max = this.max;

            if (xMin > xMax)
            {
                min.x = xMax;
                max.x = xMin;
            }

            if (yMin > yMax)
            {
                min.y = yMax;
                max.y = yMin;
            }

            return new Bounds(min, max);
        }

        public readonly Bounds GetTranslated(float xTranslation, float yTranslation)
        {
            return new Bounds(xMin + xTranslation, yMin + yTranslation, xMax + xTranslation, yMax + yTranslation);
        }

        public readonly Bounds GetTranslated(in Point translation)
        {
            return GetTranslated(translation.x, translation.y);
        }

        public readonly Bounds GetScaledFromCenter(float xFactor, float yFactor)
        {
            GetGrowthHalf(xFactor, yFactor, out var widthGrowthHalf, out var heightGrowthHalf);

            return new Bounds(xMin - widthGrowthHalf, yMin - heightGrowthHalf, xMax + widthGrowthHalf, yMax + heightGrowthHalf);
        }

        public readonly Bounds GetScaledFromMin(float xFactor, float yFactor)
        {
            GetGrowth(xFactor, yFactor, out var widthGrowth, out var heightGrowth);

            return new Bounds(xMin, yMin, xMax + widthGrowth, yMax + heightGrowth);
        }

        public readonly Bounds GetScaledFromMax(float xFactor, float yFactor)
        {
            GetGrowth(xFactor, yFactor, out var widthGrowth, out var heightGrowth);

            return new Bounds(xMin - widthGrowth, yMin - heightGrowth, xMax, yMax);
        }

        private readonly void GetGrowth(float xFactor, float yFactor, out float widthGrowth, out float heightGrowth)
        {
            var thisWidth = width;
            var thisHeight = height;

            widthGrowth = (thisWidth * xFactor) - thisWidth;
            heightGrowth = (thisHeight * yFactor) - thisHeight;
        }

        private readonly void GetGrowthHalf(float xFactor, float yFactor, out float widthGrowthHalf, out float heightGrowthHalf)
        {
            GetGrowth(xFactor, yFactor, out widthGrowthHalf, out heightGrowthHalf);

            widthGrowthHalf *= .5f;
            heightGrowthHalf *= .5f;
        }
    }

}