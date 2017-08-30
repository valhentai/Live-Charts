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

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf.Shapes;

namespace LiveCharts.Wpf.PointViews
{
    /// <summary>
    /// Candle point view class.
    /// </summary>
    /// <seealso cref="LiveCharts.Wpf.PointViews.PointView" />
    /// <seealso cref="LiveCharts.Definitions.Points.IFinancialPointView" />
    public class OhclPointView : PointView, IFinancialPointView
    {
        /// <summary>
        /// Sets the Open value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The open.
        /// </value>
        public double Open { get; protected set; }

        /// <summary>
        /// Gets or sets the High value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        public double High { get; protected set; }

        /// <summary>
        /// Gets or sets the Low value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        public double Low { get; protected set; }

        /// <summary>
        /// Gets or sets the Close value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The close.
        /// </value>
        public double Close { get; protected set; }

        /// <summary>
        /// Gets or sets the Width of the point in the drawing area of the chart.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public double Width { get; protected set; }

        /// <summary>
        /// Gets or sets the Open value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public double Left { get; protected set; }

        /// <summary>
        /// Gets or sets the candle element view.
        /// </summary>
        /// <value>
        /// The candle.
        /// </value>
        public OhlcShape OhclVisualShape { get; protected set; }

        /// <inheritdoc cref="PointView.Draw"/>
        public override void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            var candleSeries = (CandleSeries)series;

            // map the series properties to the drawn point.
            OhclVisualShape.Stroke = candleSeries.Stroke;
            OhclVisualShape.StrokeThickness = candleSeries.StrokeThickness;
            OhclVisualShape.Visibility = candleSeries.Visibility;
            Panel.SetZIndex(OhclVisualShape, Panel.GetZIndex(candleSeries));

            // initialize or update the label.
            if (candleSeries.DataLabels)
            {
                Label = candleSeries.UpdateLabelContent(
                    new DataLabelViewModel
                    {
                        FormattedText = DesignerProperties.GetIsInDesignMode(candleSeries)
                            ? "'label'"
                            : candleSeries.LabelPoint(ChartPoint),
                        Point = ChartPoint
                    }, Label);
            }

            // erase data label if it is not required anymore.
            if (!candleSeries.DataLabels && Label != null)
            {
                // notice UpdateLabelContent() added the label to the UI, we need to remove it.
                chart.View.RemoveFromDrawMargin(Label);
                Label = null;
            }


            // register the area where the point interacts with the user (hover and click).
            ChartPoint.ResponsiveArea =
                new ResponsiveRectangle(
                    High, Left,
                    Width, Math.Abs(Low - High));

            // a special case for financial series
            // allows to customize the colors using ColoringRules property
            if (candleSeries.ColoringRules == null)
            {
                if (ChartPoint.Open <= ChartPoint.Close)
                {
                    OhclVisualShape.Stroke = candleSeries.IncreaseBrush;
                    OhclVisualShape.Fill = candleSeries.IncreaseBrush;
                }
                else
                {
                    OhclVisualShape.Stroke = candleSeries.DecreaseBrush;
                    OhclVisualShape.Fill = candleSeries.DecreaseBrush;
                }
            }
            else
            {
                foreach (var rule in candleSeries.ColoringRules)
                {
                    if (!rule.Condition(ChartPoint, previousDrawn)) continue;

                    OhclVisualShape.Stroke = rule.Stroke;
                    OhclVisualShape.Fill = rule.Fill;

                    break;
                }
            }

            var height = Low - High;

            // not animated draw.
            if (chart.View.DisableAnimations)
            {
                if (OhclVisualShape == null)
                {
                    OhclVisualShape = new OhlcShape();
                    chart.View.AddToDrawMargin(OhclVisualShape);
                }

                OhclVisualShape.Width = Width;
                OhclVisualShape.Height = height;
                OhclVisualShape.Open = Open;
                OhclVisualShape.Close = Close;
                Canvas.SetTop(OhclVisualShape, High);
                Canvas.SetLeft(OhclVisualShape, Left);

                if (Label != null)
                {
                    Label.UpdateLayout();

                    var cx = CorrectXLabel(ChartPoint.ChartLocation.X - Label.ActualWidth * .5, chart);
                    var cy = CorrectYLabel(ChartPoint.ChartLocation.Y - Label.ActualHeight * .5, chart);

                    Canvas.SetTop(Label, cy);
                    Canvas.SetLeft(Label, cx);
                }

                return;
            }

            // running animations...
            var animSpeed = chart.View.AnimationsSpeed;

            var middlePoint = (High + Low) / 2;

            if (OhclVisualShape == null)
            {
                OhclVisualShape = new OhlcShape
                {
                    Width = Width,
                    Height = 0,
                    Open = middlePoint,
                    Close = middlePoint
                };
                chart.View.AddToDrawMargin(OhclVisualShape);
            }

            OhclVisualShape.Width = Width;
            OhclVisualShape.BeginAnimation(FrameworkElement.HeightProperty, new DoubleAnimation(height, animSpeed));
            OhclVisualShape.BeginAnimation(FinancialShape.OpenProperty, new DoubleAnimation(Open - High, animSpeed));
            OhclVisualShape.BeginAnimation(FinancialShape.CloseProperty, new DoubleAnimation(Low - Close, animSpeed));

            OhclVisualShape.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(Left, animSpeed));
            OhclVisualShape.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(High, animSpeed));

            if (Label != null)
            {
                var cx = CorrectXLabel(ChartPoint.ChartLocation.X - Label.ActualWidth * .5, chart);
                var cy = CorrectYLabel(ChartPoint.ChartLocation.Y - Label.ActualHeight * .5, chart);

                Label.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(cx, animSpeed));
                Label.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(cy, animSpeed));
            }
        }

        /// <inheritdoc cref="PointView.Erase"/>
        public override void Erase(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(OhclVisualShape);
            chart.View.RemoveFromDrawMargin(Label);
        }

        /// <inheritdoc cref="PointView.OnHover"/>
        public override void OnHover()
        {
            OhclVisualShape.Opacity = .8;
        }

        /// <inheritdoc cref="PointView.OnHoverLeave"/>
        public override void OnHoverLeave()
        {
            OhclVisualShape.Opacity = 1;
        }

        /// <inheritdoc cref="PointView.OnSelection"/>
        public override void OnSelection()
        {
        }

        /// <inheritdoc cref="PointView.OnSelectionLeave"/>
        public override void OnSelectionLeave()
        {
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
            //desiredPosition -= Ellipse.ActualHeight * .5 + DataLabel.ActualHeight * .5 + 2;

            if (desiredPosition + Label.ActualHeight > chart.View.DrawMarginHeight)
                desiredPosition -= desiredPosition + Label.ActualHeight - chart.View.DrawMarginHeight + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        #region IFinancialPointViewImplementation

        double IFinancialPointView.Open { set { Open = value; } }
        double IFinancialPointView.High { set { High = value; } }
        double IFinancialPointView.Low { set { Low = value; } }
        double IFinancialPointView.Close { set { Close = value; } }
        double IFinancialPointView.Left { set { Left = value; } }
        double IFinancialPointView.Width { set { Width = value; } }

        #endregion

    }
}
