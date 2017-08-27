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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Configurations;
using LiveCharts.Definitions.Series;
using LiveCharts.Series;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The stacked area compares trends and percentage, add this series to a cartesian chart
    /// </summary>
    /// <seealso cref="LiveCharts.Wpf.LineSeries" />
    /// <seealso cref="LiveCharts.Definitions.Series.IStackedAreaSeriesView" />
    public class StackedAreaSeries : LineSeries, IStackedAreaSeriesView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of StackedAreaSeries class
        /// </summary>
        public StackedAreaSeries()
        {
            Core = new StackedAreaCore(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of StackedAreaSeries class, with a given mapper
        /// </summary>
        public StackedAreaSeries(BiDimensinalMapper configuration)
        {
            Core = new StackedAreaCore(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The stack mode property
        /// </summary>
        public static readonly DependencyProperty StackModeProperty = DependencyProperty.Register(
            "StackMode", typeof(StackMode), typeof(StackedAreaSeries),
            new PropertyMetadata(default(StackMode), EnqueueUpdateCallback));
        /// <summary>
        /// Gets or sets the series stacked mode, values or percentage
        /// </summary>
        public StackMode StackMode
        {
            get { return (StackMode)GetValue(StackModeProperty); }
            set { SetValue(StackModeProperty, value); }
        }
        #endregion

        #region Private Methods

        private void InitializeDefuaults()
        {
            SetCurrentValue(LineSmoothnessProperty, .7d);
            SetCurrentValue(PointGeometrySizeProperty, 0d);
            SetCurrentValue(PointForegroundProperty, Brushes.White);
            SetCurrentValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(229, 229, 229)));
            SetCurrentValue(StrokeThicknessProperty, 0d);
            DefaultFillOpacity = 1;

            Func<ChartPoint, string> defaultLabel = x => Core.CurrentYAxis.GetFormatter()(x.Y);
            SetCurrentValue(LabelPointProperty, defaultLabel);

            PathCollection = new List<LineSeriesPathHelper>();
        }

        #endregion
    }
}
