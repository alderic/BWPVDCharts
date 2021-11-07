using System.Drawing;
namespace BWPVDCharts{
    public class LineDataSet {
        List<TimePoint> points = new ();

        TimePoint StartPoint { get; set; } = new();
        public bool ShowAll {get;set;}
        public LogicalView View {get;set;}
        

        public List<TimePoint> Points {
            get {return this.points;}
        }
        public LineDataSet(){
          //  this.viewPoints.Add(this.StartPoint);
        }
        public void AddTimePoint(DateTime x, int y)
        {
            var point = new TimePoint(x, y);
            this.points.Add(point);
            this.View.Add(new ViewTimePoint(point, this.View));

            return;
          /*  if (this.ShowAll){

            }
            else {
                //var lastPoint = /
                
                if (this.View.UseTolerance) {
                    var lastPoint = this.viewPoints.Last();
                    if (Math.Abs(lastPoint.Y - y) < this.View.Tolerance) {
                        lastPoint.X = x;
                        lastPoint.Y = (lastPoint.Y + y) / 2;
                    } else if (lastPoint.Y == y){ 
                        lastPoint.X = x;
                    }else {
                        this.viewPoints.Add(new TimePoint(x, y));
                    }
                }
            
            }*/
           // this.View.MinY = Math.Min(this.View.MinY, y);
         //   this.View.MaxY = Math.Max(this.View.MaxY, y);

        }
     /*   public void AddPoint(long x, int y){
          this.points.Add(new TimePoint(x, y));
            if (this.ShowAll){

            } else {
              //   await ctx.LineToAsync((this.DataSet.Points[i].X - x)*delta, 300 - this.DataSet.Points[i].Y);
                var startPointIndex = Math.Max(this.Points.Count - 60, 0);
                
           //     this.viewPoints.Add(ConvertToViewPoint(x, y, this.Points[startPointIndex].X));
            }
        }*/

      /*  private DataPoint ConvertToViewPoint(long x, int y, long deltaX)
        {
            return new DataPoint(x * this.View.Delta -deltaX, this.View.Height - y);
        }*/
    }
}