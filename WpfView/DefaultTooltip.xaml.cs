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
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Default Tooltip control, by default any chart that requires a tooltip will create a new instance of this class.
    /// </summary>
    public partial class DefaultTooltip : IChartTooltip, INotifyPropertyChanged
    {
        private TooltipData _data;

        /// <summary>
        /// Initializes a new instance of DefaultTooltip class
        /// </summary>
        public DefaultTooltip()
        {
            InitializeComponent();
            
            DataContext = this;
        }

        /// <summary>
        /// Initializes the <see cref="DefaultTooltip"/> class.
        /// </summary>
        static DefaultTooltip()
        {
            BackgroundProperty.OverrideMetadata(
                typeof(DefaultTooltip), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(140, 255, 255, 255))));
            PaddingProperty.OverrideMetadata(
                typeof(DefaultTooltip), new FrameworkPropertyMetadata(new Thickness(10, 5, 10, 5)));
            EffectProperty.OverrideMetadata(
                typeof(DefaultTooltip),
                new FrameworkPropertyMetadata(new DropShadowEffect {BlurRadius = 3, Color = Color.FromRgb(50,50,50), Opacity = .2}));
        }

        /// <summary>
        /// The show title property
        /// </summary>
        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
            "ShowTitle", typeof(bool), typeof(DefaultTooltip), new PropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether the tooltip should show the shared coordinate value in the current tooltip data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show title]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowTitle
        {
            get { return (bool) GetValue(ShowTitleProperty); }
            set { SetValue(ShowTitleProperty, value); }
        }

        /// <summary>
        /// The show series property
        /// </summary>
        public static readonly DependencyProperty ShowSeriesProperty = DependencyProperty.Register(
            "ShowSeries", typeof(bool), typeof(DefaultTooltip), new PropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether should show series name and color.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show series]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSeries
        {
            get { return (bool) GetValue(ShowSeriesProperty); }
            set { SetValue(ShowSeriesProperty, value); }
        }

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof (CornerRadius), typeof (DefaultTooltip), new PropertyMetadata(new CornerRadius(4)));
        /// <summary>
        /// Gets or sets the corner radius of the tooltip.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// The selection mode property
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode", typeof (TooltipSelectionMode), typeof (DefaultTooltip),
            new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets the tooltip selection mode, default is null, if this property is null LiveCharts will decide the selection mode based on the series (that fired the tooltip) preferred section mode
        /// </summary>
        public TooltipSelectionMode SelectionMode
        {
            get { return (TooltipSelectionMode) GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// The bullet size property
        /// </summary>
        public static readonly DependencyProperty BulletSizeProperty = DependencyProperty.Register(
            "BulletSize", typeof (double), typeof (DefaultTooltip), new PropertyMetadata(15d));
        /// <summary>
        /// Gets or sets the bullet size, the bullet size modifies the drawn shape size.
        /// </summary>
        public double BulletSize
        {
            get { return (double) GetValue(BulletSizeProperty); }
            set { SetValue(BulletSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public TooltipData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
