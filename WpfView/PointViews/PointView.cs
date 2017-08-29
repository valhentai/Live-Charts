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
    /// <summary>
    /// The Windows Presentation Foundation base point view.
    /// </summary>
    /// <seealso cref="IChartPointView" />
    public abstract class PointView : IChartPointView
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public ContentControl Label { get; set; }

        /// <inheritdoc cref="IChartPointView.ChartPoint"/>
        public ChartPoint ChartPoint { get; internal set; }

        /// <inheritdoc cref="IChartPointView.Draw"/>
        public virtual void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IChartPointView.Erase"/>
        public virtual void Erase(ChartCore chart)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IChartPointView.OnHover"/>
        public virtual void OnHover()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IChartPointView.OnHoverLeave"/>
        public virtual void OnHoverLeave()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IChartPointView.OnSelection"/>
        public virtual void OnSelection()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="IChartPointView.OnSelectionLeave"/>
        public void OnSelectionLeave()
        {
            throw new System.NotImplementedException();
        }
    }
}