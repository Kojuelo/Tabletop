
using System;
using System.Runtime.CompilerServices;


namespace Kojuelo.Tabletop
{
    public static class Geometry
    {
        private const float EPSILON = 0.00001f;

        private const float INTERSECTION_EPSILON = 0.00001f;


        #region POINT DISTANCE
        public static float PointDistanceSquared(in Point p1, in Point p2)
        {
            var xDiff = p2.x - p1.x;
            var yDiff = p2.y - p1.y;

            return (xDiff * xDiff) + (yDiff * yDiff);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PointDistance(in Point p1, in Point p2)
        {
            return MathF.Sqrt(PointDistanceSquared(p1, p2));
        }
        #endregion

        #region POINT INTERPOLATE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point PointInterpolate(in Point p1, in Point p2, float interpolant)
        {
            return p1 + ((p2 - p1) * interpolant);
        }
        #endregion
    
        #region CIRCLE POINT CLOSEST
        public static Point CirclePointClosest(in Circle c, in Point p, out float centerDistance)
        {
            var xDiff = p.x - c.x;
            var yDiff = p.y - c.y;

            centerDistance = MathF.Sqrt((xDiff * xDiff) + (yDiff * yDiff));

            if (centerDistance < c.radius)
            {
                return p;
            }

            return new Point
            (
                c.x + ((xDiff / centerDistance) * c.radius),
                c.y + ((yDiff / centerDistance) * c.radius)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point CirclePointClosest(in Circle c, in Point p)
        {
            return CirclePointClosest(c, p, out _);
        }
        #endregion

        #region CIRCLE POINT DISTANCE
        public static float CirclePointDistance(in Circle c, in Point p, out float pointDistance)
        {
            pointDistance = PointDistance(c.center, p);

            if (pointDistance <= c.radius)
            {
                return 0f;
            }

            return pointDistance - c.radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CirclePointDistance(in Circle c, in Point p)
        {
            return CirclePointDistance(c, p, out _);
        }
        #endregion

        #region CIRCLE POINT CONTAIN
        public static bool CirclePointContain(in Circle c, in Point p, out float circleDistance, out float pointDistance)
        {
            circleDistance = CirclePointDistance(c, p, out pointDistance);

            return circleDistance <= INTERSECTION_EPSILON;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CirclePointContain(in Circle c, in Point p)
        {
            return CirclePointContain(c, p, out _, out _);
        }
        #endregion

        #region CIRCLE CLOSEST
        public static PointPair CircleClosest(in Circle c1, in Circle c2, out float centerDistance)
        {
            centerDistance = PointDistance(c1.center, c2.center);

            var xDir = (c2.x - c1.x) / centerDistance;
            var yDir = (c2.y - c1.y) / centerDistance;

            Point c1Closest;

            if (centerDistance <= c1.radius)
            {
                c1Closest = c2.center;
            }
            else
            {
                c1Closest = new Point
                (
                    c1.x + (xDir * c1.radius),
                    c1.y + (yDir * c1.radius)
                );
            }

            Point c2Closest;

            if (centerDistance <= c2.radius)
            {
                c2Closest = c1.center;
            }
            else
            {
                c2Closest = new Point
                (
                    c2.x + ((-xDir) * c2.radius),
                    c2.y + ((-yDir) * c2.radius)
                );
            }

            return new PointPair(c1Closest, c2Closest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CircleClosest(in Circle c1, in Circle c2)
        {
            return CircleClosest(c1, c2, out _);
        }
        #endregion

        #region CIRCLE DISTANCE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CircleDistance(in Circle c1, in Circle c2)
        {
            return PointDistance(c1.center, c2.center) - (c1.radius + c2.radius);
        }
        #endregion

        #region CIRCLE INTERSECT
        public static bool CircleIntersect(in Circle c1, in Circle c2, out float centerDistance)
        {
            centerDistance = PointDistance(c1.center, c2.center);

            return centerDistance <= (c1.radius + c2.radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleIntersect(in Circle c1, in Circle c2)
        {
            return CircleIntersect(c1, c2, out _);
        }
        #endregion
    
        #region LINE POINT DISTANCE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LinePointDistance(in Line l, in Point p)
        {
            return LinePointDistance(l, p.x, p.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float LinePointDistance(in Line l, float px, float py)
        {
            return MathF.Abs((l.yMultiplier * px) + (l.xMultiplier * py) + l.constant) / MathF.Sqrt((l.yMultiplier * l.yMultiplier) + (l.xMultiplier * l.xMultiplier));
        }
        #endregion

        #region LINE INTERSECT
        public static bool LineIntersect(in Line l1, in Line l2, out Point intersection)
        {
            intersection = default;

            float denominator = (l1.xMultiplier * l2.yMultiplier) - (l2.xMultiplier * l1.yMultiplier);
            if (MathF.Abs(denominator) >= EPSILON)
            {
                intersection.x = ((l1.constant * l2.xMultiplier) - (l2.constant * l1.xMultiplier)) / denominator;
                intersection.y = ((l1.constant * l2.yMultiplier) - (l2.constant * l1.yMultiplier)) / denominator;

                return true;
            }
            else
            {
                // They are parallel.

                return false;
            }
        }

        public static bool LineIntersect(Line l1, Line l2)
        {
            return LineIntersect(l1, l2, out _);
        }
        #endregion

        #region SEGMENT POINT CLOSEST
        public static Point SegmentPointClosest(in Segment s, in Point p)
        {
            float abx = s.b.x - s.a.x;
            float aby = s.b.y - s.a.y;

            float apx = p.x - s.a.x;
            float apy = p.y - s.a.y;

            var abSqrMag = (abx * abx) + (aby * aby);

            float t;

            if (abSqrMag <= 0f)
            {
                t = -1f;
            }
            else
            {
                t = ((apx * abx) + (apy * aby)) / abSqrMag;
            }

            if (t <= 0.0f)
            {
                return s.a;
            }
            else if (t >= 1.0f)
            {
                return s.b;
            }
            else
            {
                return new Point
                (
                    s.a.x + (t * abx),
                    s.a.y + (t * aby)
                );
            }
        }

        public static float SegmentPointClosestInterpolant(in Segment s, in Point p)
        {
            float abx = s.b.x - s.a.x;
            float aby = s.b.y - s.a.y;

            float apx = p.x - s.a.x;
            float apy = p.y - s.a.y;

            var abSqrMag = (abx * abx) + (aby * aby);

            float t;

            if (abSqrMag <= 0f)
            {
                t = -1f;
            }
            else
            {
                t = ((apx * abx) + (apy * aby)) / abSqrMag;
            }

            if (t <= 0.0f)
            {
                return 0f;
            }
            else if (t >= 1.0f)
            {
                return 1f;
            }
            else
            {
                return t;
            }
        }
        #endregion

        #region SEGMENT POINT DISTANCE
        public static float SegmentPointDistanceSquared(in Segment s, in Point p, out Point closest)
        {
            closest = SegmentPointClosest(s, p);

            var dx = p.x - closest.x;
            var dy = p.y - closest.y;

            return (dx * dx) + (dy * dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentPointDistanceSquared(in Segment s, in Point p)
        {
            return SegmentPointDistanceSquared(s, p, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentPointDistance(in Segment s, in Point p, out Point closest)
        {
            return MathF.Sqrt(SegmentPointDistanceSquared(s, p, out closest));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentPointDistance(in Segment s, in Point p)
        {
            return MathF.Sqrt(SegmentPointDistanceSquared(s, p));
        }
        #endregion

        #region SEGMENT CIRCLE CLOSEST
        public static PointPair SegmentCircleClosest(in Segment s, in Circle c, out float centerDistance)
        {
            centerDistance = SegmentPointDistance(s, c.center, out var segmentClosest);

            if (centerDistance <= c.radius)
            {
                return new PointPair(segmentClosest, segmentClosest);
            }

            var xDiff = segmentClosest.x - c.x;
            var yDiff = segmentClosest.y - c.y;

            return new PointPair
            (
                segmentClosest,
                new Point
                (
                    c.x + ((xDiff / centerDistance) * c.radius),
                    c.y + ((yDiff / centerDistance) * c.radius)
                )
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair SegmentCircleClosest(in Segment s, in Circle c)
        {
            return SegmentCircleClosest(s, c, out _);
        }
        #endregion

        #region SEGMENT CIRCLE DISTANCE
        public static float SegmentCircleDistance(in Segment s, in Circle c, out Point segmentClosest, out float centerDistance)
        {
            centerDistance = SegmentPointDistance(s, c.center, out segmentClosest);

            return centerDistance - c.radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentCircleDistance(in Segment s, in Circle c, out Point segmentClosest)
        {
            return SegmentCircleDistance(s, c, out segmentClosest, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentCircleDistance(in Segment s, in Circle c)
        {
            return SegmentCircleDistance(s, c, out _, out _);
        }
        #endregion

        #region SEGMENT CIRCLE INTERSECT
        public static bool SegmentCircleIntersect(in Segment s, in Circle c, out Point segmentClosest, out float circleDistance, out float centerDistance)
        {
            circleDistance = SegmentCircleDistance(s, c, out segmentClosest, out centerDistance);

            return circleDistance <= INTERSECTION_EPSILON;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SegmentCircleIntersect(in Segment s, in Circle c, out Point segmentClosest)
        {
            return SegmentCircleIntersect(s, c, out segmentClosest, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SegmentCircleIntersect(in Segment s, in Circle c, out float circleDistance, out float centerDistance)
        {
            return SegmentCircleIntersect(s, c, out _, out circleDistance, out centerDistance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SegmentCircleIntersect(in Segment s, in Circle c)
        {
            return SegmentCircleIntersect(s, c, out _, out _, out _);
        }
        #endregion

        #region SEGMENT CLOSEST
        public static PointPair SegmentClosest(in Segment s1, in Segment s2, out Point lineIntersect, out bool lineParallel)
        {
            if (LineIntersect(new Line(s1), new Line(s2), out lineIntersect))
            {
                lineParallel = false;

                return new PointPair(SegmentPointClosest(s1, lineIntersect), SegmentPointClosest(s2, lineIntersect));
            }
            else
            {
                lineParallel = true;

                // S1 to S2.a (Default)
                var s1Closest = SegmentPointClosest(s1, s2.a);

                var s2Closest = s2.a;

                var distanceSqrd = PointDistanceSquared(s1Closest, s2Closest);


                // S1 to S2.b
                var p = SegmentPointClosest(s1, s2.b);

                float test = PointDistanceSquared(p, s2.b);

                if (test < distanceSqrd)
                {
                    s1Closest = p;

                    s2Closest = s2.b;

                    distanceSqrd = test;
                }


                // S2 to S1.a
                p = SegmentPointClosest(s2, s1.a);

                test = PointDistanceSquared(p, s1.a);

                if (test < distanceSqrd)
                {
                    s1Closest = s1.a;

                    s2Closest = p;

                    distanceSqrd = test;
                }


                // S2 to S1.b
                p = SegmentPointClosest(s2, s1.b);

                test = PointDistanceSquared(p, s1.b);

                if (test < distanceSqrd)
                {
                    s1Closest = s1.b;

                    s2Closest = p;

                    distanceSqrd = test;
                }

                return new PointPair(s1Closest, s2Closest);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair SegmentClosest(in Segment s1, in Segment s2, out Point lineIntersect)
        {
            return SegmentClosest(s1, s2, out lineIntersect, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair SegmentClosest(in Segment s1, in Segment s2)
        {
            return SegmentClosest(s1, s2, out _, out _);
        }
        #endregion
    
        #region SEGMENT DISTANCE
        public static float SegmentDistanceSquared(in Segment s1, in Segment s2, out PointPair segmentClosestPair, out Point lineIntersect, out bool lineParallel)
        {
            segmentClosestPair = SegmentClosest(s1, s2, out lineIntersect, out lineParallel);

            return PointDistanceSquared(segmentClosestPair.p1, segmentClosestPair.p2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentDistanceSquared(in Segment s1, in Segment s2, out PointPair segmentClosestPair)
        {
            return SegmentDistanceSquared(s1, s2, out segmentClosestPair, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentDistanceSquared(in Segment s1, in Segment s2)
        {
            return SegmentDistanceSquared(s1, s2, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentDistance(in Segment s1, in Segment s2, out PointPair segmentClosestPair, out Point lineIntersect, out bool lineParallel)
        {
            return MathF.Sqrt(SegmentDistanceSquared(s1, s2, out segmentClosestPair, out lineIntersect, out lineParallel));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentDistance(in Segment s1, in Segment s2, out PointPair segmentClosestPair)
        {
            return MathF.Sqrt(SegmentDistanceSquared(s1, s2, out segmentClosestPair));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SegmentDistance(in Segment s1, in Segment s2)
        {
            return MathF.Sqrt(SegmentDistanceSquared(s1, s2));
        }
        #endregion

        #region SEGMENT INTERSECT
        public static bool SegmentIntersect(in Segment s1, in Segment s2, out PointPair segmentClosestPair, out float distance)
        {
            distance = SegmentDistance(s1, s2, out segmentClosestPair);

            if (distance <= INTERSECTION_EPSILON)
            {
                distance = 0f;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SegmentIntersect(in Segment s1, in Segment s2, out PointPair segmentClosestPair)
        {
            return SegmentIntersect(s1, s2, out segmentClosestPair, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SegmentIntersect(in Segment s1, in Segment s2)
        {
            return SegmentIntersect(s1, s2, out _);
        }
        #endregion
    
        #region CAPSULE POINT CLOSEST
        public static Point CapsulePointClosest(in Capsule c, in Point p, out Point segmentClosest, out float segmentDistance)
        {
            segmentClosest = SegmentPointClosest(c.segment, p);

            var xDiff = p.x - segmentClosest.x;
            var yDiff = p.y - segmentClosest.y;

            segmentDistance = MathF.Sqrt((xDiff * xDiff) + (yDiff * yDiff));

            if (segmentDistance <= c.radius)
            {
                return p;
            }

            return new Point
            (
                segmentClosest.x + ((xDiff / segmentDistance) * c.radius),
                segmentClosest.y + ((yDiff / segmentDistance) * c.radius)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point CapsulePointClosest(in Capsule c, in Point p, out Point segmentClosest)
        {
            return CapsulePointClosest(c, p, out segmentClosest, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point CapsulePointClosest(in Capsule c, in Point p)
        {
            return CapsulePointClosest(c, p, out _, out _);
        }
        #endregion

        #region CAPSULE POINT DISTANCE
        public static float CapsulePointDistance(in Capsule c, in Point p, out float segmentDistance, out Point segmentClosest)
        {
            segmentDistance = SegmentPointDistance(c.segment, p, out segmentClosest);

            return segmentDistance - c.radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsulePointDistance(in Capsule c, in Point p, out float segmentDistance)
        {
            return CapsulePointDistance(c, p, out segmentDistance, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsulePointDistance(in Capsule c, in Point p, out Point segmentClosest)
        {
            return CapsulePointDistance(c, p, out _, out segmentClosest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsulePointDistance(in Capsule c, in Point p)
        {
            return CapsulePointDistance(c, p, out _, out _);
        }
        #endregion

        #region CAPSULE POINT CONTAIN
        public static bool CapsulePointContain(in Capsule c, in Point p, out float capsuleDistance, out float segmentDistance)
        {
            capsuleDistance = CapsulePointDistance(c, p, out segmentDistance);

            return capsuleDistance <= INTERSECTION_EPSILON;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsulePointContain(in Capsule c, in Point p)
        {
            return CapsulePointContain(c, p, out _, out _);
        }
        #endregion

        #region CAPSULE CIRCLE CLOSEST
        public static PointPair CapsuleCircleClosest(in Capsule capsule, in Circle circle, out Point segmentClosest, out float centerDistance)
        {
            segmentClosest = SegmentPointClosest(capsule.segment, circle.center);

            var xDiff = circle.x - segmentClosest.x;
            var yDiff = circle.y - segmentClosest.y;

            centerDistance = MathF.Sqrt((xDiff * xDiff) + (yDiff * yDiff));

            xDiff /= centerDistance; // Normalize.
            yDiff /= centerDistance; // Normalize.

            Point capsuleClosest;

            if (centerDistance <= capsule.radius)
            {
                capsuleClosest = circle.center;
            }
            else
            {
                capsuleClosest = new Point
                (
                    segmentClosest.x + (xDiff * capsule.radius),
                    segmentClosest.y + (yDiff * capsule.radius)
                );
            }

            Point circleClosest;

            if (centerDistance <= circle.radius)
            {
                circleClosest = segmentClosest;
            }
            else
            {
                circleClosest = new Point
                (
                    segmentClosest.x + ((-xDiff) * circle.radius),
                    segmentClosest.y + ((-yDiff) * circle.radius)
                );
            }

            return new PointPair(capsuleClosest, circleClosest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleCircleClosest(in Capsule capsule, in Circle circle, out Point segmentClosest)
        {
            return CapsuleCircleClosest(capsule, circle, out segmentClosest, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleCircleClosest(in Capsule capsule, in Circle circle, out float centerDistance)
        {
            return CapsuleCircleClosest(capsule, circle, out _, out centerDistance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleCircleClosest(in Capsule capsule, in Circle circle)
        {
            return CapsuleCircleClosest(capsule, circle, out _, out _);
        }
        #endregion
    
        #region CAPSULE CIRCLE DISTANCE
        public static float CapsuleCircleDistance(in Capsule capsule, in Circle circle, out Point segmentClosest, out float centerDistance)
        {
            centerDistance = SegmentPointDistance(capsule.segment, circle.center, out segmentClosest);

            return centerDistance - (capsule.radius + circle.radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleCircleDistance(in Capsule capsule, in Circle circle, out Point segmentClosest)
        {
            return CapsuleCircleDistance(capsule, circle, out segmentClosest, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleCircleDistance(in Capsule capsule, in Circle circle, out float centerDistance)
        {
            return CapsuleCircleDistance(capsule, circle, out _, out centerDistance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleCircleDistance(in Capsule capsule, in Circle circle)
        {
            return CapsuleCircleDistance(capsule, circle, out _, out _);
        }
        #endregion

        #region CAPSULE CIRCLE INTERSECT
        public static bool CapsuleCircleIntersect(in Capsule capsule, in Circle circle, out Point segmentClosest, out float distance, out float centerDistance)
        {
            distance = CapsuleCircleDistance(capsule, circle, out segmentClosest, out centerDistance);

            return distance <= INTERSECTION_EPSILON;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleCircleIntersect(in Capsule capsule, in Circle circle, out Point segmentClosest)
        {
            return CapsuleCircleIntersect(capsule, circle, out segmentClosest, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleCircleIntersect(in Capsule capsule, in Circle circle, out float distance, out float centerDistance)
        {
            return CapsuleCircleIntersect(capsule, circle, out _, out distance, out centerDistance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleCircleIntersect(in Capsule capsule, in Circle circle)
        {
            return CapsuleCircleIntersect(capsule, circle, out _, out _, out _);
        }
        #endregion

        #region CAPSULE SEGMENT CLOSEST
        public static PointPair CapsuleSegmentClosest(in Capsule c, in Segment s, out Point capsuleSegmentClosest, out float segmentDistance, out Point lineIntersect, out bool lineParallel)
        {
            segmentDistance = SegmentDistance(c.segment, s, out var segmentClosestPair, out lineIntersect, out lineParallel);

            capsuleSegmentClosest = segmentClosestPair.p1;

            if (segmentDistance < c.radius)
            {
                return new PointPair(segmentClosestPair.p2, segmentClosestPair.p2);
            }

            var xDiff = segmentClosestPair.p2.x - segmentClosestPair.p1.x;
            var yDiff = segmentClosestPair.p2.y - segmentClosestPair.p1.y;

            return new PointPair
            (
                new Point
                (
                    capsuleSegmentClosest.x + ((xDiff / segmentDistance) * c.radius),
                    capsuleSegmentClosest.y + ((yDiff / segmentDistance) * c.radius)
                ),
                segmentClosestPair.p2
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleSegmentClosest(in Capsule c, in Segment s, out Point capsuleSegmentClosest, out float segmentDistance)
        {
            return CapsuleSegmentClosest(c, s, out capsuleSegmentClosest, out segmentDistance, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleSegmentClosest(in Capsule c, in Segment s, out Point capsuleSegmentClosest)
        {
            return CapsuleSegmentClosest(c, s, out capsuleSegmentClosest, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleSegmentClosest(in Capsule c, in Segment s)
        {
            return CapsuleSegmentClosest(c, s, out _, out _);
        }
        #endregion

        #region CAPSULE SEGMENT DISTANCE
        public static float CapsuleSegmentDistance(in Capsule c, in Segment s, out float segmentDistance, out PointPair segmentClosestPair, out Point lineIntersect, out bool lineParallel)
        {
            segmentDistance = SegmentDistance(c.segment, s, out segmentClosestPair, out lineIntersect, out lineParallel);

            return segmentDistance - c.radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleSegmentDistance(in Capsule c, in Segment s, out float segmentDistance, out PointPair segmentClosestPair)
        {
            return CapsuleSegmentDistance(c, s, out segmentDistance, out segmentClosestPair, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleSegmentDistance(in Capsule c, in Segment s, out float segmentDistance)
        {
            return CapsuleSegmentDistance(c, s, out segmentDistance, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleSegmentDistance(in Capsule c, in Segment s)
        {
            return CapsuleSegmentDistance(c, s, out _, out _);
        }
        #endregion
    
        #region CAPSULE SEGMENT INTERSECT
        public static bool CapsuleSegmentIntersect(in Capsule c, in Segment s, out PointPair segmentClosestPair, out float distance, out float segmentDistance)
        {
            distance = CapsuleSegmentDistance(c, s, out segmentDistance, out segmentClosestPair);

            return distance <= INTERSECTION_EPSILON;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleSegmentIntersect(in Capsule c, in Segment s, out PointPair segmentClosestPair)
        {
            return CapsuleSegmentIntersect(c, s, out segmentClosestPair, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleSegmentIntersect(in Capsule c, in Segment s, out float distance, out float segmentDistance)
        {
            return CapsuleSegmentIntersect(c, s, out _, out distance, out segmentDistance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleSegmentIntersect(in Capsule c, in Segment s)
        {
            return CapsuleSegmentIntersect(c, s, out _, out _, out _);
        }
        #endregion

        #region CAPSULE CLOSEST
        public static PointPair CapsuleClosest(in Capsule c1, in Capsule c2, out PointPair segmentClosestPair, out float segmentDistance)
        {
            segmentDistance = SegmentDistance(c1.segment, c2.segment, out segmentClosestPair);

            var xDir = (segmentClosestPair.p2.x - segmentClosestPair.p1.x) / segmentDistance;
            var yDir = (segmentClosestPair.p2.y - segmentClosestPair.p1.y) / segmentDistance;

            Point c1Closest;

            if (segmentDistance <= c1.radius)
            {
                c1Closest = segmentClosestPair.p2;
            }
            else
            {
                c1Closest = new Point
                (
                    segmentClosestPair.p1.x + (xDir * c1.radius),
                    segmentClosestPair.p1.y + (yDir * c1.radius)
                );
            }

            Point c2Closest;

            if (segmentDistance <= c2.radius)
            {
                c2Closest = segmentClosestPair.p1;
            }
            else
            {
                c2Closest = new Point
                (
                    segmentClosestPair.p2.x + ((-xDir) * c2.radius),
                    segmentClosestPair.p2.y + ((-yDir) * c2.radius)
                );
            }

            return new PointPair(c1Closest, c2Closest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleClosest(in Capsule c1, in Capsule c2, out PointPair segmentClosestPair)
        {
            return CapsuleClosest(c1, c2, out segmentClosestPair, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleClosest(in Capsule c1, in Capsule c2, out float segmentDistance)
        {
            return CapsuleClosest(c1, c2, out _, out segmentDistance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair CapsuleClosest(in Capsule c1, in Capsule c2)
        {
            return CapsuleClosest(c1, c2, out _, out _);
        }
        #endregion
    
        #region CAPSULE DISTANCE
        public static float CapsuleDistance(in Capsule c1, in Capsule c2, out float segmentDistance, out PointPair segmentClosestPair)
        {
            segmentDistance = SegmentDistance(c1.segment, c2.segment, out segmentClosestPair);

            return segmentDistance - (c1.radius + c2.radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleDistance(in Capsule c1, in Capsule c2, out float segmentDistance)
        {
            return CapsuleDistance(c1, c2, out segmentDistance, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleDistance(in Capsule c1, in Capsule c2, out PointPair segmentClosestPair)
        {
            return CapsuleDistance(c1, c2, out _, out segmentClosestPair);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CapsuleDistance(in Capsule c1, in Capsule c2)
        {
            return CapsuleDistance(c1, c2, out _, out _);
        }
        #endregion
    
        #region CAPSULE INTERSECT
        public static bool CapsuleIntersect(in Capsule c1, in Capsule c2, out PointPair segmentClosestPair, out float distance, out float segmentDistance)
        {
            distance = CapsuleDistance(c1, c2, out segmentDistance, out segmentClosestPair);

            return distance <= INTERSECTION_EPSILON;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleIntersect(in Capsule c1, in Capsule c2, out PointPair segmentClosestPair)
        {
            return CapsuleIntersect(c1, c2, out segmentClosestPair, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleIntersect(in Capsule c1, in Capsule c2, out float distance, out float segmentDistance)
        {
            return CapsuleIntersect(c1, c2, out _, out distance, out segmentDistance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CapsuleIntersect(in Capsule c1, in Capsule c2)
        {
            return CapsuleIntersect(c1, c2, out _, out _, out _);
        }
        #endregion

        #region TRIANGLE POINT CONTAIN
        public static bool TrianglePointContain(in Triangle t, in Point p)
        {
            var s = (t.a.x - t.c.x) * (p.y - t.c.y) - (t.a.y - t.c.y) * (p.x - t.c.x);
            var u = (t.b.x - t.a.x) * (p.y - t.a.y) - (t.b.y - t.a.y) * (p.x - t.a.x);

            if (((s < 0) != (u < 0)) && (s != 0) && (u != 0))
            {
                return false;
            }

            var d = ((t.c.x - t.b.x) * (p.y - t.b.y)) - ((t.c.y - t.b.y) * (p.x - t.b.x));
            return (d == 0) || ((d < 0) == (s + u <= 0));
        }
        #endregion

        #region TRIANGLE POINT CLOSEST
        public static Point TrianglePointClosest(in Triangle t, in Point p, out Point abClosest, out Point bcClosest, out Point caClosest, out float closestDistanceSqrd, out float abDistanceSqrd, out float bcDistanceSqrd, out float caDistanceSqrd)
        {
            abDistanceSqrd = SegmentPointDistanceSquared(t.ab, p, out abClosest);
            bcDistanceSqrd = SegmentPointDistanceSquared(t.bc, p, out bcClosest);
            caDistanceSqrd = SegmentPointDistanceSquared(t.ca, p, out caClosest);

            if (TrianglePointContain(t, p))
            {
                closestDistanceSqrd = 0f;

                return p;
            }

            if (abDistanceSqrd <= bcDistanceSqrd)
            {
                if (abDistanceSqrd <= caDistanceSqrd)
                {
                    closestDistanceSqrd = abDistanceSqrd;

                    return abClosest;
                }
                else
                {
                    closestDistanceSqrd = caDistanceSqrd;

                    return caClosest;
                }
            }
            else if (bcDistanceSqrd <= caDistanceSqrd)
            {
                closestDistanceSqrd = bcDistanceSqrd;

                return bcClosest;
            }
            else
            {
                closestDistanceSqrd = caDistanceSqrd;

                return caClosest;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point TrianglePointClosest(in Triangle t, in Point p, out Point abClosest, out Point bcClosest, out Point caClosest, out float closestDistanceSqrd)
        {
            return TrianglePointClosest(t, p, out abClosest, out bcClosest, out caClosest, out closestDistanceSqrd, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point TrianglePointClosest(in Triangle t, in Point p, out Point abClosest, out Point bcClosest, out Point caClosest)
        {
            return TrianglePointClosest(t, p, out abClosest, out bcClosest, out caClosest, out _, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point TrianglePointClosest(in Triangle t, in Point p, out float closestDistanceSqrd, out float abDistanceSqrd, out float bcDistanceSqrd, out float caDistanceSqrd)
        {
            return TrianglePointClosest(t, p, out _, out _, out _, out closestDistanceSqrd, out abDistanceSqrd, out bcDistanceSqrd, out caDistanceSqrd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point TrianglePointClosest(in Triangle t, in Point p, out float closestDistanceSqrd)
        {
            return TrianglePointClosest(t, p, out _, out _, out _, out closestDistanceSqrd, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point TrianglePointClosest(in Triangle t, in Point p)
        {
            return TrianglePointClosest(t, p, out _, out _, out _, out _, out _, out _, out _);
        }
        #endregion
    
        #region TRIANGLE CIRCLE CLOSEST
        public static PointPair TriangleCircleClosest(in Triangle t, in Circle c, out Point abClosest, out Point bcClosest, out Point caClosest, out float closestCenterDistance, out float abCenterDistanceSqrd, out float bcCenterDistanceSqrd, out float caCenterDistanceSqrd)
        {
            var triangleClosest = TrianglePointClosest(t, c.center, out abClosest, out bcClosest, out caClosest, out closestCenterDistance, out abCenterDistanceSqrd, out bcCenterDistanceSqrd, out caCenterDistanceSqrd);

            closestCenterDistance = MathF.Sqrt(closestCenterDistance);

            if (closestCenterDistance <= c.radius)
            {
                return new PointPair
                (
                    triangleClosest,
                    c.center
                );
            }

            var xDir = (triangleClosest.x - c.x) / closestCenterDistance;
            var yDir = (triangleClosest.y - c.y) / closestCenterDistance;

            return new PointPair
            (
                triangleClosest,
                new Point
                (
                    c.x + (xDir * c.radius),
                    c.y + (yDir * c.radius)
                )
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair TriangleCircleClosest(in Triangle t, in Circle c, out Point abClosest, out Point bcClosest, out Point caClosest, out float closestCenterDistance)
        {
            return TriangleCircleClosest(t, c, out abClosest, out bcClosest, out caClosest, out closestCenterDistance, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair TriangleCircleClosest(in Triangle t, in Circle c, out Point abClosest, out Point bcClosest, out Point caClosest)
        {
            return TriangleCircleClosest(t, c, out abClosest, out bcClosest, out caClosest, out _, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair TriangleCircleClosest(in Triangle t, in Circle c, out float closestCenterDistance, out float abCenterDistanceSqrd, out float bcCenterDistanceSqrd, out float caCenterDistanceSqrd)
        {
            return TriangleCircleClosest(t, c, out _, out _, out _, out closestCenterDistance, out abCenterDistanceSqrd, out bcCenterDistanceSqrd, out caCenterDistanceSqrd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair TriangleCircleClosest(in Triangle t, in Circle c, out float closestCenterDistance)
        {
            return TriangleCircleClosest(t, c, out _, out _, out _, out closestCenterDistance, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointPair TriangleCircleClosest(in Triangle t, in Circle c)
        {
            return TriangleCircleClosest(t, c, out _, out _, out _, out _, out _, out _, out _);
        }
        #endregion
    
        #region TRIANGLE CIRCLE DISTANCE
        public static float TriangleCircleDistance(in Triangle t, in Circle c, out float centerDistance, out float abDistanceSqrd, out float bcDistanceSqrd, out float caDistanceSqrd, out Point triangleClosest, out Point abClosest, out Point bcClosest, out Point caClosest)
        {
            triangleClosest = TrianglePointClosest(t, c.center, out abClosest, out bcClosest, out caClosest, out centerDistance, out abDistanceSqrd, out bcDistanceSqrd, out caDistanceSqrd);

            centerDistance = MathF.Sqrt(centerDistance);

            return centerDistance - c.radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TriangleCircleDistance(in Triangle t, in Circle c, out float centerDistance, out float abDistanceSqrd, out float bcDistanceSqrd, out float caDistanceSqrd, out Point triangleClosest)
        {
            return TriangleCircleDistance(t, c, out centerDistance, out abDistanceSqrd, out bcDistanceSqrd, out caDistanceSqrd, out triangleClosest, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TriangleCircleDistance(in Triangle t, in Circle c, out float centerDistance, out float abDistanceSqrd, out float bcDistanceSqrd, out float caDistanceSqrd)
        {
            return TriangleCircleDistance(t, c, out centerDistance, out abDistanceSqrd, out bcDistanceSqrd, out caDistanceSqrd, out _, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TriangleCircleDistance(in Triangle t, in Circle c, out float centerDistance)
        {
            return TriangleCircleDistance(t, c, out centerDistance, out _, out _, out _, out _, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TriangleCircleDistance(in Triangle t, in Circle c, out Point triangleClosest, out Point abClosest, out Point bcClosest, out Point caClosest)
        {
            return TriangleCircleDistance(t, c, out _, out _, out _, out _, out triangleClosest, out abClosest, out bcClosest, out caClosest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TriangleCircleDistance(in Triangle t, in Circle c, out Point triangleClosest)
        {
            return TriangleCircleDistance(t, c, out _, out _, out _, out _, out triangleClosest, out _, out _, out _);
        }
        #endregion

        #region TRIANGLE CIRCLE INTERSECT
        public static bool TriangleCircleIntersect(in Triangle t, in Circle c, out Point triangleClosest, out Point abClosest, out Point bcClosest, out Point caClosest, out float closestCircleDistance, out float closestCenterDistance, out float abCenterDistanceSqrd, out float bcCenterDistanceSqrd, out float caCenterDistanceSqrd)
        {
            closestCircleDistance = TriangleCircleDistance(t, c, out closestCenterDistance, out abCenterDistanceSqrd, out bcCenterDistanceSqrd, out caCenterDistanceSqrd, out triangleClosest, out abClosest, out bcClosest, out caClosest);

            return closestCenterDistance <= INTERSECTION_EPSILON;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TriangleCircleIntersect(in Triangle t, in Circle c, out Point triangleClosest, out Point abClosest, out Point bcClosest, out Point caClosest, out float closestCircleDistance, out float closestCenterDistance)
        {
            return TriangleCircleIntersect(t, c, out triangleClosest, out abClosest, out bcClosest, out caClosest, out closestCircleDistance, out closestCenterDistance, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TriangleCircleIntersect(in Triangle t, in Circle c, out Point triangleClosest, out Point abClosest, out Point bcClosest, out Point caClosest)
        {
            return TriangleCircleIntersect(t, c, out triangleClosest, out abClosest, out bcClosest, out caClosest, out _, out _, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TriangleCircleIntersect(in Triangle t, in Circle c, out Point triangleClosest)
        {
            return TriangleCircleIntersect(t, c, out triangleClosest, out _, out _, out _, out _, out _, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TriangleCircleIntersect(in Triangle t, in Circle c, out float closestCircleDistance, out float closestCenterDistance, out float abCenterDistanceSqrd, out float bcCenterDistanceSqrd, out float caCenterDistanceSqrd)
        {
            return TriangleCircleIntersect(t, c, out _, out _, out _, out _, out closestCircleDistance, out closestCenterDistance, out abCenterDistanceSqrd, out bcCenterDistanceSqrd, out caCenterDistanceSqrd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TriangleCircleIntersect(in Triangle t, in Circle c, out float closestCircleDistance, out float closestCenterDistance)
        {
            return TriangleCircleIntersect(t, c, out _, out _, out _, out _, out closestCircleDistance, out closestCenterDistance, out _, out _, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TriangleCircleIntersect(in Triangle t, in Circle c)
        {
            return TriangleCircleIntersect(t, c, out _, out _, out _, out _, out _, out _, out _, out _, out _);
        }
        #endregion
    }
}