namespace BWPVDCharts{
    public class TimePoint{


        public TimePoint(){}
        public TimePoint(DateTime x, int y)
        {
            X = x;
            Y = y;

        }

        public DateTime X { get;set; }

        public int Y { get; set;}
    }
}