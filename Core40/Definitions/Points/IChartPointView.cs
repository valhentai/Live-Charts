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

using LiveCharts.Charts;
using LiveCharts.Definitions.Series;

namespace LiveCharts.Definitions.Points
{
    /// <summary>
    ///
    /// </summary>
    public interface IChartPointView
    {
        /// <summary>
        /// Gets the ChartPoint that owns the view.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        ChartPoint ChartPoint { get; }

        /// <summary>
        /// Draws the point.
        /// </summary>
        /// <param name="previousDrawn">The previous drawn.</param>
        /// <param name="index">The index.</param>
        /// <param name="series"></param>
        /// <param name="chart">The chart.</param>
        void Draw(ChartPoint previousDrawn, int index, ISeriesView series, ChartCore chart);

        /// <summary>
        /// Erases the point from the view.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void Erase(ChartCore chart);

        /// <summary>
        /// Called when the point is hovered.
        /// </summary>
        void OnHover();

        /// <summary>
        /// Called when the point hover leaves.
        /// </summary>
        void OnHoverLeave();

        /// <summary>
        /// Called when the point is selected.
        /// </summary>
        void OnSelection();

        /// <summary>
        /// Called when the point selection state changes from true to false.
        /// </summary>
        void OnSelectionLeave();
    }

}
