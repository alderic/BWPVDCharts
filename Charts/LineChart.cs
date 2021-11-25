using System.Linq.Expressions;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using Blazor.Extensions;

namespace MiK.Charts {
    public class LineChart: ComponentBase {
        private BECanvasComponent canvasRef;

        [Inject]
        internal Microsoft.JSInterop.IJSRuntime JSRuntime { get; set;}

        [Parameter]
        public LineViewModel Model {get;set;}
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int count = 0;
            builder.OpenComponent<Blazor.Extensions.Canvas.BECanvas>(count++);
            builder.AddAttribute(count++, "Width", (long)this.Model.Widht);
            builder.AddAttribute(count++, "Height", (long)this.Model.Height);
            builder.AddComponentReferenceCapture(count++, inst=> this.canvasRef = (BECanvasComponent)inst);
            builder.CloseComponent();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) {
             //   var pix = await this.JSRuntime.InvokeAsync<int>("_setupCanvas", new object[] {this.canvasRef.CanvasReference, this.Model.Widht, this.Model.Height});
            }
         //   await UpdateCanvasAsync();
            await base.OnAfterRenderAsync(firstRender);
        }

        public void Update(){

        }

        private async Task UpdateCanvasAsync()
        {
            using  (var context = await this.canvasRef.CreateCanvas2DAsync()) {
                await context.BeginBatchAsync();
                await context.ClearRectAsync(0, 0, this.Model.Widht, this.Model.Height);
            
            var a = 1;
            var b =2 ;
            if (this.Model == null)
            Console.WriteLine("ddddd");
                if (this.Model.ViewPoints.Count == 0) {
                    return;
                }
                    await context.BeginPathAsync(  );

                    var i = 0;
                    ViewPoint point = null;
                    var startPoint = this.Model.ViewPoints[0];
                    var pointCount = this.Model.ViewPoints.Count;
                    await context.MoveToAsync(0, startPoint.Y);

                    while (i < pointCount){
                        point = this.Model.ViewPoints[i];
                        if (point.IsComplex)
                        {
                       //     await context.SetStrokeStyleAsync("#e51400");
                            await context.LineToAsync(point.X, point.MinY);
                            await context.LineToAsync(point.X, point.MaxY);
                        //    await context.SetStrokeStyleAsync("#0050ef");
                        }
                        else
                        {
                            await context.LineToAsync(point.X, point.Y);
                        }

                    i++;
                    }
                    await context.SetStrokeStyleAsync("#0050ef");
                    await context.SetLineWidthAsync(2);
                    await context.StrokeAsync();

                    await context.EndBatchAsync();
            }
        }

    }
}