namespace BWPVDCharts
{
    public class DataPoint
    {
         public DataPoint(){}
        public DataPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}