using System;
using System.Collections.Generic;
using System.Linq;

namespace MiK.Charts
{
    public class LineViewModel
    {
        private readonly List<long> intervals = new()
        {
            new TimeSpan(0, 1, 0).Ticks,
            new TimeSpan(0, 1, 30).Ticks,
            new TimeSpan(0, 2, 15).Ticks,
            new TimeSpan(0, 3, 15).Ticks,
            new TimeSpan(0, 5, 0).Ticks,
            new TimeSpan(0, 7, 30).Ticks,
            new TimeSpan(0, 10, 0).Ticks,
            new TimeSpan(0, 15, 0).Ticks,
            new TimeSpan(0, 20, 0).Ticks,
            new TimeSpan(0, 30, 0).Ticks,
            new TimeSpan(0, 45, 0).Ticks,
            new TimeSpan(1, 0, 0).Ticks,
            new TimeSpan(1, 30, 0).Ticks,
            new TimeSpan(2, 15, 0).Ticks,
            new TimeSpan(3, 15, 0).Ticks,
            new TimeSpan(5, 0, 0).Ticks,
            new TimeSpan(7, 30, 0).Ticks,
            new TimeSpan(10, 0, 0).Ticks,
            new TimeSpan(15, 0, 0).Ticks,
            new TimeSpan(20, 0, 0).Ticks,
            new TimeSpan(24, 0, 0).Ticks
        };
        private int currentIntervalIndex = 1;
        private DataPoint prevPoint;
        List<DataPoint> dataPoints = new();
        List<ViewPoint> viewPoints = new();
        private long totalTicksPerUnit;
        private DataPoint start;
        private long displayInterval;
        private long curentDisplayInterval;
        private long unit;
        public ChartDisplayType View { get; set; }
        public long DisplayInterval => this.displayInterval;
        public double Tolerance { get; set; }
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
            else if (this.dataPoints.Count == 1)
            {
                p.CalcLogicalX(this.start.Time.Ticks, unit);
            }
            else
            {
                p.CalcLogicalX(this.start.Time.Ticks, unit);
                if (this.Tolerance > 0 && Math.Abs(prevPoint.Val - val) < this.Tolerance)
                {
                    prevPoint.LogicalX = p.LogicalX;
                    prevPoint.Time = time;
                }
            }
            prevPoint = p;
            this.dataPoints.Add(p);
        }
        public void Init()
        {
            //   this.view = ChartDisplayType.ShowAll;
            this.displayInterval = TimeSpan.FromSeconds(60).Ticks;
            this.curentDisplayInterval = this.displayInterval;
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

            switch (this.View)
            {
                case ChartDisplayType.DynamicRange:
                    var recalc = CalculateDisplayInterval(min, max);
                    
                  //  var 
                    break;
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

        private bool CalculateDisplayInterval(long min, long max)
        {
            var interval = max - min;
            if (interval > this.intervals[this.currentIntervalIndex])
            {
                this.currentIntervalIndex++;
                this.displayInterval = this.intervals[this.currentIntervalIndex];
                return true;
            }
            return false;
        }


        public void Clear(bool clearData = false)
        {
            if (clearData) this.dataPoints.Clear();
            this.viewPoints.Clear();
        }
    }
}