//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez Orozco & LiveCharts Contributors

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
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts.Wpf.Points
{
    /// <summary>
    /// Point drawn by the line series class.
    /// </summary>
    /// <seealso cref="IBezierPointView" />
    public class LineSeriesPointView : IBezierPointView
    {
        /// <summary>
        /// Gets or sets the point shape path.
        /// </summary>
        /// <value>
        /// The point shape path.
        /// </value>
        protected Path PointShapePath { get; set; }
        /// <summary>
        /// Gets or sets the bezier.
        /// </summary>
        /// <value>
        /// The bezier.
        /// </value>
        protected BezierSegment Bezier { get; set; }
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        protected ContentControl Label { get; set; }

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

            #endregion

            #region Data Label

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

            if (Bezier == null)
            {
                Bezier = new BezierSegment();
            }

            ShadowPath.Segments.Remove(Bezier);
            ShadowPath.Segments.Insert(index, Bezier);
            StrokePath.Segments.Remove(Bezier);
            StrokePath.Segments.Add(Bezier);

            Bezier.Point1 = Data.Point1.AsPoint();
            Bezier.Point2 = Data.Point2.AsPoint();
            Bezier.Point3 = Data.Point3.AsPoint();

            if (PointShapePath != null)
            {
                Canvas.SetLeft(PointShapePath, current.ChartLocation.X - PointShapePath.Width * .5);
                Canvas.SetTop(PointShapePath, current.ChartLocation.Y - PointShapePath.Height * .5);
            }

            if (Label != null)
            {
                Label.UpdateLayout();
                var xl = CorrectXLabel(current.ChartLocation.X - Label.ActualWidth * .5, chart);
                var yl = CorrectYLabel(current.ChartLocation.Y - Label.ActualHeight * .5, chart);
                Canvas.SetLeft(Label, xl);
                Canvas.SetTop(Label, yl);
            }
            return;
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

        private double CorrectXLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + Label.ActualWidth * .5 < -0.1) return -Label.ActualWidth;

            if (desiredPosition + Label.ActualWidth > chart.View.DrawMarginWidth)
                desiredPosition -= desiredPosition + Label.ActualWidth - chart.View.DrawMarginWidth + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        private double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            desiredPosition -= (PointShapePath == null ? 0 : PointShapePath.ActualHeight * .5) + Label.ActualHeight * .5 + 2;

            if (desiredPosition + Label.ActualHeight > chart.View.DrawMarginHeight)
                desiredPosition -= desiredPosition + Label.ActualHeight - chart.View.DrawMarginHeight + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }
    }

    public class HorizontalBezierPointView : PointView, IBezierPointView
    {
        public BezierSegment Segment { get; set; }
        public Path Shape { get; set; }
        public PathFigure ShadowContainer { get; set; }
        public PathFigure LineContainer { get; set; }
        public BezierData Data { get; set; }

        public override void Draw(ChartPoint previousDrawn, ChartPoint current, int index, ISeriesView series, ChartCore chart)
        {
            var previosPbv = previousDrawn == null
                ? null
                : (HorizontalBezierPointView) previousDrawn.View;

            var y = chart.View.DrawMarginTop + chart.View.DrawMarginHeight;

            ShadowContainer.Segments.Remove(Segment);
            ShadowContainer.Segments.Insert(index, Segment);
            LineContainer.Segments.Remove(Segment);
            LineContainer.Segments.Add(Segment);

            if (IsNew)
            {
                if (previosPbv != null && !previosPbv.IsNew)
                {
                    Segment.Point1 = previosPbv.Segment.Point3;
                    Segment.Point2 = previosPbv.Segment.Point3;
                    Segment.Point3 = previosPbv.Segment.Point3;

                    if (Shape != null)
                    {
                        Canvas.SetTop(Shape, Canvas.GetTop(previosPbv.Shape));
                        Canvas.SetLeft(Shape, Canvas.GetLeft(previosPbv.Shape));
                    }

                    if (DataLabel != null)
                    {
                        Canvas.SetTop(DataLabel, Canvas.GetTop(previosPbv.DataLabel));
                        Canvas.SetLeft(DataLabel, Canvas.GetLeft(previosPbv.DataLabel));
                    }
                }
                else
                {
                    //if (current.SeriesView.IsFirstDraw)
                    //{
                    //    Segment.Point1 = new Point(Data.Point1.X, y);
                    //    Segment.Point2 = new Point(Data.Point2.X, y);
                    //    Segment.Point3 = new Point(Data.Point3.X, y);

                    //    if (Shape != null)
                    //    {
                    //        Canvas.SetTop(Shape, y);
                    //        Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width*.5);
                    //    }

                    //    if (DataLabel != null)
                    //    {
                    //        Canvas.SetTop(DataLabel, y);
                    //        Canvas.SetLeft(DataLabel, current.ChartLocation.X - DataLabel.ActualWidth * .5);
                    //    }
                    //}
                    //else
                    //{
                        var startPoint = ((LineSeries)current.SeriesView).PathCollection[0].Left.Point;
                        Segment.Point1 = startPoint;
                        Segment.Point2 = startPoint;
                        Segment.Point3 = startPoint;

                        if (Shape != null)
                        {
                            Canvas.SetTop(Shape, y);
                            Canvas.SetLeft(Shape, startPoint.X - Shape.Width * .5);
                        }

                        if (DataLabel != null)
                        {
                            Canvas.SetTop(DataLabel, y);
                            Canvas.SetLeft(DataLabel, startPoint.X - DataLabel.ActualWidth * .5);
                        }
                    //}
                }
            }
            else if (DataLabel != null && double.IsNaN(Canvas.GetLeft(DataLabel)))
            {
                Canvas.SetTop(DataLabel, y);
                Canvas.SetLeft(DataLabel, current.ChartLocation.X - DataLabel.ActualWidth*.5);
            }

            #region No Animated

                if (chart.View.DisableAnimations)
            {
                Segment.Point1 = Data.Point1.AsPoint();
                Segment.Point2 = Data.Point2.AsPoint();
                Segment.Point3 = Data.Point3.AsPoint();

                if (Shape != null)
                {
                    Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width*.5);
                    Canvas.SetTop(Shape, current.ChartLocation.Y - Shape.Height*.5);
                }

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();
                    var xl = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualWidth * .5, chart);
                    var yl = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualHeight * .5, chart);
                    Canvas.SetLeft(DataLabel, xl);
                    Canvas.SetTop(DataLabel, yl);
                }
                return;
            }

            #endregion

            Segment.BeginAnimation(BezierSegment.Point1Property,
                new PointAnimation(Segment.Point1, Data.Point1.AsPoint(), chart.View.AnimationsSpeed));
            Segment.BeginAnimation(BezierSegment.Point2Property,
                new PointAnimation(Segment.Point2, Data.Point2.AsPoint(), chart.View.AnimationsSpeed));
            Segment.BeginAnimation(BezierSegment.Point3Property,
                new PointAnimation(Segment.Point3, Data.Point3.AsPoint(), chart.View.AnimationsSpeed));

            if (Shape != null)
            {
                if (double.IsNaN(Canvas.GetLeft(Shape)))
                {
                    Canvas.SetLeft(Shape, current.ChartLocation.X - Shape.Width * .5);
                    Canvas.SetTop(Shape, current.ChartLocation.Y - Shape.Height * .5);
                }
                else
                {
                    Shape.BeginAnimation(Canvas.LeftProperty,
                        new DoubleAnimation(current.ChartLocation.X - Shape.Width*.5, chart.View.AnimationsSpeed));
                    Shape.BeginAnimation(Canvas.TopProperty,
                        new DoubleAnimation(current.ChartLocation.Y - Shape.Height * .5, chart.View.AnimationsSpeed));
                }
            }

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();

                var xl = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualWidth*.5, chart);
                var yl = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualHeight*.5, chart);

                DataLabel.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(xl, chart.View.AnimationsSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(yl, chart.View.AnimationsSpeed));
            }
        }

        public override void Erase(ChartCore chart)
        {
            
        }

        protected double CorrectXLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + DataLabel.ActualWidth*.5 < -0.1) return -DataLabel.ActualWidth;

            if (desiredPosition + DataLabel.ActualWidth > chart.View.DrawMarginWidth)
                desiredPosition -= desiredPosition + DataLabel.ActualWidth - chart.View.DrawMarginWidth + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        protected double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            desiredPosition -= (Shape == null ? 0 : Shape.ActualHeight*.5) + DataLabel.ActualHeight*.5 + 2;

            if (desiredPosition + DataLabel.ActualHeight > chart.View.DrawMarginHeight)
                desiredPosition -= desiredPosition + DataLabel.ActualHeight - chart.View.DrawMarginHeight + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        public override void OnHover(ChartPoint point)
        {
            
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            
        }
    }
}
