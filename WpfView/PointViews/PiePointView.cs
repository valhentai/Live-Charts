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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf.PointViews
{
    internal class PiePointView : PointView, IPieSlicePointView
    {
        public double Rotation { get; set; }
        public double InnerRadius { get; set; }
        public double Radius { get; set; }
        public double Wedge { get; set; }
        public PieSlice Slice { get; set; }
        public double OriginalPushOut { get; set; }

        public override void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            //if (IsNew)
            {
                Canvas.SetTop(Slice, chart.View.DrawMarginHeight/2);
                Canvas.SetLeft(Slice, chart.View.DrawMarginWidth/2);

                Slice.WedgeAngle = 0;
                Slice.RotationAngle = 0;
            }

            if (Label != null && double.IsNaN(Canvas.GetLeft(Label)))
            {
                Canvas.SetTop(Label, chart.View.DrawMarginHeight/2);
                Canvas.SetLeft(Label, chart.View.DrawMarginWidth/2);
            }

            var lh = 0d;
            if (Label != null)
            {
                Label.UpdateLayout();
                lh = Label.ActualHeight;
            }

            var hypo = ((PieSeries) ChartPoint.SeriesView).LabelPosition == PieLabelPosition.InsideSlice
                ? (Radius + InnerRadius)*(Math.Abs(InnerRadius) < 0.01 ? .65 : .5)
                : Radius+lh;
            var gamma = ChartPoint.Participation*360/2 + Rotation;
            var cp = new Point(hypo * Math.Sin(gamma * (Math.PI / 180)), hypo * Math.Cos(gamma * (Math.PI / 180)));

            if (chart.View.DisableAnimations)
            {
                Slice.InnerRadius = InnerRadius;
                Slice.Radius = Radius;
                Slice.WedgeAngle = Wedge;
                Slice.RotationAngle = Rotation;
                Canvas.SetTop(Slice, chart.View.DrawMarginHeight / 2);
                Canvas.SetLeft(Slice, chart.View.DrawMarginWidth / 2);

                if (Label != null)
                {
                    var lx = cp.X + chart.View.DrawMarginWidth/2 - Label.ActualWidth * .5;
                    var ly = chart.View.DrawMarginHeight/2 - cp.Y - Label.ActualHeight*.5;

                    Canvas.SetLeft(Label, lx);
                    Canvas.SetTop(Label, ly);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (Label != null)
            {
                Label.UpdateLayout();

                var lx = cp.X + chart.View.DrawMarginWidth/2 - Label.ActualWidth * .5;
                var ly = chart.View.DrawMarginHeight/2 - cp.Y - Label.ActualHeight * .5;

                Label.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(lx, animSpeed));
                Label.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(ly, animSpeed));
            }

            Slice.BeginAnimation(Canvas.LeftProperty, 
                new DoubleAnimation(chart.View.DrawMarginWidth / 2, animSpeed));
            Slice.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(chart.View.DrawMarginHeight/2, animSpeed));
            Slice.BeginAnimation(PieSlice.InnerRadiusProperty, new DoubleAnimation(InnerRadius, animSpeed));
            Slice.BeginAnimation(PieSlice.RadiusProperty, new DoubleAnimation(Radius, animSpeed));
            Slice.BeginAnimation(PieSlice.WedgeAngleProperty, new DoubleAnimation(Wedge, animSpeed));
            Slice.BeginAnimation(PieSlice.RotationAngleProperty, new DoubleAnimation(Rotation, animSpeed));
        }

        public override void Erase(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(Slice);
            chart.View.RemoveFromDrawMargin(Label);
        }

        public override void OnHover()
        {
            var copy = Slice.Fill.Clone();
            copy.Opacity -= .15;
            Slice.Fill = copy;

            var pieChart = (PieChart) ChartPoint.SeriesView.Core.Chart.View;

            Slice.BeginAnimation(PieSlice.PushOutProperty,
                new DoubleAnimation(Slice.PushOut, OriginalPushOut + pieChart.HoverPushOut,
                    ChartPoint.SeriesView.Core.Chart.View.AnimationsSpeed));
        }

        public override void OnHoverLeave()
        {
            if (ChartPoint.Fill != null)
            {
                Slice.Fill = (Brush)ChartPoint.Fill;
            }
            else
            {
                Slice.Fill = ((Series) ChartPoint.SeriesView).Fill;
            }
            Slice.BeginAnimation(PieSlice.PushOutProperty,
                new DoubleAnimation(OriginalPushOut, ChartPoint.SeriesView.Core.Chart.View.AnimationsSpeed));
        }
    }
}
