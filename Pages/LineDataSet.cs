using System.Drawing;
namespace BWPVDCharts{
    public class LineDataSet {
        List<DataPoint> points = new List<DataPoint>();

        public List<DataPoint> Points {
            get {return this.points;}
        }
        public void AddPoint(int x, int y){
            this.points.Add(new DataPoint(x, y));
        }
    }
}