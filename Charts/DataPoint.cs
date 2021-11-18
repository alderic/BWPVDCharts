namespace MiK.Charts
{
    public class DataPoint
    {
        private DateTime time;
        private int val;

        public DateTime Time {get {return this.time;}}
        public int Val {get {return this.val;}}
        public DataPoint(DateTime time, int val)
        {
            this.time = time;
            this.val = val;
        }

        internal ViewPoint ToViewPoint(LineViewModel model)
        {
           return new ViewPoint(this, model);
        }
    }
}