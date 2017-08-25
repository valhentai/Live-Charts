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
    /// Defines a bi-demensional mapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BiDimensionalMapper<T> : BiDimensinalMapper
    {
        private Func<T, int, double> _firstDimension = (v, i) => i;
        private Func<T, int, double> _secondDimension = (v, i) => i;
        private Func<T, int, bool> _selected = (v, i) => false;
        private Func<T, int, object> _stroke;
        private Func<T, int, object> _fill;

        /// <summary>
        /// Evaluates the specified key and value pair, and sets the results to the given chart point.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="point">The point.</param>
        public override void Evaluate(int key, object value, ChartPoint point)
        {
            Evaluate(key, (T) value, point);
        }

        /// <summary>
        /// Evaluates the specified key and value pair, and sets the results to the given chart point.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="point">The point.</param>
        public virtual void Evaluate(int key, T value, ChartPoint point)
        {
            point.X = _firstDimension(value, key);
            point.Y = _secondDimension(value, key);
            point.Selected = _selected(value, key);

            //ToDo: Remove the next 2 lines, they are obsolete, and were replaced with the Selected state.
            if (_stroke != null) point.Stroke = _stroke(value, key);
            if (_fill != null) point.Fill = _fill(value, key);
        }
        
        /// <summary>
        /// Sets the first dimension mapper.
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        protected BiDimensionalMapper<T> SetFirstDimension(Func<T, double> predicate)
        {
            return SetFirstDimension((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the first dimension mapper.
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        protected BiDimensionalMapper<T> SetFirstDimension(Func<T, int, double> predicate)
        {
            _firstDimension = predicate;
            return this;
        }

        /// <summary>
        /// Sets the second dimension mapper.
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public BiDimensionalMapper<T> SetSecondDimension(Func<T, double> predicate)
        {
            return SetSecondDimension((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the second dimension mapper.
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public BiDimensionalMapper<T> SetSecondDimension(Func<T, int, double> predicate)
        {
            _secondDimension = predicate;
            return this;
        }

        /// <summary>
        /// Sets the selection predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual BiDimensionalMapper<T> SelectedWhen(Func<T, int, bool> predicate)
        {
            _selected = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Stroke of the point.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [Obsolete("Replaced with SelectedWhen() method.")]
        public virtual BiDimensionalMapper<T> Stroke(Func<T, object> predicate)
        {
            return Stroke((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Stroke of the point.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [Obsolete("Replaced with SelectedWhen() method.")]
        public virtual BiDimensionalMapper<T> Stroke(Func<T, int, object> predicate)
        {
            _stroke = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Fill of the point.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [Obsolete("Replaced with SelectedWhen() method.")]
        public virtual BiDimensionalMapper<T> Fill(Func<T, object> predicate)
        {
            return Fill((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Fill of the point.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [Obsolete("Replaced with SelectedWhen() method.")]
        public virtual BiDimensionalMapper<T> Fill(Func<T, int, object> predicate)
        {
            _fill = predicate;
            return this;
        }
    }
}
