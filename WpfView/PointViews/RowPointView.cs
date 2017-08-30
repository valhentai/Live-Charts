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
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Data;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;

namespace LiveCharts.Wpf.PointViews
{
    internal class RowPointView : PointView, IRectanglePointView
    {
        public Rectangle Rectangle { get; set; }
        public RectangleData Data { get; set; }
        public double ZeroReference  { get; set; }
        public BarLabelPosition LabelPosition { get; set; }
        private RotateTransform Transform { get; set; }

        public override void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            //if (IsNew)
            {
                Canvas.SetTop(Rectangle, Data.Top);
                Canvas.SetLeft(Rectangle, ZeroReference);

                Rectangle.Width = 0;
                Rectangle.Height = Data.Height;
            }

            if (Label != null && double.IsNaN(Canvas.GetLeft(Label)))
            {
                Canvas.SetTop(Label, Data.Top);
                Canvas.SetLeft(Label, ZeroReference);
            }

            Func<double> getY = () =>
            {
                if (LabelPosition == BarLabelPosition.Perpendicular)
                {
                    if (Transform == null)
                        Transform = new RotateTransform(270);

                    Label.RenderTransform = Transform;
                    return Data.Top + Data.Height/2 + Label.ActualWidth*.5;
                }

                var r = Data.Top + Data.Height / 2 - Label.ActualHeight / 2;

                if (r < 0) r = 2;
                if (r + Label.ActualHeight > chart.View.DrawMarginHeight)
                    r -= r + Label.ActualHeight - chart.View.DrawMarginHeight + 2;

                return r;
            };

            Func<double> getX = () =>
            {
                double r;

#pragma warning disable 618
                if (LabelPosition == BarLabelPosition.Parallel || LabelPosition == BarLabelPosition.Merged)
#pragma warning restore 618
                {
                    r = Data.Left + Data.Width/2 - Label.ActualWidth/2;
                }
                else if (LabelPosition == BarLabelPosition.Perpendicular)
                {
                    r = Data.Left + Data.Width/2 - Label.ActualHeight/2;
                }
                else
                {
                    if (Data.Left < ZeroReference)
                    {
                        r = Data.Left - Label.ActualWidth - 5;
                        if (r < 0) r = Data.Left + 5;
                    }
                    else
                    {
                        r = Data.Left + Data.Width + 5;
                        if (r + Label.ActualWidth > chart.View.DrawMarginWidth)
                            r -= Label.ActualWidth + 10;
                    }
                }

                return r;
            };

            if (chart.View.DisableAnimations)
            {
                Rectangle.Width = Data.Width;
                Rectangle.Height = Data.Height;

                Canvas.SetTop(Rectangle, Data.Top);
                Canvas.SetLeft(Rectangle, Data.Left);

                if (Label != null)
                {
                    Label.UpdateLayout();

                    Canvas.SetTop(Label, getY());
                    Canvas.SetLeft(Label, getX());
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (Label != null)
            {
                Label.UpdateLayout();

                Label.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(getX(), animSpeed));
                Label.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(getY(), animSpeed));
            }

            Rectangle.BeginAnimation(Canvas.TopProperty, 
                new DoubleAnimation(Data.Top, animSpeed));
            Rectangle.BeginAnimation(Canvas.LeftProperty,
                new DoubleAnimation(Data.Left, animSpeed));

            Rectangle.BeginAnimation(FrameworkElement.HeightProperty, 
                new DoubleAnimation(Data.Height, animSpeed));
            Rectangle.BeginAnimation(FrameworkElement.WidthProperty,
                new DoubleAnimation(Data.Width, animSpeed));
        }

        public override void Erase(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(Rectangle);
            chart.View.RemoveFromDrawMargin(Label);
        }

        public override void OnHover()
        {
            var copy = Rectangle.Fill.Clone();
            copy.Opacity -= .15;
            Rectangle.Fill = copy;
        }

        public override void OnHoverLeave()
        {
            if (Rectangle == null) return;

            if (ChartPoint.Fill != null)
            {
                Rectangle.Fill = (Brush)ChartPoint.Fill;
            }
            else
            {
                Rectangle.Fill = ((Series) ChartPoint.SeriesView).Fill;
            }
        }
    }
}
