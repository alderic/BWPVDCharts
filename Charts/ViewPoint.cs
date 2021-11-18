namespace MiK.Charts
{
    public class ViewPoint
    {
        private DataPoint dataPoint;
        private LineViewModel model;

        public int X {get;set;}
        public int Y {get;set;}
        public int MinY { get; private set; }
        public int LogicalX { get; internal set; }
        public TimeSpan DebugViewTime { get; internal set; }
        public int MaxY { get; private set; }
        public bool IsComplex { get; private set; }

        public ViewPoint(DataPoint dataPoint, LineViewModel model)
        {
            this.dataPoint = dataPoint;
            this.model = model;
        }

        internal void MapY()
        {
            this.Y = (int)this.model.Height - dataPoint.Val;
            this.MinY = this.MaxY = this.Y;
        }

        internal void Merge(ViewPoint p)
        {
            this.MinY = Math.Min(this.MinY, p.Y);
            this.MaxY = Math.Max(this.MaxY, p.Y);
            this.IsComplex = true;
        }
    }
}