using System.Linq.Expressions;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Excubo.Blazor.Canvas;

namespace BWPVDCharts {
    public class LineChart : ComponentBase {
        [Parameter]
        public LineDataSet DataSet {get;set;}
        bool isShowAll = false;
        Canvas canvas;
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
    
            int count = 0;
            builder.OpenComponent<Canvas>(count++);
            builder.AddAttribute(count++, "width", "300");
            builder.AddAttribute(count++, "height", "300");
            builder.AddAttribute(count++, "style", "border: 5px solid red");
            builder.AddComponentReferenceCapture(count++, inst=> this.canvas = (Excubo.Blazor.Canvas.Canvas)inst);
            builder.CloseComponent();
       
           // base.BuildRenderTree(builder);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await UpdateCanvasAsync();
            if (firstRender){
               
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task UpdateCanvasAsync()
        {
             await using (var ctx = await canvas.GetContext2DAsync())
            {

                await ctx.ClearRectAsync(0, 0, 300, 300);
               /* await ctx.SetTransformAsync(1, 0, 0, 1, 0, 0);
                await ctx.RestoreAsync();
                await ctx.SaveAsync();*/
                await ctx.BeginPathAsync(  );
     
                var delta = 300.0/60.0;
                if (isShowAll) {
                    await ctx.MoveToAsync(0, 300);
                    foreach (var p in this.DataSet.Points){
                        await ctx.LineToAsync(p.X*delta, 300 - p.Y);
                    }
                } else {
                    var startPoint = Math.Max(this.DataSet.Points.Count - 60, 0);
                    var x = 0;
                    if (startPoint == 0)
                        await ctx.MoveToAsync(0, 300);
                    else {
                        x = this.DataSet.Points[startPoint].X;
                        await ctx.MoveToAsync(0, 300 - this.DataSet.Points[startPoint].Y);
                        
                        startPoint++;
                }
                    for (var i = startPoint; i < this.DataSet.Points.Count; i++){
                            await ctx.LineToAsync((this.DataSet.Points[i].X - x)*delta, 300 - this.DataSet.Points[i].Y);
                    }
                }
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