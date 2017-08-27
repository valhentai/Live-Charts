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

using System;
using System.ComponentModel;
using System.Windows;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf.Charts.Base;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Cartesian chart can plot any series with x and y coordinates
    /// </summary>
    public class CartesianChart : Chart2D, ICartesianChart
    {
        /// <summary>
        /// Initializes a new instance of CartesianChart class
        /// </summary>
        public CartesianChart()
        {
            SetCurrentValue(SeriesProperty,
                DesignerProperties.GetIsInDesignMode(this)
                    ? GetDesignerModeCollection()
                    : new SeriesCollection());

            SetCurrentValue(XAxisProperty, new AxesCollection {new Axis()});
            SetCurrentValue(YAxisProperty, new AxesCollection {new Axis()});
            SetCurrentValue(VisualElementsProperty, new VisualElementsCollection());
        }

        #region Properties

        /// <summary>
        /// The axis y property
        /// </summary>
        public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(
            "YAxis", typeof(AxesCollection), typeof(CartesianChart),
            new PropertyMetadata(null, OnAxisInstanceChanged(AxisOrientation.Y)));

        /// <summary>
        /// Gets or sets vertical axis
        /// </summary>
        public AxesCollection YAxis
        {
            get { return (AxesCollection)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets vertical axis
        /// </summary>
        [Obsolete("Renamed to YAxis")]
        public AxesCollection AxisY
        {
            get { return (AxesCollection)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        /// <summary>
        /// The axis x property
        /// </summary>
        public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(
            "XAxis", typeof(AxesCollection), typeof(CartesianChart),
            new PropertyMetadata(null, OnAxisInstanceChanged(AxisOrientation.X)));

        /// <summary>
        /// Gets or sets horizontal axis
        /// </summary>
        public AxesCollection XAxis
        {
            get { return (AxesCollection)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets horizontal axis
        /// </summary>
        [Obsolete("Renamed to XAxis")]
        public AxesCollection AxisX
        {
            get { return (AxesCollection)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        /// <summary>
        /// The visual elements property
        /// </summary>
        public static readonly DependencyProperty VisualElementsProperty = DependencyProperty.Register(
            "VisualElements", typeof (VisualElementsCollection), typeof (CartesianChart),
            new PropertyMetadata(default(VisualElementsCollection), OnVisualCollectionChanged));

        /// <summary>
        /// Gets or sets the collection of visual elements in the chart, a visual element display another UiElement in the chart.
        /// </summary>
        public VisualElementsCollection VisualElements
        {
            get { return (VisualElementsCollection) GetValue(VisualElementsProperty); }
            set { SetValue(VisualElementsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the axis x.
        /// </summary>
        /// <value>
        /// The axis x.
        /// </value>
        public sealed override AxesCollection FirstDimension => XAxis;

        /// <summary>
        /// Gets or sets the axis y.
        /// </summary>
        /// <value>
        /// The axis y.
        /// </value>
        public sealed override AxesCollection SecondDimension => YAxis;

        #endregion

        private static void OnVisualCollectionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var chart = (CartesianChart)dependencyObject;

            if (chart.VisualElements != null) chart.VisualElements.Chart = chart.Core;
        }
    }
}
