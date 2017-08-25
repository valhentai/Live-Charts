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

using LiveCharts.Dtos;

namespace LiveCharts
{
    /// <summary>
    /// Defines a responsive polar area.
    /// </summary>
    /// <seealso cref="LiveCharts.ResponsiveArea" />
    public class ResponsivePolarArea : ResponsiveArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsivePolarArea"/> class.
        /// </summary>
        public ResponsivePolarArea()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsivePolarArea"/> class, with given dimensions.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="alphaI">The alpha i.</param>
        /// <param name="alphaJ">The alpha j.</param>
        /// <param name="center">The center.</param>
        public ResponsivePolarArea(double radius, double alphaI, double alphaJ, CorePoint center)
        {
            Radius = radius;
            AlphaI = alphaI;
            AlphaJ = alphaJ;
            Center = center;
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double Radius { get; set; }

        /// <summary>
        /// Gets or sets the alpha i.
        /// </summary>
        /// <value>
        /// The alpha i.
        /// </value>
        public double AlphaI { get; set; }

        /// <summary>
        /// Gets or sets the alpha j.
        /// </summary>
        /// <value>
        /// The alpha j.
        /// </value>
        public double AlphaJ { get; set; }

        /// <summary>
        /// Gets or sets the center.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public CorePoint Center { get; set; }

        /// <inheritdoc cref="ResponsiveArea.IsInside"/>
        public override bool IsInside(CorePoint point)
        {
            throw new System.NotImplementedException();
        }
    }
}