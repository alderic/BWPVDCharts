namespace BWPVDCharts {
    public class ViewTimePoint
    {
        private TimePoint point;
        private LogicalView view;
        public int X { get; set;}
        public int Y { get; set;}

        public TimePoint Data {get { return this.point; } }

        public long OffsetX { get; internal set; }
        public ViewTimePoint() { }
        public ViewTimePoint(TimePoint point, LogicalView view)
        {
            this.point = point;
            this.view = view;
    
        }
    }
}