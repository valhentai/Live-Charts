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

namespace LiveCharts.Definitions.Points
{
    /// <summary>
    /// Represents a financial point view.
    /// </summary>
    /// <seealso cref="ChartPointView" />
    public interface IFinancialPointView
    {
        /// <summary>
        /// Sets the Open value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The open.
        /// </value>
        double Open { set; }

        /// <summary>
        /// Sets the High value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        double High { set; }

        /// <summary>
        /// Sets the Low value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        double Low { set; }

        /// <summary>
        /// Sets the Close value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The close.
        /// </value>
        double Close { set; }

        /// <summary>
        /// Sets the Width of the point in the drawing area of the chart.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        double Width { set; }

        /// <summary>
        /// Sets the Open value in the chart, measured from the top of the drawing area in the chart.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        double Left { set; }

    }
}