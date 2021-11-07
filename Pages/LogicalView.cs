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
            if (this.ShowAll)
                this.UpdateAllInternal();
            else 
                this.UpdateIntervalInternal();
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