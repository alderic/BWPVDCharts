namespace MiK.Charts
{
    public class DataPoint
    {
        private DateTime time;
        private int val;

        public DateTime Time { get { return this.time; } set { this.time = time; } }
        public int Val { get { return this.val; } }
        public int LogicalX { get; set; }
        public DataPoint(DateTime time, int val)
        {
            this.time = time;
            this.val = val;
        }

        internal ViewPoint ToViewPoint(LineViewModel model)
        {
            return new ViewPoint(this, model);
        }

        internal void CalcLogicalX(long ticks, long unit)
        {
            this.LogicalX = (int)((this.time.Ticks - ticks) / unit);
        }
    }
}