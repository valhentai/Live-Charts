using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Samples.Wpf.CartesianChart;
using Samples.Wpf.CartesianChart.BasicLine;
using Samples.Wpf.CartesianChart.Basic_Bars;
using Samples.Wpf.CartesianChart.Basic_Stacked_Bar;
using Samples.Wpf.CartesianChart.Bubbles;
using Samples.Wpf.CartesianChart.Chart_to_Image;
using Samples.Wpf.CartesianChart.ConstantChanges;
using Samples.Wpf.CartesianChart.Customized_Line_Series;
using Samples.Wpf.CartesianChart.CustomTooltipAndLegend;
using Samples.Wpf.CartesianChart.DataLabelTemplate;
using Samples.Wpf.CartesianChart.DateAxis;
using Samples.Wpf.CartesianChart.DynamicVisibility;
using Samples.Wpf.CartesianChart.Energy_Predictions;
using Samples.Wpf.CartesianChart.Events;
using Samples.Wpf.CartesianChart.Financial;
using Samples.Wpf.CartesianChart.FullyResponsive;
using Samples.Wpf.CartesianChart.Funnel_Chart;
using Samples.Wpf.CartesianChart.GanttChart;
using Samples.Wpf.CartesianChart.HeatChart;
using Samples.Wpf.CartesianChart.Inverted_Series;
using Samples.Wpf.CartesianChart.Irregular_Intervals;
using Samples.Wpf.CartesianChart.Linq;
using Samples.Wpf.CartesianChart.LogarithmScale;
using Samples.Wpf.CartesianChart.ManualZAndP;
using Samples.Wpf.CartesianChart.MaterialCards;
using Samples.Wpf.CartesianChart.Missing_Line_Points;
using Samples.Wpf.CartesianChart.NegativeStackedRow;
using Samples.Wpf.CartesianChart.PointState;
using Samples.Wpf.CartesianChart.ScatterPlot;
using Samples.Wpf.CartesianChart.Scatter_With_Pies;
using Samples.Wpf.CartesianChart.Sections;
using Samples.Wpf.CartesianChart.SectionsDragable;
using Samples.Wpf.CartesianChart.SectionsMouseMove;
using Samples.Wpf.CartesianChart.SolidColorChart;
using Samples.Wpf.CartesianChart.StackedArea;
using Samples.Wpf.CartesianChart.StepLine;
using Samples.Wpf.CartesianChart.ThreadSafe;
using Samples.Wpf.CartesianChart.UIElements;
using Samples.Wpf.CartesianChart.WindowAxis;
using Samples.Wpf.CartesianChart.ZoomingAndPanning;
using Wpf.Gauges;
using Wpf.Maps;
using Wpf.PieChart;
using Wpf.PieChart.DropDowns;

