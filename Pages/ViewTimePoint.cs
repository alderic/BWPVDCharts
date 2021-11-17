namespace BWPVDCharts {
    public class ViewTimePoint
    {
        private TimePoint point;
        private LogicalView view;
        public int X { get; set;}
        public int Y { get; set;}

        public TimePoint Data {get { return this.point; } }

        public long OffsetX { get; internal set; }
        public TimeSpan DebugView { get; internal set; }
        public int ViewXF { get; internal set; }
        public int ViewXC { get; internal set; }
        public int RealX { get; internal set; }
        public int MinY { get; internal set; }
        public int MaxY { get; internal set; }

        public ViewTimePoint() { }
        public ViewTimePoint(TimePoint point, LogicalView view)
        {
            this.point = point;
            this.view = view;
    
        }
    }
}