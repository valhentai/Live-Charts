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
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;

namespace LiveCharts.Wpf.PointViews
{
    internal class ScatterPointView : PointView, IScatterPointView
    {
        public Shape Shape { get; set; }
        public double Diameter { get; set; }

        public override void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            //if (IsNew)
            {
                Canvas.SetTop(Shape, ChartPoint.ChartLocation.Y);
                Canvas.SetLeft(Shape, ChartPoint.ChartLocation.X);

                Shape.Width = 0;
                Shape.Height = 0;
            }

            if (Label != null && double.IsNaN(Canvas.GetLeft(Label)))
            {
                Canvas.SetTop(Label, ChartPoint.ChartLocation.Y);
                Canvas.SetLeft(Label, ChartPoint.ChartLocation.X);
            }

            if (chart.View.DisableAnimations)
            {
                Shape.Width = Diameter;
                Shape.Height = Diameter;

                Canvas.SetTop(Shape, ChartPoint.ChartLocation.Y - Shape.Height*.5);
                Canvas.SetLeft(Shape, ChartPoint.ChartLocation.X - Shape.Width*.5);

                if (Label != null)
                {
                    Label.UpdateLayout();

                    var cx = CorrectXLabel(ChartPoint.ChartLocation.X - Label.ActualWidth*.5, chart);
                    var cy = CorrectYLabel(ChartPoint.ChartLocation.Y - Label.ActualHeight*.5, chart);

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

            Shape.BeginAnimation(FrameworkElement.WidthProperty,
                new DoubleAnimation(Diameter, animSpeed));
            Shape.BeginAnimation(FrameworkElement.HeightProperty,
                new DoubleAnimation(Diameter, animSpeed));

            Shape.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(ChartPoint.ChartLocation.Y - Diameter*.5, animSpeed));
            Shape.BeginAnimation(Canvas.LeftProperty,
                new DoubleAnimation(ChartPoint.ChartLocation.X - Diameter*.5, animSpeed));
        }

        public override void Erase(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(Shape);
            chart.View.RemoveFromDrawMargin(Label);
        }

        protected double CorrectXLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + Label.ActualWidth > chart.View.DrawMarginWidth)
                desiredPosition -= desiredPosition + Label.ActualWidth - chart.View.DrawMarginWidth;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        protected double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + Label.ActualHeight > chart.View.DrawMarginHeight)
                desiredPosition -= desiredPosition + Label.ActualHeight - chart.View.DrawMarginHeight;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        public override void OnHover()
        {
            var copy = Shape.Fill.Clone();
            copy.Opacity -= .15;
            Shape.Fill = copy;
        }

        public override void OnHoverLeave()
        {
            if (Shape == null) return;

            if (ChartPoint.Fill != null)
            {
                Shape.Fill = (Brush) ChartPoint.Fill;
            }
            else
            {
                Shape.Fill = ((Series) ChartPoint.SeriesView).Fill;
            }
        }
    }
}
