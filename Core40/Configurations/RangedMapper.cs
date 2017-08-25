
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

using System;

namespace LiveCharts.Configurations
{
    /// <summary>
    /// Mapper to configure X and Y points
    /// </summary>
    /// <typeparam name="T">Type to configure</typeparam>
    public class RangedMapper<T> : CartesianMapper<T>
    {
        private Func<T, int, double> _startX = (v, i) => 0;
        private Func<T, int, double> _startY = (v, i) => 0;

        /// <summary>
        /// Sets values for a specific point
        /// </summary>
        /// <param name="point">Point to set</param>
        /// <param name="value"></param>
        /// <param name="key"></param>
        public override void Evaluate(int key, T value, ChartPoint point)
        {
            base.Evaluate(key, value, point);

            point.XStart = _startX(value, key);
            point.YStart = _startY(value, key);
            point.XEnd = point.X;
            point.YEnd = point.Y;
        }

        /// <summary>
        /// Sets the X coordinate mapper.
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> X(Func<T, double> predicate)
        {
            return (RangedMapper<T>)SetFirstDimension((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the X coordinate mapper.
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> X(Func<T, int, double> predicate)
        {
            return (RangedMapper<T>)SetFirstDimension(predicate);
        }

        /// <summary>
        /// Sets the Y coordinate mapper.
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> Y(Func<T, double> predicate)
        {
            return (RangedMapper<T>)SetSecondDimension((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Y coordinate mapper.
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> Y(Func<T, int, double> predicate)
        {
            return (RangedMapper<T>)SetSecondDimension(predicate);
        }

        /// <summary>
        /// Sets the XStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> XStart(Func<T, double> predicate)
        {
            return XStart((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the XStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> XStart(Func<T, int, double> predicate)
        {
            _startX = predicate;
            return this;
        }

        /// <summary>
        /// Sets the XStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> XEnd(Func<T, int, double> predicate)
        {
            _startX = predicate;
            return this;
        }

        /// <summary>
        /// Sets the XStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> XEnd(Func<T, double> predicate)
        {
            return (RangedMapper<T>)SetFirstDimension((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the YStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> YStart(Func<T, double> predicate)
        {
            return YStart((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the YStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> YStart(Func<T, int, double> predicate)
        {
            _startY = predicate;
            return this;
        }

        /// <summary>
        /// Sets the YStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> YEnd(Func<T, double> predicate)
        {
            return YStart((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the YStart mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public RangedMapper<T> YEnd(Func<T, int, double> predicate)
        {
            return (RangedMapper<T>) SetSecondDimension(predicate);
        }
    }
}