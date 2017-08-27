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

using LiveCharts.Dtos;

namespace LiveCharts
{
    /// <summary>
    /// Defines a responsive rectangle area.
    /// </summary>
    public class ResponsiveRectangle : ResponsiveArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsiveRectangle"/> class.
        /// </summary>
        public ResponsiveRectangle()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsiveRectangle"/> class, with given dimensions.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        public ResponsiveRectangle(double left, double top, double height, double width)
        {
            Top = top;
            Left = left;
            Height = height;
            Width = width;
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public double Width { get; set; }

        /// <inheritdoc cref="ResponsiveArea.IsInside"/>
        public override bool IsInside(CorePoint point)
        {
            return point.X >= Left && point.X <= Left + Width &&
                   point.Y >= Top && point.Y <= Top + Height;
        }
    }
}