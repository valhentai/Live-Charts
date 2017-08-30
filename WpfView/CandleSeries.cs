
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
using System.Windows.Media;
using LiveCharts.Configurations;
using LiveCharts.Definitions.Series;
using LiveCharts.Series;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Candle series defines a financial series, add this series to a cartesian chart.
    /// </summary>
    public class CandleSeries : Series, IFinancialSeriesView
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of OhclSeries class
        /// </summary>
        public CandleSeries()
        {
            Core = new CandleCore(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of OhclSeries class with a given mapper
        /// </summary>
        /// <param name="configuration"></param>
        public CandleSeries(BiDimensinalMapper configuration)
        {
            Core = new CandleCore(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The maximum column width property
        /// </summary>
        public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.Register(
            "MaxColumnWidth", typeof (double), typeof (CandleSeries), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets or sets the maximum with of a point, a point will be capped to this width.
        /// </summary>
        public double MaxColumnWidth
        {
            get { return (double) GetValue(MaxColumnWidthProperty); }
            set { SetValue(MaxColumnWidthProperty, value); }
        }

        /// <summary>
        /// The increase brush property
        /// </summary>
        public static readonly DependencyProperty IncreaseBrushProperty = DependencyProperty.Register(
            "IncreaseBrush", typeof (Brush), typeof (CandleSeries), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the brush of the point when close value is grater than open value
        /// </summary>
        public Brush IncreaseBrush
        {
            get { return (Brush) GetValue(IncreaseBrushProperty); }
            set { SetValue(IncreaseBrushProperty, value); }
        }

        /// <summary>
        /// The decrease brush property
        /// </summary>
        public static readonly DependencyProperty DecreaseBrushProperty = DependencyProperty.Register(
            "DecreaseBrush", typeof (Brush), typeof (CandleSeries), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// Gets or sets the brush of the point when close value is less than open value
        /// </summary>
        public Brush DecreaseBrush
        {
            get { return (Brush) GetValue(DecreaseBrushProperty); }
            set { SetValue(DecreaseBrushProperty, value); }
        }

        /// <summary>
        /// The coloring rules property
        /// </summary>
        public static readonly DependencyProperty ColoringRulesProperty = DependencyProperty.Register(
            "ColoringRules", typeof(IList<FinancialColoringRule>), typeof(CandleSeries), new PropertyMetadata(default(IList<FinancialColoringRule>)));
        /// <summary>
        /// Gets or sets the coloring rules, the coloring rules allows you to customize Stroke and Fill properties according to your needs, the first rule in this collection that returns true, will decide the Stroke/Fill of every point. If this property is not null (default is null), CandleSeries Fill/Stroke will be based on DecreaseBrush and IncreaseBrush properties.
        /// </summary>
        /// <value>
        /// The coloring rules.
        /// </value>
        public IList<FinancialColoringRule> ColoringRules
        {
            get { return (IList<FinancialColoringRule>) GetValue(ColoringRulesProperty); }
            set { SetValue(ColoringRulesProperty, value); }
        }

        #endregion

        private void InitializeDefuaults()
        {
            SetCurrentValue(StrokeThicknessProperty, 1d);
            SetCurrentValue(MaxColumnWidthProperty, 35d);
            SetCurrentValue(IncreaseBrushProperty, new SolidColorBrush(Color.FromRgb(76, 174, 80)));
            SetCurrentValue(DecreaseBrushProperty, new SolidColorBrush(Color.FromRgb(238, 83, 80)));

            Func<ChartPoint, string> defaultLabel = x => $"O: {x.Open}, H: {x.High}, L: {x.Low} C: {x.Close}";
            SetCurrentValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 1;
        }
    }
}
