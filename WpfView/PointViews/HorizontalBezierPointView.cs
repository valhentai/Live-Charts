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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.Charts;
using LiveCharts.Definitions.Series;

namespace LiveCharts.Wpf.PointViews
{
    /// <summary>
    /// Horizontal Bezier point view, with a vertical animation.
    /// </summary>
    /// <seealso cref="BaseBezierPointView" />
    public class HorizontalBezierPointView : BaseBezierPointView
    {
        /// <inheritdoc cref="BaseBezierPointView.Draw"/>
        public override void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            // handles shapes, deletes, or initializes a new instance if necessary.
            base.Draw(previousDrawn, index, series, chart);

            var lineSeries = (LineSeries) series;
            var previousPv = previousDrawn?.View as HorizontalBezierPointView;

            var y = chart.View.DrawMarginTop + chart.View.DrawMarginHeight;

            // set default label position
            if (Label != null)
            {
                Canvas.SetTop(Label, y);
                Canvas.SetLeft(Label, ChartPoint.ChartLocation.X - Label.ActualWidth * .5);
            }

            #region No animated

            if (chart.View.DisableAnimations)
            {
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
                    Canvas.SetLeft(PointShapePath, ChartPoint.ChartLocation.X - PointShapePath.Width * .5);
                    Canvas.SetTop(PointShapePath, ChartPoint.ChartLocation.Y - PointShapePath.Height * .5);
                }

                if (Label != null)
                {
                    Label.UpdateLayout();
                    var xl = CorrectXLabel(ChartPoint.ChartLocation.X - Label.ActualWidth * .5, chart);
                    var yl = CorrectYLabel(ChartPoint.ChartLocation.Y - Label.ActualHeight * .5, chart);
                    Canvas.SetLeft(Label, xl);
                    Canvas.SetTop(Label, yl);
                }

                return;
            }

            #endregion

            #region Initial animations position

            if (Bezier == null)
            {
                // if this is a new point.
                Bezier = new BezierSegment();

                // update the segments in the path
                ShadowPath.Segments.Remove(Bezier);
                ShadowPath.Segments.Insert(index, Bezier);
                StrokePath.Segments.Remove(Bezier);
                StrokePath.Segments.Add(Bezier);

                if (previousPv?.Bezier != null)
                {
                    // and there exists a previous point (i.e. on insert)
                    // lets place the initial position of this point, at the same position of the previous point.

                    Bezier.Point1 = previousPv.Bezier.Point3;
                    Bezier.Point2 = previousPv.Bezier.Point3;
                    Bezier.Point3 = previousPv.Bezier.Point3;

                    if (PointShapePath != null)
                    {
                        Canvas.SetTop(PointShapePath, Canvas.GetTop(previousPv.PointShapePath));
                        Canvas.SetLeft(PointShapePath, Canvas.GetLeft(previousPv.PointShapePath));
                    }

                    if (Label != null)
                    {
                        Canvas.SetTop(Label, Canvas.GetTop(previousPv.Label));
                        Canvas.SetLeft(Label, Canvas.GetLeft(previousPv.Label));
                    }
                }
                else
                {
                    // and there is no a previous point
                    if (ChartPoint.Gci <= 1)
                    {
                        // on first draw
                        Bezier.Point1 = new Point(Data.Point1.X, y);
                        Bezier.Point2 = new Point(Data.Point2.X, y);
                        Bezier.Point3 = new Point(Data.Point3.X, y);

                        if (PointShapePath != null)
                        {
                            Canvas.SetTop(PointShapePath, y);
                            Canvas.SetLeft(PointShapePath, ChartPoint.ChartLocation.X - PointShapePath.Width * .5);
                        }

                        if (Label != null)
                        {
                            Canvas.SetTop(Label, y);
                            Canvas.SetLeft(Label, ChartPoint.ChartLocation.X - Label.ActualWidth * .5);
                        }
                    }
                    else
                    {
                        // could be on zooming/panning

                        var startPoint = lineSeries.PathCollection[0].Left.Point;
                        Bezier.Point1 = startPoint;
                        Bezier.Point2 = startPoint;
                        Bezier.Point3 = startPoint;

                        if (PointShapePath != null)
                        {
                            Canvas.SetTop(PointShapePath, y);
                            Canvas.SetLeft(PointShapePath, startPoint.X - PointShapePath.Width * .5);
                        }

                        if (Label != null)
                        {
                            Canvas.SetTop(Label, y);
                            Canvas.SetLeft(Label, startPoint.X - Label.ActualWidth * .5);
                        }
                    }
                }
            }

            #endregion

            // begin the animations...

            Bezier.BeginAnimation(BezierSegment.Point1Property,
                new PointAnimation(Bezier.Point1, Data.Point1.AsPoint(), chart.View.AnimationsSpeed));
            Bezier.BeginAnimation(BezierSegment.Point2Property,
                new PointAnimation(Bezier.Point2, Data.Point2.AsPoint(), chart.View.AnimationsSpeed));
            Bezier.BeginAnimation(BezierSegment.Point3Property,
                new PointAnimation(Bezier.Point3, Data.Point3.AsPoint(), chart.View.AnimationsSpeed));

            if (PointShapePath != null)
            {
                if (double.IsNaN(Canvas.GetLeft(PointShapePath)))
                {
                    Canvas.SetLeft(PointShapePath, ChartPoint.ChartLocation.X - PointShapePath.Width * .5);
                    Canvas.SetTop(PointShapePath, ChartPoint.ChartLocation.Y - PointShapePath.Height * .5);
                }
                else
                {
                    PointShapePath.BeginAnimation(Canvas.LeftProperty,
                        new DoubleAnimation(ChartPoint.ChartLocation.X - PointShapePath.Width * .5, chart.View.AnimationsSpeed));
                    PointShapePath.BeginAnimation(Canvas.TopProperty,
                        new DoubleAnimation(ChartPoint.ChartLocation.Y - PointShapePath.Height * .5, chart.View.AnimationsSpeed));
                }
            }

            if (Label != null)
            {
                Label.UpdateLayout();

                var xl = CorrectXLabel(ChartPoint.ChartLocation.X - Label.ActualWidth * .5, chart);
                var yl = CorrectYLabel(ChartPoint.ChartLocation.Y - Label.ActualHeight * .5, chart);

                Label.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(xl, chart.View.AnimationsSpeed));
                Label.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(yl, chart.View.AnimationsSpeed));
            }
        }
    }
}
