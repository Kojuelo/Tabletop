namespace Kojuelo.Tabletop
{
    public readonly struct PointPair
    {
        public readonly Point p1;
        
        public readonly Point p2;

        
        public PointPair(in Point p1, in Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}