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
    /// Mapper to configure Bubble points
    /// </summary>
    /// <typeparam name="T">type to configure</typeparam>
    public class WeightedMapper<T> : BiDimensionalMapper<T>
    {
        private Func<T, int, double> _weight = (v, i) => 0;

        /// <summary>
        /// Sets values for a specific point
        /// </summary>
        /// <param name="point">Point to set</param>
        /// <param name="value"></param>
        /// <param name="key"></param>
        public override void Evaluate(int key, T value, ChartPoint point)
        {
            base.Evaluate(key, value, point);
            point.Weight = _weight(value, key);
        }

        /// <summary>
        /// Sets the X coordinate mapper.
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public WeightedMapper<T> X(Func<T, double> predicate)
        {
            return (WeightedMapper<T>)SetFirstDimension((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the X coordinate mapper.
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public WeightedMapper<T> X(Func<T, int, double> predicate)
        {
            return (WeightedMapper<T>)SetFirstDimension(predicate);
        }

        /// <summary>
        /// Sets the Y coordinate mapper.
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public WeightedMapper<T> Y(Func<T, double> predicate)
        {
            return (WeightedMapper<T>)SetSecondDimension((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Y coordinate mapper.
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public WeightedMapper<T> Y(Func<T, int, double> predicate)
        {
            return (WeightedMapper<T>)SetSecondDimension(predicate);
        }

        /// <summary>
        /// Sets Weight mapper
        /// </summary>
        /// <param name="predicate">function that pulls the point's weight</param>
        /// <returns>current mapper instance</returns>
        public WeightedMapper<T> Weight(Func<T, double> predicate)
        {
            return Weight((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets Weight mapper
        /// </summary>
        /// <param name="predicate">function that pulls the point's weight, value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public WeightedMapper<T> Weight(Func<T, int, double> predicate)
        {
            _weight = predicate;
            return this;
        }
    }
}