//copyright(c) 2016 Alberto Rodriguez

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
using System.Windows.Media;

namespace LiveCharts.Wpf.Shapes
{
    /// <summary>
    /// Defines a candle shape.
    /// </summary>
    /// <seealso cref="System.Windows.Shapes.Shape" />
    public class CandleShape : FinancialShape
    {
        /// <inheritdoc cref="FinancialShape.DrawGeometry"/>
        protected override void DrawGeometry(StreamGeometryContext context)
        {
            var middle = ActualWidth / 2;

            double i, j;

            if (Open < Close)
            {
                i = Open;
                j = Close;
            }
            else
            {
                i = Close;
                j = Open;
            }

            context.BeginFigure(new Point(middle, 0), true, true);
            context.LineTo(new Point(0, i), true, true);
            context.LineTo(new Point(-middle, 0), true, true);
            context.LineTo(new Point(0, j - i), true, true);
            context.LineTo(new Point(middle, 0), true, true);
            context.LineTo(new Point(0, ActualHeight-j), true, true);
            context.LineTo(new Point(0, -(ActualHeight - j)), true, true);
            context.LineTo(new Point(middle, 0), true, true);
            context.LineTo(new Point(0, -(j - i)), true, true);
            context.LineTo(new Point(-middle, 0), true, true);
            context.Close();
        }
    }
}
