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

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Definitions.Series;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The default legend control, by default a new instance of this control is created for every chart that requires a legend.
    /// </summary>
    public partial class DefaultLegend : IChartLegend, INotifyPropertyChanged
    {
        private ISeriesView[] _series;
         
        /// <summary>
        /// Initializes a new instance of DefaultLegend class
        /// </summary>
        public DefaultLegend()
        {
            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the series displayed in the legend.
        /// </summary>
        public ISeriesView[] Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged("Series");
            }
        }

        /// <summary>
        /// The orientation property
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof (Orientation?), typeof (DefaultLegend), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets the orientation of the legend, default is null, if null LiveCharts will decide which orientation to use, based on the Chart.Legend location property.
        /// </summary>
        public Orientation? Orientation
        {
            get { return (Orientation?) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// The actual orientation property
        /// </summary>
        public static readonly DependencyProperty ActualOrientationProperty = DependencyProperty.Register(
            "ActualOrientation", typeof(Orientation), typeof(DefaultLegend), new PropertyMetadata(default(Orientation)));

        /// <summary>
        /// Gets the actual orientation.
        /// </summary>
        /// <value>
        /// The actual orientation.
        /// </value>
        public Orientation ActualOrientation
        {
            get { return (Orientation) GetValue(ActualOrientationProperty); }
            internal set { SetValue(ActualOrientationProperty, value); }
        }

        /// <summary>
        /// The bullet size property
        /// </summary>
        public static readonly DependencyProperty BulletSizeProperty = DependencyProperty.Register(
            "BulletSize", typeof(double), typeof(DefaultLegend), new PropertyMetadata(15d));
        /// <summary>
        /// Gets or sets the bullet size, the bullet size modifies the drawn shape size.
        /// </summary>
        public double BulletSize
        {
            get { return (double)GetValue(BulletSizeProperty); }
            set { SetValue(BulletSizeProperty, value); }
        }

        /// <summary>
        /// Notifies the UI that a property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
