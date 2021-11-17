using System.Linq.Expressions;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Excubo.Blazor.Canvas;
using Blazor.Extensions;

namespace BWPVDCharts {
    public class LineChart : ComponentBase {
        [Parameter]
        public LineDataSet DataSet {get;set;}
        bool isShowAll = false;
        Canvas canvas;
        BECanvasComponent  canvasRef;
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
    
            int count = 0;
            builder.OpenComponent<Blazor.Extensions.Canvas.BECanvas>(count++);
            builder.AddAttribute(count++, "Width", (long)this.DataSet.View.Widht);
            builder.AddAttribute(count++, "Height", (long)this.DataSet.View.Height);
       //     builder.AddAttribute(count++, "style", "border: 1px solid gray");
            builder.AddComponentReferenceCapture(count++, inst=> this.canvasRef = (BECanvasComponent)inst);
            builder.CloseComponent();
       
           // base.BuildRenderTree(builder);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
           
            await UpdateCanvasAsync();  
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task UpdateCanvasAsync()
        {
            var view = this.DataSet.View;
           
           using  (var context = await this.canvasRef.CreateCanvas2DAsync()) {
              await context.BeginBatchAsync();
                await context.ClearRectAsync(0, 0, this.DataSet.View.Widht, this.DataSet.View.Height);
               
                 if (view.Points.Count == 0) 
                return;
               /* await ctx.SetTransformAsync(1, 0, 0, 1, 0, 0);
                await ctx.RestoreAsync();
                await ctx.SaveAsync();*/
                await context.BeginPathAsync(  );

                var i = 0;
                ViewTimePoint point = null;
                var pointCount = view.Points.Count;
                await context.MoveToAsync(0, view.StartPoint.Y);
                Console.WriteLine(view.StartPoint.Y);
                while (i < pointCount){
                    point = view.Points[i];
                   
                    await context.LineToAsync(point.X, point.Y);
             
                    i++;
                }
                await context.SetStrokeStyleAsync("#ccc");
                  await context.SetLineWidthAsync(2);
                   await context.StrokeAsync();

          await context.EndBatchAsync();
              
           }
   

           return;
            await using (var ctx = await canvas.GetContext2DAsync())
            {

                await ctx.ClearRectAsync(0, 0, this.DataSet.View.Widht, this.DataSet.View.Height);
                 if (view.Points.Count == 0) 
                return;
               /* await ctx.SetTransformAsync(1, 0, 0, 1, 0, 0);
                await ctx.RestoreAsync();
                await ctx.SaveAsync();*/
                await ctx.BeginPathAsync(  );

                var i = 0;
                ViewTimePoint point = null;
                var pointCount = view.Points.Count;
                await ctx.MoveToAsync(0, view.StartPoint.Y);
                Console.WriteLine(view.StartPoint.Y);
                while (i < pointCount){
                    point = view.Points[i];
                    await ctx.LineToAsync(point.X, point.Y);
                    if (i == 0)
                        Console.WriteLine(point.Y);
                    i++;
                }
               /* var delta = 300.0/60.0;
                if (isShowAll) {
                    await ctx.MoveToAsync(0, 300);
                    foreach (var p in this.DataSet.Points){
                        await ctx.LineToAsync(p.X*delta, 300 - p.Y);
                    }
                } else {
                    var startPoint = Math.Max(this.DataSet.Points.Count - 60, 0);
                    var x = 0;
                    if (startPoint == 0) {
                        await ctx.MoveToAsync(0, 300);
                    }
                    else {
                        x = this.DataSet.Points[startPoint].X;
                        await ctx.MoveToAsync(0, 300 - this.DataSet.Points[startPoint].Y);
                        startPoint++;
                }
                    for (var i = startPoint; i < this.DataSet.Points.Count; i++){
                            await ctx.LineToAsync((this.DataSet.Points[i].X - x)*delta, 300 - this.DataSet.Points[i].Y);
                    }
                }*/
                await ctx.LineWidthAsync(1);
                await ctx.StrokeStyleAsync("#086afc");
                await ctx.StrokeAsync();
              //  await ctx.ClosePathAsync();
            }
        }

        private string BuildPointArray()
        {
            var sb = new StringBuilder();
            foreach (var p in this.DataSet.Points){
                sb.Append(p.X);
                sb.Append(",");
                sb.Append(p.Y);
                sb.AppendLine(" ");
            }
            return sb.ToString();
        }
        public void Update(){
            this.StateHasChanged();
        }
    }
}