using System.Text;

namespace BWPVDCharts{
    public class LogicalView{
        public LineDataSet Owner;
        public int Widht {get;set;}
        public int Height {get;set;}
        public int DisplayInterval {get;set;}
        public bool ShowAll {get;set;}

        private long displayIntervalInTicks;

        public int Delta { get; private set; }

        private long interval;
        private double factor;

        public bool UseTolerance {get;set;}
        public int Tolerance {get;set;}
        List<ViewTimePoint> points = new();
        private long minX;
        private ViewTimePoint startPoint = new ();

        private long maxX;
public ViewTimePoint StartPoint {get { return this.startPoint; } }
        public List<ViewTimePoint> Points {
            get {return this.points;}
        }
        public LogicalView(int w, int h, int s, bool showAll = false){
            this.Widht = w;
            this.Height = h;
            this.DisplayInterval = s;
            this.ShowAll = showAll;
            this.displayIntervalInTicks = this.DisplayInterval * TimeSpan.TicksPerSecond;
            var pixelsPerTick = (float)this.Widht / (float)this.DisplayInterval / TimeSpan.TicksPerSecond;

            
            this.factor = pixelsPerTick;
          //  var dt = DateTime.Now;
         //   long ticks = dt.Ticks % TimeSpan.TicksPerSecond;
        }
        public void Switch(){
            this.points.Clear();
            if (this.ShowAll){
        
                foreach (var point in this.Owner.Points)
                {
                    this.AddAllInternal(new ViewTimePoint(point, this));
                }
               

                this.UpdateAllInternal();
            }
            else
            {
                
                foreach (var point in this.Owner.Points)
                {
                    this.AddIntervalInternal(new ViewTimePoint(point, this));
                }
                this.UpdateIntervalInternal();
            }
        }
        public void Update() {
               var start = this.Owner.Points.First();
                var end = this.Owner.Points.Last();
                var min = start.X.Ticks;
                var max = end.X.Ticks;
                var unit = TimeSpan.FromMilliseconds(250).Ticks;
                var totalTicks = (max - min) / (float)unit ;
                 var totalTicks2 = (max - min) / unit;
                var factor = (float)this.Widht /  (float)totalTicks;

                this.points.Clear();
                var sb = new StringBuilder();
                ViewTimePoint prev = null;
                foreach (var point in this.Owner.Points)
                {
                    var p = new ViewTimePoint(point, this);
                    p.Y = this.Height - p.Data.Y;
                    p.RealX = (int)((point.X.Ticks - start.X.Ticks) / unit);
                    p.DebugView = TimeSpan.FromTicks(point.X.Ticks - start.X.Ticks);
                    p.ViewXF = (int) Math.Floor(p.RealX * factor);
                   p.ViewXC =(int) Math.Ceiling(p.RealX * factor);
                    p.X = p.ViewXC;
                  //  this.points.Add(p);
                   if (prev != null && prev.X == p.X){
                        prev.MinY = Math.Min(prev.MinY, p.Y);
                        prev.MaxY = Math.Max(prev.MaxY, p.Y);
                         prev = p;
                    } else {
                    this.points.Add(p);
                    
                    prev = p;
                    prev.MinY = prev.Y;
                    prev.MaxY = prev.Y;
                    }
                    
                }
            return;
            if (this.ShowAll)
                this.UpdateAllInternal();
            else
                this.UpdateIntervalInternal();

         //   this.ConvertToBezier();
        }

        private void ConvertToBezier(double tension = 2)
        {
            var bezierPoints = new List<ViewTimePoint>();

  for (var i = 0; i < points.Count; i++) {
    if (i == 0) {
      bezierPoints.Add(points[0]);
      continue;
    }

    int i1, i2, pointIndex;

    pointIndex = i - 1;
    i1 = pointIndex == 0 ? 0 : pointIndex - 1;
    i2 = pointIndex == points.Count - 1 ? pointIndex : pointIndex + 1;

    var drv1 = new ViewTimePoint {
      X = (int)((points[i2].X - points[i1].X) / tension ),
      Y=  (int)((points[i2].Y - points[i1].Y) / tension )
    };
    var cp1 = new ViewTimePoint{
      X= (int)(points[pointIndex].X + drv1.X / 3),
      Y= (int)( points[pointIndex].Y + drv1.Y / 3)
    };
    bezierPoints.Add(cp1);

    pointIndex = i;
    i1 = pointIndex ==0 ? 0 : pointIndex - 1;
    i2 = pointIndex == points.Count - 1 ? pointIndex : pointIndex + 1;

    var drv2 = new ViewTimePoint{
      X= (int)((points[i2].X - points[i1].X) / tension),
      Y= (int)((points[i2].Y- points[i1].Y) / tension)
    };
    var cp2 = new ViewTimePoint{
      X=(int)(points[pointIndex].X - drv2.X / 3),
      Y=(int)( points[pointIndex].Y - drv2.Y / 3)
    };
    bezierPoints.Add(cp2);

    bezierPoints.Add( points[i]);
    }
    Console.WriteLine(bezierPoints.Count);
}

        private void UpdateIntervalInternal()
        {
             var point = this.points.Last();
                if (point.OffsetX > this.displayIntervalInTicks)
                {
                    //var delta = point.OffsetX - this.displayIntervalInTicks;
                    var start = this.points.First();
                    bool removed = false;
                    while (point.OffsetX - start.OffsetX > this.displayIntervalInTicks)
                    {
                        this.points.RemoveAt(0);
                        start = this.points.First();
                        removed = true;
                    }
                    var newStart = this.points.First();
                    if (removed)
                    {
                        this.startPoint.Y = (start.Y + newStart.Y) / 2;//cal %
                    }
                    this.minX = point.Data.X.Ticks - this.displayIntervalInTicks;
                    foreach (var p in this.points)
                    {
                        p.OffsetX = p.Data.X.Ticks - this.minX;
                        p.X = (int)Math.Round(p.OffsetX * this.factor);
                    }
                }
        }

        private void UpdateAllInternal()
        {
          //  throw new NotImplementedException();

                var start = this.Owner.Points.First();
                var end = this.Owner.Points.Last();
                var min = start.X.Ticks;
                var max = end.X.Ticks;
                var totalTicks = max - min;
                var factor = (float)this.Widht /  (float)totalTicks;
          
            foreach (var p in this.points)
                    {
                        p.OffsetX = p.Data.X.Ticks - min;
                        p.X = (int)Math.Round(p.OffsetX * factor);
                        p.Y = this.Height - p.Data.Y;
                    }
            
        }

        public void Add(ViewTimePoint point){
            if (this.ShowAll)
                this.AddAllInternal(point);
            else 
                this.AddIntervalInternal(point);
            


        }

        private void AddIntervalInternal(ViewTimePoint point)
        {
            point.Y = this.Height - point.Data.Y;
            if (this.points.Count == 0)   {
                this.minX = point.Data.X.Ticks;
                this.startPoint.Y = point.Y;
            }
             
            
            this.points.Add(point);

            this.maxX = point.Data.X.Ticks;
           
            point.OffsetX = point.Data.X.Ticks - this.minX;
            point.X = (int)Math.Round(point.OffsetX  * this.factor);
        }

        private void AddAllInternal(ViewTimePoint point)
        {
            this.points.Add(point);
        }
    }
}