namespace Wpf.Home
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private UserControl _content;
        private bool _isMenuOpen;
        private string _criteria;
        private IEnumerable<SampleGroupVm> _samples;
        private readonly IEnumerable<SampleGroupVm> _dataSource;

        public HomeViewModel()
        {
            IsMenuOpen = true;
            _dataSource = new[]
            {
                new SampleGroupVm
                {
                    Name = "Customizing",
                    Items = new[]
                    {
                        new SampleVm("Series", typeof(CustomizedLineSeries)),
                        new SampleVm("Tooltips/Legends", typeof(CustomTooltipAndLegendExample)),
                        new SampleVm("Material Design", typeof(MaterialCards)),
                        new SampleVm("Solid Color", typeof(SolidColorExample)),
                        new SampleVm("Energy Predictions", typeof(EnergyPredictionExample)),
                        new SampleVm("Funnel Chart", typeof(FunnelExample))
                    }
                },
                new SampleGroupVm
                {
                    Name = "Basic Plots",
                    Items = new[]
                    {
                        new SampleVm("Lines", typeof(BasicLineExample)),
                        new SampleVm("Columns", typeof(BasicColumn)),
                        new SampleVm("Stacked Columns", typeof(StackedColumnExample)),
                        new SampleVm("Rows", typeof(BasicRowExample)),
                        new SampleVm("Stacked Area", typeof(StackedAreaExample)),
                        new SampleVm("Step Line", typeof(StepLineExample)),
                        new SampleVm("Scatter", typeof(ScatterExample)),
                        new SampleVm("Bubbles", typeof(BubblesExample)),
                        new SampleVm("OHCL Series", typeof(OhclExample)),
                        new SampleVm("Candle Series", typeof(CandleStickExample)),
                        new SampleVm("Pie Chart", typeof(PieChartExample)),
                        new SampleVm("Doughnut Chart", typeof(DoughnutChartExample)),
                        new SampleVm("Solid Gauges", typeof(Gauge360)),
                        new SampleVm("Angular Gauge", typeof(AngularGaugeExmple)),
                        new SampleVm("Heat Series", typeof(HeatSeriesExample)),
                        new SampleVm("GeoHeatMap", typeof(GeoMapExample)),
                        new SampleVm("Gantt Chart", typeof(GanttExample))
                    }
                },
                new SampleGroupVm
                {
                    Name = "Features",
                    Items = new[]
                    {
                        new SampleVm("Inverted Series", typeof(InvertedExample)),
                        new SampleVm("Sections", typeof(SectionsExample)),
                        new SampleVm("Dragable Sections", typeof(DragableSections)),
                        new SampleVm("Section Mouse Move", typeof(SectionMouseMoveSample)),
                        new SampleVm("Multiple Axes", typeof(MultiAxesChart)),
                        new SampleVm("Events", typeof(EventsExample)),
                        new SampleVm("Visual Elements", typeof(UiElementsAndEventsExample)),
                        new SampleVm("Chart to Image", typeof(ChartToImageSample)),
                        new SampleVm("DataLabelTemplate", typeof(DataLabelTemplateSample))
                    }
                },
                new SampleGroupVm
                {
                    Name = "More",
                    Items = new[]
                    {
                        new SampleVm("Irregular intervals", typeof(IrregularIntervalsExample)),
                        new SampleVm("Pie with drop downs", typeof(PieDropDownSample)),
                        new SampleVm("Missing Points", typeof(MissingPointsExample)),
                        new SampleVm("Constant Changes", typeof(ConstantChangesChart)),
                        new SampleVm("Thread Safe", typeof(ThreadSafeExample)),
                        new SampleVm("Zooming/Panning", typeof(ZoomingAndPanning)),
                        new SampleVm("Data Pagination", typeof(ManualZAndPExample)),
                        new SampleVm("Scattered Pie", typeof(Scatter_With_Pies)),
                        new SampleVm("Observable point", typeof(FullyResponsiveExample)),
                        new SampleVm("Point State", typeof(PointStateExample)),
                        new SampleVm("Negative Stacked", typeof(NegativeStackedRowExample)),
                        new SampleVm("Dynamic Visibility", typeof(DynamicVisibilityExample)),
                        new SampleVm("Filtering Data", typeof(LinqExample)),
                        new SampleVm("Percentage Stacked", typeof(BasicStackedRowPercentageExample))
                    }
                },
                new SampleGroupVm
                {
                    Name = "Special axes",
                    Items = new[]
                    {
                        new SampleVm("Window Axis", typeof(WindowAxisExample)),
                        new SampleVm("Date Axis", typeof(DateAxisExample)),
                        new SampleVm("Logarithmic Axis", typeof(LogarithmScaleExample))
                    }
                }
            };

            _samples = _dataSource;
        }

        public IEnumerable<SampleGroupVm> Samples
        {
            get { return _samples; }
            set
            {
                _samples = value;
                OnPropertyChanged("Samples");
            }
        }
        public UserControl Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }
        public bool IsMenuOpen
        {
            get { return _isMenuOpen; }
            set
            {
                _isMenuOpen = value;
                OnPropertyChanged("IsMenuOpen");
            }
        }
        public string Criteria
        {
            get { return _criteria; }
            set
            {
                _criteria = value;
                OnPropertyChanged("Criteria");
                OnCriteriaChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnCriteriaChanged()
        {
            if (string.IsNullOrWhiteSpace(Criteria))
            {
                Samples = _dataSource;
                return;
            }

            Samples = Samples.Select(x => new SampleGroupVm
            {
                Name = x.Name,
                Items = x.Items.Where(y => y.Title.ToLowerInvariant().Contains(Criteria.ToLowerInvariant()) ||
                                           y.Tags.ToLowerInvariant().Contains(Criteria.ToLowerInvariant()))
            });
        }
    }

    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
