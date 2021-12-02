namespace MiK.Charts
{
    public class LineViewModel
    {
        List<DataPoint> dataPoints = new();
        List<ViewPoint> viewPoints = new();
        private long totalTicksPerUnit;
        private DataPoint start;
        private long unit;
        private ChartDisplayType view;

        public int Widht { get; internal set; }
        public int Height { get; internal set; }
        public double UpdateInterval { get; set; }
        public List<ViewPoint> ViewPoints
        {
            get
            {
                return this.viewPoints;
            }
        }
        public List<DataPoint> DataPoints
        {
            get
            {
                return this.dataPoints;
            }
        }

        public void AddPoint(DateTime time, int val)
        {
            var p = new DataPoint(time, val);
            if (this.dataPoints.Count == 0)
            {
                this.start = p;
            }
            else
            {
                p.LogicalX = (int)((p.Time.Ticks - this.start.Time.Ticks) / unit);
            }
            this.dataPoints.Add(p);
        }
        public void Init()
        {
            this.view = ChartDisplayType.ShowAll;

            this.unit = TimeSpan.FromMilliseconds(this.UpdateInterval).Ticks;
            this.viewPoints = new List<ViewPoint>(this.Widht);
        }
        public void Update()
        {
            if (this.dataPoints.Count == 0)
                return;
            var endPoint = this.dataPoints.Last();

            var min = this.start.Time.Ticks;
            var max = endPoint.Time.Ticks;

            switch (this.view)
            {
                case ChartDisplayType.ShowAll:
                    var totalTicksPerUnit = (max - min) / unit;
                    Console.WriteLine("totalTicksPerUnit: " + totalTicksPerUnit);
                    var factor = (float)this.Widht / (float)totalTicksPerUnit;
                    this.Clear();
                    ViewPoint prev = null;
                    foreach (var dataPoint in this.dataPoints)
                    {
                        var p = dataPoint.ToViewPoint(this);
                        p.MapY();
                        p.LogicalX = (int)((dataPoint.Time.Ticks - this.start.Time.Ticks) / unit);
                        p.DebugViewTime = TimeSpan.FromTicks(dataPoint.Time.Ticks - this.start.Time.Ticks);
                        p.X = (int)Math.Ceiling(p.LogicalX * factor);
                        //  this.points.Add(p);
                        Console.WriteLine(p.X);
                        if (prev != null && prev.X == p.X)
                        {
                            prev.Merge(p);
                        }
                        else
                        {
                            this.viewPoints.Add(p);
                        }
                        prev = p;
                    }

                    break;
            }
            /*
              var startPoint = this.dataPoints[0];
              var endPoint = this.dataPoints.Last();
              var min = startPoint.Time.Ticks;
              var max = endPoint.Time.Ticks;

              this.totalTicksPerUnit = (max - min) / unit;
              Console.WriteLine("totalTicksPerUnit: " + totalTicksPerUnit);
              var factor = (float)this.Widht / (float)totalTicksPerUnit;

              this.Clear();

              ViewPoint prev = null;
              foreach (var dataPoint in this.dataPoints)
              {
                  var p = dataPoint.ToViewPoint(this);
                  p.MapY();
                  p.LogicalX = (int)((dataPoint.Time.Ticks - startPoint.Time.Ticks) / unit);
                  p.DebugViewTime = TimeSpan.FromTicks(dataPoint.Time.Ticks - startPoint.Time.Ticks);
                  p.X = (int)Math.Ceiling(p.LogicalX * factor);
                  //  this.points.Add(p);
                  Console.WriteLine(p.X);
                  if (prev != null && prev.X == p.X)
                  {
                      prev.Merge(p);
                  }
                  else
                  {
                      this.viewPoints.Add(p);
                  }
                  prev = p;
              }
              Console.WriteLine("view points count: " + viewPoints.Count);
              */
        }
        public void Clear(bool clearData = false)
        {
            if (clearData) this.dataPoints.Clear();
            this.viewPoints.Clear();
        }
    }
}