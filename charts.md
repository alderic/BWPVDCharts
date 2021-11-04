https://github.com/mariusmuntean/ChartJs.Blazor
dotnet add package ChartJs.Blazor.Fork

<script src="https://cdn.jsdelivr.net/npm/chart.js@2.9.4/dist/Chart.min.js"></script>

<!-- This is the glue between Blazor and Chart.js -->
<script src="_content/ChartJs.Blazor.Fork/ChartJsBlazorInterop.js"></script>
If you are using a time scale (TimeAxis), you also need to include Moment.js before including Chart.js.

<script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>


ChartJs.Blazor.Common
ChartJs.Blazor.Common.Axes
ChartJs.Blazor.Common.Axes.Ticks
ChartJs.Blazor.Common.Enums
ChartJs.Blazor.Common.Handlers
ChartJs.Blazor.Common.Time
ChartJs.Blazor.Util
ChartJs.Blazor.Interop
