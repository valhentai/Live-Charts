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

using System.Windows.Controls;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;

namespace LiveCharts.Wpf.PointViews
{
    /// <inheritdoc cref="ChartPointView"/>
    /// <seealso cref="ChartPointView" />
    public class PointView : ChartPointView
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public ContentControl Label { get; protected set; }

        /// <inheritdoc cref="ChartPointView.Draw"/>
        public override void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="ChartPointView.Erase"/>
        public override void Erase(ChartCore chart)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="ChartPointView.OnHover"/>
        public override void OnHover()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="ChartPointView.OnHoverLeave"/>
        public override void OnHoverLeave()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="ChartPointView.OnSelection"/>
        public override void OnSelection()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="ChartPointView.OnSelectionLeave"/>
        public override void OnSelectionLeave()
        {
            throw new System.NotImplementedException();
        }
    }
}