//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

namespace LiveCharts.Helpers
{
    /// <summary>
    /// Specifies the reason that throw the exception.
    /// </summary>
    internal enum ExceptionReason
    {
        [Reason("The given point can not be casted as FinancialChartPoint.")]
        FinancialPointCastFailed,

        [Reason("There is an invalid series in the series collection, verify " +
                "that all the series implement IPieSeries.")]
        NotAPieSeries,

        [Reason("The view does not implement IBezierPointView.")]
        BezierViewRequired,

        [Reason("There is no a valid gradient to create a heat series.")]
        HeatGradientRequired,

        [Reason("There is a invalid series in the series collection, verify " +
                "that all the series implement ICartesianSeries.")]
        NotACartesianSeries,

        [Reason("LiveCharts does not know how to plot the given type, " +
                "you can either, use an already configured type " +
                "or configure this type you are trying to use")]
        UnknowTypeToPlot,

        [Reason("One axis has an invalid range, it is or it " +
                "tends to zero, please ensure your axis has a valid " +
                "range")]
        InvalidAxisRange,

        [Reason("The current tooltip is not valid, ensure it implements " +
                "IChartsTooltip")]
        InvalidTooltipException,

        [Reason("The current legend is not valid, ensure it implements " +
                "IChartLegend")]
        InvalidLegend,

        [Reason("TicksStep property is too small compared with the range in the " +
                "gauge, to avoid performance issues, please increase it.")]
        InvalidGaugeTicks,

        [Reason("LabelsStep property is too small compared with the range in " +
                "the gauge, to avoid performance issues, please increase it.")]
        InvalidGaugeLabelStep
    }
}