namespace BWPVDCharts{
    public class LogicalView{
        public int Widht {get;set;}
        public int Height {get;set;}
        public int DisplayInterval {get;set;}
        public bool ShowAll {get;set;}
        public int Delta { get; private set; }

        public LogicalView(int w, int h, int s, bool showAll = false){
            this.Widht = w;
            this.Height = h;
            this.DisplayInterval = s;
            this.ShowAll = showAll;
            this.Delta = (int)Math.Round(w / (this.DisplayInterval* 1.0));
        }
    }
}