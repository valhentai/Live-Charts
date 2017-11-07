﻿//The MIT License(MIT)

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

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Dtos;

namespace LiveCharts.Wpf.Points
{
    internal class ColumnPointView : PointView, IRectanglePointView
    {
        public Rectangle Rectangle { get; set; }
        public CoreRectangle Data { get; set; }
        public double ZeroReference  { get; set; }
        public BarLabelPosition LabelPosition { get; set; }
        private RotateTransform Transform { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
           
            Func<double> getY = () =>
            {
                double y;

#pragma warning disable 618
                if (LabelPosition == BarLabelPosition.Parallel || LabelPosition == BarLabelPosition.Merged)
#pragma warning restore 618
                {
                    if (Transform == null)
                        Transform = new RotateTransform(270);

                    y = Data.Top + Data.Height / 2d + DataLabel.ActualWidth * .5;
                    DataLabel.RenderTransform = Transform;
                }
                else if (LabelPosition == BarLabelPosition.Perpendicular)
                {
                    y = Data.Top + Data.Height / 2d - DataLabel.ActualHeight * .5;
                }
                else
                {
                    if (ZeroReference > Data.Top)
                    {
                        y = Data.Top - DataLabel.ActualHeight;
                        if (y < 0) y = Data.Top;
                    }
                    else
                    {
                        y = Data.Top + Data.Height;
                        if (y + DataLabel.ActualHeight > chart.DrawMargin.Height) y -= DataLabel.ActualHeight;
                    }
                }

                return y;
            };

            Func<double> getX = () =>
            {
                double x;

#pragma warning disable 618
                if (LabelPosition == BarLabelPosition.Parallel || LabelPosition == BarLabelPosition.Merged)
#pragma warning restore 618
                {
                    x = Data.Left + Data.Width / 2d - DataLabel.ActualHeight / 2d;
                }
                else if (LabelPosition == BarLabelPosition.Perpendicular)
                {
                    x = Data.Left + Data.Width / 2d - DataLabel.ActualWidth / 2d;
                }
                else
                {
                    x = Data.Left + Data.Width / 2d - DataLabel.ActualWidth / 2d;
                    if (x < 0)
                        x = 2;
                    if (x + DataLabel.ActualWidth > chart.DrawMargin.Width)
                        x -= x + DataLabel.ActualWidth - chart.DrawMargin.Width + 2d;
                }

                return x;
            };

            if (IsNew)
            {
                Canvas.SetTop(Rectangle, ZeroReference);
                Canvas.SetLeft(Rectangle, Data.Left);

                Rectangle.Width = Data.Width;
                Rectangle.Height = 0;
                
            }

            if (DataLabel != null && double.IsNaN(Canvas.GetLeft(DataLabel)))
            {
                Canvas.SetTop(DataLabel, ZeroReference);
                Canvas.SetLeft(DataLabel, current.ChartLocation.X);
            }


            var dirty = ChartPoint.DirtyFlag.None;
            if (Canvas.GetLeft(Rectangle) != Data.Left)
                dirty = dirty | ChartPoint.DirtyFlag.X;
            if (Canvas.GetTop(Rectangle) != Data.Top)
                dirty = dirty | ChartPoint.DirtyFlag.Y;

            if (dirty == ChartPoint.DirtyFlag.None && Rectangle.Width == Data.Width && Rectangle.Height == Data.Height)
                return;

            
            if (chart.View.DisableAnimations)
            {
                Rectangle.Width = Data.Width;
                Rectangle.Height = Data.Height;

                Canvas.SetTop(Rectangle, Data.Top);
                Canvas.SetLeft(Rectangle, Data.Left);

                if (DataLabel != null)
                {
                    if (IsNew || dirty != ChartPoint.DirtyFlag.None ||Rectangle.Width != Data.Width || Rectangle.Height != Data.Height)
                        DataLabel.UpdateLayout();

                    if (IsNew || dirty.HasFlag(ChartPoint.DirtyFlag.X))
                        Canvas.SetTop(DataLabel, getY());
                    if (IsNew || dirty.HasFlag(ChartPoint.DirtyFlag.Y))
                        Canvas.SetLeft(DataLabel, getX());
                }

                if (HoverShape != null)
                {
                    Canvas.SetTop(HoverShape, Data.Top);
                    Canvas.SetLeft(HoverShape, Data.Left);
                    HoverShape.Height = Data.Height;
                    HoverShape.Width = Data.Width;
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (DataLabel != null)
            {
                if(IsNew || dirty != ChartPoint.DirtyFlag.None)
                    DataLabel.UpdateLayout();

                if (IsNew || dirty.HasFlag(ChartPoint.DirtyFlag.X) || Rectangle.Width != Data.Width)
                    DataLabel.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(getX(), animSpeed));
                if (IsNew || dirty.HasFlag(ChartPoint.DirtyFlag.Y) || Rectangle.Height != Data.Height)
                    DataLabel.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(getY(), animSpeed));
            }


            if(dirty.HasFlag(ChartPoint.DirtyFlag.X))
                Rectangle.BeginAnimation(Canvas.LeftProperty, 
                new DoubleAnimation(Data.Left, animSpeed));
            if (dirty.HasFlag(ChartPoint.DirtyFlag.Y))
                Rectangle.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(Data.Top, animSpeed));

            if (dirty.HasFlag(ChartPoint.DirtyFlag.X) || Rectangle.Width!= Data.Width)
                Rectangle.BeginAnimation(FrameworkElement.WidthProperty,
                new DoubleAnimation(Data.Width, animSpeed));
            if (dirty.HasFlag(ChartPoint.DirtyFlag.Y) || Rectangle.Height != Data.Height)
                Rectangle.BeginAnimation(FrameworkElement.HeightProperty,
                new DoubleAnimation(Data.Height, animSpeed));

            if (HoverShape != null)
            {
                Canvas.SetTop(HoverShape, Data.Top);
                Canvas.SetLeft(HoverShape, Data.Left);
                HoverShape.Height = Data.Height;
                HoverShape.Width = Data.Width;
            }
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Rectangle);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }

        public override void OnHover(ChartPoint point)
        {
            var copy = Rectangle.Fill.Clone();
            copy.Opacity -= .15;
            Rectangle.Fill = copy;
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            if (Rectangle == null) return;

            if (point.Fill != null)
            {
                Rectangle.Fill = (Brush) point.Fill;
            }
            else
            {
                Rectangle.Fill = ((Series) point.SeriesView).Fill;
            }
        }
    }
}
