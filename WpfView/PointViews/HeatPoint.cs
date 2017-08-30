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
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts.Wpf.PointViews
{
    /// <summary>
    /// Defines a heat point.
    /// </summary>
    /// <seealso cref="PointView" />
    /// <seealso cref="LiveCharts.Definitions.Points.IHeatPointView" />
    internal class HeatPoint : PointView, IHeatPointView
    {
        /// <summary>
        /// Gets or sets the rectangle.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// Gets or sets the color components.
        /// </summary>
        /// <value>
        /// The color components.
        /// </value>
        public CoreColor ColorComponents { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public double Height { get; set; }

        /// <inheritdoc cref=""/>
        public override void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            Canvas.SetTop(Rectangle, ChartPoint.ChartLocation.Y);
            Canvas.SetLeft(Rectangle, ChartPoint.ChartLocation.X);

            Rectangle.Width = Width;
            Rectangle.Height = Height;

            //if (IsNew)
            {
                Rectangle.Fill = new SolidColorBrush(Colors.Transparent);
            }

            if (Label != null)
            {
                Label.UpdateLayout();
                Canvas.SetTop(Label, ChartPoint.ChartLocation.Y + (Height/2) - Label.ActualHeight*.5);
                Canvas.SetLeft(Label, ChartPoint.ChartLocation.X + (Width/2) - Label.ActualWidth*.5);
            }

            var targetColor = new Color
            {
                A = ColorComponents.A,
                R = ColorComponents.R,
                G = ColorComponents.G,
                B = ColorComponents.B
            };

            if (chart.View.DisableAnimations)
            {
                Rectangle.Fill = new SolidColorBrush(targetColor);
                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            Rectangle.Fill.BeginAnimation(SolidColorBrush.ColorProperty,
                new ColorAnimation(targetColor, animSpeed));
        }

        public override void Erase(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(Rectangle);
            chart.View.RemoveFromDrawMargin(Label);
        }

        public override void OnHover()
        {
            Rectangle.StrokeThickness++;
        }

        public override void OnHoverLeave()
        {
            Rectangle.StrokeThickness = ((Series) ChartPoint.SeriesView).StrokeThickness;
        }

        public override void OnSelection()
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelectionLeave()
        {
            throw new System.NotImplementedException();
        }
    }
}
