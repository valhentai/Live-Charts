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

using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;

namespace LiveCharts.Wpf.PointViews
{
    internal class OhlcPointView : PointView, IOhlcPointView
    {
        public Line HighToLowLine { get; set; }
        public Line OpenLine { get; set; }
        public Line CloseLine { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Close { get; set; }
        public double Low { get; set; }
        public double Width { get; set; }
        public double Left { get; set; }
        public double StartReference { get; set; }

        public override void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            var center = Left + Width / 2;

            //if (IsNew)
            {
                HighToLowLine.X1 = center;
                HighToLowLine.X2 = center;
                HighToLowLine.Y1 = StartReference;
                HighToLowLine.Y2 = StartReference;

                OpenLine.X1 = Left;
                OpenLine.X2 = center;
                OpenLine.Y1 = StartReference;
                OpenLine.Y2 = StartReference;

                CloseLine.X1 = center;
                CloseLine.X2 = Left + Width;
                CloseLine.Y1 = StartReference;
                CloseLine.Y2 = StartReference;

                if (Label != null)
                {
                    Canvas.SetTop(Label, ChartPoint.ChartLocation.Y);
                    Canvas.SetLeft(Label, ChartPoint.ChartLocation.X);
                }
            }

            if (Label != null && double.IsNaN(Canvas.GetLeft(Label)))
            {
                Canvas.SetTop(Label, ChartPoint.ChartLocation.Y);
                Canvas.SetLeft(Label, ChartPoint.ChartLocation.X);
            }

            if (chart.View.DisableAnimations)
            {
                HighToLowLine.Y1 = High;
                HighToLowLine.Y2 = Low;
                HighToLowLine.X1 = center;
                HighToLowLine.X2 = center;

                OpenLine.Y1 = Open;
                OpenLine.Y2 = Open;
                OpenLine.X1 = Left;
                OpenLine.X2 = center;

                CloseLine.Y1 = Close;
                CloseLine.Y2 = Close;
                CloseLine.X1 = center;
                CloseLine.X2 = Left;

                if (Label != null)
                {
                    Label.UpdateLayout();

                    var cx = CorrectXLabel(ChartPoint.ChartLocation.X - Label.ActualHeight*.5, chart);
                    var cy = CorrectYLabel(ChartPoint.ChartLocation.Y - Label.ActualWidth*.5, chart);
                    
                    Canvas.SetTop(Label, cy);
                    Canvas.SetLeft(Label, cx);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (Label != null)
            {
                Label.UpdateLayout();

                var cx = CorrectXLabel(ChartPoint.ChartLocation.X - Label.ActualWidth*.5, chart);
                var cy = CorrectYLabel(ChartPoint.ChartLocation.Y - Label.ActualHeight*.5, chart);

                Label.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(cx, animSpeed));
                Label.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(cy, animSpeed));
            }

            HighToLowLine.BeginAnimation(Line.X1Property, new DoubleAnimation(center, animSpeed));
            HighToLowLine.BeginAnimation(Line.X2Property, new DoubleAnimation(center, animSpeed));
            OpenLine.BeginAnimation(Line.X1Property, new DoubleAnimation(Left, animSpeed));
            OpenLine.BeginAnimation(Line.X2Property, new DoubleAnimation(center, animSpeed));
            CloseLine.BeginAnimation(Line.X1Property, new DoubleAnimation(center, animSpeed));
            CloseLine.BeginAnimation(Line.X2Property, new DoubleAnimation(Left + Width, animSpeed));

            HighToLowLine.BeginAnimation(Line.Y1Property, new DoubleAnimation(High, animSpeed));
            HighToLowLine.BeginAnimation(Line.Y2Property, new DoubleAnimation(Low, animSpeed));
            OpenLine.BeginAnimation(Line.Y1Property, new DoubleAnimation(Open, animSpeed));
            OpenLine.BeginAnimation(Line.Y2Property, new DoubleAnimation(Open, animSpeed));
            CloseLine.BeginAnimation(Line.Y1Property, new DoubleAnimation(Close, animSpeed));
            CloseLine.BeginAnimation(Line.Y2Property, new DoubleAnimation(Close, animSpeed));
        }

        public override void Erase(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(OpenLine);
            chart.View.RemoveFromDrawMargin(CloseLine);
            chart.View.RemoveFromDrawMargin(HighToLowLine);
            chart.View.RemoveFromDrawMargin(Label);
        }

        protected double CorrectXLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + Label.ActualWidth * .5 < -0.1) return -Label.ActualWidth;

            if (desiredPosition + Label.ActualWidth > chart.View.DrawMarginWidth)
                desiredPosition -= desiredPosition + Label.ActualWidth - chart.View.DrawMarginWidth + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        protected double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + Label.ActualHeight > chart.View.DrawMarginHeight)
                desiredPosition -= desiredPosition + Label.ActualHeight - chart.View.DrawMarginHeight + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }
        
    }
}
