//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez Orozco & LiveCharts Contributors

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
using System.Windows.Shapes;
using LiveCharts.Dtos;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class LineSeriesPathHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeriesPathHelper"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="startsY">The starts y.</param>
        public LineSeriesPathHelper(CorePoint location, double startsY)
        {
            var p = new Point(location.X, startsY);
            Bottom = new LineSegment {Point = p};
            Left = new LineSegment {Point = p};
            Right = new LineSegment {Point = p};
        }

        //ToDo: Delete!
        public bool IsNew { get; set; }

        /// <summary>
        /// Gets or sets the line path.
        /// </summary>
        /// <value>
        /// The line path.
        /// </value>
        public Path LinePath { get; set; }

        /// <summary>
        /// Gets or sets the shadow path.
        /// </summary>
        /// <value>
        /// The shadow path.
        /// </value>
        public Path ShadowPath { get; set; }

        /// <summary>
        /// Gets or sets the line figure.
        /// </summary>
        /// <value>
        /// The line figure.
        /// </value>
        public PathFigure LineFigure { get; set; }

        /// <summary>
        /// Gets or sets the shadow figure.
        /// </summary>
        /// <value>
        /// The shadow figure.
        /// </value>
        public PathFigure ShadowFigure { get; set; }

        /// <summary>
        /// Gets the bottom.
        /// </summary>
        /// <value>
        /// The bottom.
        /// </value>
        public LineSegment Bottom { get; private set; }

        /// <summary>
        /// Gets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public LineSegment Left { get; private set; }

        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <value>
        /// The right.
        /// </value>
        public LineSegment Right { get; private set; }

        /// <summary>
        /// Gets or sets the index of the splitter collector.
        /// </summary>
        /// <value>
        /// The index of the splitter collector.
        /// </value>
        public int SplitterCollectorIndex { get; set; }
    }
}
