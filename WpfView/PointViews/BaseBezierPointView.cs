//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodríguez Orozco & LiveCharts Contributors

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

using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts.Wpf.PointViews
{
    /// <summary>
    /// A base bezier point view.
    /// </summary>
    /// <seealso cref="IBezierPointView" />
    public class BaseBezierPointView : IBezierPointView
    {
        /// <summary>
        /// Gets or sets the point shape path.
        /// </summary>
        /// <value>
        /// The point shape path.
        /// </value>
        public Path PointShapePath { get; set; }

        /// <summary>
        /// Gets or sets the bezier.
        /// </summary>
        /// <value>
        /// The bezier.
        /// </value>
        public BezierSegment Bezier { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public ContentControl Label { get; set; }

        /// <summary>
        /// Gets or sets the data to draw.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public BezierData Data { get; set; }

        /// <summary>
        /// Gets or sets the shadow path.
        /// </summary>
        /// <value>
        /// The shadow path.
        /// </value>
        public PathFigure ShadowPath { get; set; }

        /// <summary>
        /// Gets or sets the stroke path.
        /// </summary>
        /// <value>
        /// The stroke path.
        /// </value>
        public PathFigure StrokePath { get; set; }

        /// <inheritdoc cref="IChartPointView.Draw"/>
        public virtual void Draw(ChartPoint previousDrawn, ChartPoint current, int index, ISeriesView series, ChartCore chart)
        {
            var lineSeries = (LineSeries) series;
            
            // --------------------
            // Initializing shapes
            // --------------------

            #region Point shape

            // Initialize the path, if it is required by the series and it is null.
            if (lineSeries.PointGeometry != null && PointShapePath == null)
            {
                PointShapePath = new Path
                {
                    Stretch = Stretch.Fill,
                    StrokeThickness = lineSeries.StrokeThickness
                };

                chart.View.AddToDrawMargin(PointShapePath);
            }

            // map the series properties to the drawn point.
            if (PointShapePath != null)
            {
                PointShapePath.Fill = lineSeries.PointForeground;
                PointShapePath.Stroke = lineSeries.Stroke;
                PointShapePath.StrokeThickness = lineSeries.StrokeThickness;
                PointShapePath.Width = lineSeries.PointGeometrySize;
                PointShapePath.Height = lineSeries.PointGeometrySize;
                PointShapePath.Data = lineSeries.PointGeometry;
                PointShapePath.Visibility = lineSeries.Visibility;
                Panel.SetZIndex(PointShapePath, Panel.GetZIndex(lineSeries) + 1);

                // Obsolete, replaced with IChartPointView.Selected() method.
                // ToDo: Remove the next 2 lines in a future version...
                if (current.Stroke != null) PointShapePath.Stroke = (Brush) current.Stroke;
                if (current.Fill != null) PointShapePath.Fill = (Brush) current.Fill;
            }
            
            // erase the path shape if it is not required any more
            if (lineSeries.PointGeometry == null && PointShapePath != null)
            {
                chart.View.RemoveFromDrawMargin(PointShapePath);
                PointShapePath = null;
            }

            #endregion

            #region Label

            // initialize or update the label.
            if (lineSeries.DataLabels)
            {
                Label = lineSeries.UpdateLabelContent(
                    new DataLabelViewModel
                    {
                        FormattedText = DesignerProperties.GetIsInDesignMode(lineSeries)
                            ? "'label'"
                            : lineSeries.LabelPoint(current),
                        Point = current
                    }, Label);
            }

            // erase data label if it is not required anymore.
            if (!lineSeries.DataLabels && Label != null)
            {
                // notice UpdateLabelContent() added the label to the UI, we need to remove it.
                chart.View.RemoveFromDrawMargin(Label);
                Label = null;
            }

            #endregion

            // --------------------
            // Placing shapes
            // --------------------

            #region Responsive area in chart

            // register the area where the point interacts with the user (hover and click).
            var minDimension = lineSeries.PointGeometrySize < 12 ? 12 : lineSeries.PointGeometrySize;
            current.ResponsiveArea = new ResponsiveRectangle(
                current.ChartLocation.X - minDimension / 2,
                current.ChartLocation.Y - minDimension / 2,
                minDimension,
                minDimension);

            #endregion
        }

        /// <inheritdoc cref="IChartPointView.Erase"/>
        public virtual void Erase(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(PointShapePath);
            chart.View.RemoveFromDrawMargin(Label);
            ShadowPath.Segments.Remove(Bezier);
        }

        /// <inheritdoc cref="IChartPointView.OnHover"/>
        public virtual void OnHover(ChartPoint point)
        {
            var lineSeries = (LineSeries)point.SeriesView;
            if (PointShapePath != null) PointShapePath.Fill = PointShapePath.Stroke;

            lineSeries.PathCollection.ForEach(s => s.StrokePath.StrokeThickness++);
        }

        /// <inheritdoc cref="IChartPointView.OnHoverLeave"/>
        public virtual void OnHoverLeave(ChartPoint point)
        {
            var lineSeries = (LineSeries)point.SeriesView;
            if (PointShapePath != null)
                PointShapePath.Fill = point.Fill == null
                    ? lineSeries.PointForeground
                    : (Brush)point.Fill;

            lineSeries.PathCollection.ForEach(s =>
            {
                s.StrokePath.StrokeThickness = lineSeries.StrokeThickness;
            });
        }

        /// <inheritdoc cref="IChartPointView.OnSelection"/>
        public virtual void OnSelection()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IChartPointView.OnHoverLeave"/>
        public virtual void OnSelectionLeave()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Corrects the x label position.
        /// </summary>
        /// <param name="desiredPosition">The desired position.</param>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        protected double CorrectXLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + Label.ActualWidth * .5 < -0.1) return -Label.ActualWidth;

            if (desiredPosition + Label.ActualWidth > chart.View.DrawMarginWidth)
                desiredPosition -= desiredPosition + Label.ActualWidth - chart.View.DrawMarginWidth + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        /// <summary>
        /// Corrects the y label position.
        /// </summary>
        /// <param name="desiredPosition">The desired position.</param>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        protected double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            desiredPosition -= (PointShapePath?.ActualHeight * .5 ?? 0) + Label.ActualHeight * .5 + 2;

            if (desiredPosition + Label.ActualHeight > chart.View.DrawMarginHeight)
                desiredPosition -= desiredPosition + Label.ActualHeight - chart.View.DrawMarginHeight + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }
    }
}