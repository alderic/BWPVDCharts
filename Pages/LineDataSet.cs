using System.Drawing;
namespace BWPVDCharts{
    public class LineDataSet {
        List<DataPoint> points = new ();
        List<DataPoint> viewPoints = new() { new DataPoint()};
        public bool ShowAll {get;set;}
        public LogicalView View {get;set;}
        
        public List<DataPoint> ViewPoints {
            get {return this.viewPoints;}
        }
        public List<DataPoint> Points {
            get {return this.points;}
        }
        public void AddPoint(int x, int y){
            this.points.Add(new DataPoint(x, y));
            if (this.ShowAll){

            } else {
              //   await ctx.LineToAsync((this.DataSet.Points[i].X - x)*delta, 300 - this.DataSet.Points[i].Y);
                var startPointIndex = Math.Max(this.Points.Count - 60, 0);
                
                this.viewPoints.Add(ConvertToViewPoint(x, y, this.Points[startPointIndex].X));
            }
        }

        private DataPoint ConvertToViewPoint(int x, int y, int deltaX)
        {
            return new DataPoint(x * this.View.Delta -deltaX, this.View.Height - y);
        }
    }
}