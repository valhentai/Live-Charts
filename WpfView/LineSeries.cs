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
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;
using LiveCharts.Series;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.PointViews;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The line series displays trends between points, you must add this series to a cartesian chart. 
    /// </summary>
    public class LineSeries : Series, ILineSeriesView, IAreaPointView, IFondeable
    {
        #region Fields

        private int _activeSplitterCount;
        private int _splittersCollector;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LineSeries class
        /// </summary>
        public LineSeries() 
        {
            Core = new LineCore(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes a new instance of the LineSeries class with a given mapper
        /// </summary>
        /// <param name="configuration"></param>
        public LineSeries(BiDimensinalMapper configuration)
        {
            Core = new LineCore(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the path collection.
        /// </summary>
        /// <value>
        /// The path collection.
        /// </value>
        public List<LineSeriesPathHelper> PathCollection { get; protected set; }

        /// <summary>
        /// The point geometry size property
        /// </summary>
        public static readonly DependencyProperty PointGeometrySizeProperty = DependencyProperty.Register(
            "PointGeometrySize", typeof(double), typeof(LineSeries),
            new PropertyMetadata(8d, EnqueueUpdateCallback));
        /// <summary>
        /// Gets or sets the point geometry size, increasing this property will make the series points bigger
        /// </summary>
        public double PointGeometrySize
        {
            get { return (double)GetValue(PointGeometrySizeProperty); }
            set { SetValue(PointGeometrySizeProperty, value); }
        }

        /// <summary>
        /// The point foreground property
        /// </summary>
        public static readonly DependencyProperty PointForegroundProperty = DependencyProperty.Register(
            "PointForeground", typeof (Brush), typeof (LineSeries), 
            new PropertyMetadata(Brushes.White));
        /// <summary>
        /// Gets or sets the point shape foreground.
        /// </summary>
        public Brush PointForeground
        {
            get { return (Brush) GetValue(PointForegroundProperty); }
            set { SetValue(PointForegroundProperty, value); }
        }

        /// <summary>
        /// The line smoothness property
        /// </summary>
        public static readonly DependencyProperty LineSmoothnessProperty = DependencyProperty.Register(
            "LineSmoothness", typeof (double), typeof (LineSeries), 
            new PropertyMetadata(0.7d, EnqueueUpdateCallback));
        /// <summary>
        /// Gets or sets line smoothness, this property goes from 0 to 1, use 0 to draw straight lines, 1 really curved lines.
        /// </summary>
        public double LineSmoothness
        {
            get { return (double) GetValue(LineSmoothnessProperty); }
            set { SetValue(LineSmoothnessProperty, value); }
        }

        /// <summary>
        /// The area limit property
        /// </summary>
        public static readonly DependencyProperty AreaLimitProperty = DependencyProperty.Register(
            "AreaLimit", typeof(double), typeof(LineSeries), new PropertyMetadata(double.NaN));
        /// <summary>
        /// Gets or sets the limit where the fill area changes orientation
        /// </summary>
        public double AreaLimit
        {
            get { return (double) GetValue(AreaLimitProperty); }
            set { SetValue(AreaLimitProperty, value); }
        }

        #endregion

        #region Series View Implementation

        double ILineSeriesView.LineSmoothness => LineSmoothness;

        double ILineSeriesView.AreaLimit => AreaLimit;

        double IAreaPointView.PointMaxRadius => (PointGeometry == null ? 0 : PointGeometrySize) / 2;

        void ILineSeriesView.StartSegment(CorePoint location, double areaLimit, TimeSpan animationsSpeed)
        {
            InitializeNewPath(location, areaLimit);

            var splitter = PathCollection[_activeSplitterCount];
            splitter.SplitterCollectorIndex = _splittersCollector;

            if (animationsSpeed == TimeSpan.Zero)
            {
                PathCollection[_activeSplitterCount].StrokeFigure.StartPoint = new Point(location.X, location.Y);
                PathCollection[_activeSplitterCount].ShadowFigure.StartPoint = new Point(location.X, areaLimit);

                splitter.Bottom.Point = new Point(location.X, areaLimit);
                splitter.Left.Point = location.AsPoint();
            }
            else
            {
                PathCollection[_activeSplitterCount]
                    .StrokeFigure.BeginAnimation(
                        PathFigure.StartPointProperty,
                        new PointAnimation(new Point(location.X, location.Y), animationsSpeed));

                PathCollection[_activeSplitterCount]
                    .ShadowFigure.BeginAnimation(
                        PathFigure.StartPointProperty,
                        new PointAnimation(new Point(location.X, areaLimit), animationsSpeed));

                splitter.Bottom.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(location.X, areaLimit), animationsSpeed));

                splitter.Left.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(location.AsPoint(), animationsSpeed));
            }
        }

        void ILineSeriesView.EndSegment(int atIndex, CorePoint location, double areaLimit, TimeSpan animationsSpeed)
        {
            var splitter = PathCollection[_activeSplitterCount];

            PathCollection[_activeSplitterCount].ShadowFigure.Segments.Remove(splitter.Right);

            if (animationsSpeed == TimeSpan.Zero)
            {
                splitter.Right.Point = new Point(location.X, areaLimit);
            }
            else
            {
                splitter.Right.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(location.X, areaLimit), animationsSpeed));
            }

            PathCollection[_activeSplitterCount].ShadowFigure.Segments.Insert(atIndex, splitter.Right);

            _activeSplitterCount++;
        }

        #endregion

        #region Overridden Methods

        /// <inheritdoc cref="Series.OnSeriesUpdateStart" />
        protected override void OnSeriesUpdateStart()
        {
            _activeSplitterCount = 0;
            if (_splittersCollector != int.MaxValue - 1) return;
            PathCollection.ForEach(s => s.SplitterCollectorIndex = 0);
            _splittersCollector = 0;
        }

        /// <inheritdoc cref="Series.OnSeriesUpdateFinish" />
        protected override void OnSeriesUpdateFinish()
        {
            base.OnSeriesUpdateFinish();

            for (var i = _activeSplitterCount; i < PathCollection.Count; i++)
            {
                var s = PathCollection[i - 1];
                Core.Chart.View.RemoveFromView(s);
                PathCollection.Remove(s);
            }
        }

        /// <inheritdoc cref="Series.InitializePointView"/>
        protected override IChartPointView InitializePointView(IChart2DView chartView)
        {
            var pointView = new HorizontalBezierPointView
            {
                ShadowPath = PathCollection[_activeSplitterCount].ShadowFigure,
                StrokePath = PathCollection[_activeSplitterCount].StrokeFigure
            };

            return pointView;
        }

        /// <inheritdoc cref="Erase" />
        protected override void Erase(bool removeFromView = true)
        {
            ((ISeriesView)this).ActualValues.GetPoints(this).ForEach(p =>
            {
                p.View?.Erase(Core.Chart);
            });

            if (removeFromView)
            {
                PathCollection.ForEach(s =>
                {
                    Core.Chart.View.RemoveFromDrawMargin(s.StrokePath);
                    Core.Chart.View.RemoveFromDrawMargin(s.ShadowPath);
                });
                Core.Chart.View.RemoveFromView(this);
            }
        }

        #endregion

        #region Private Methods

        private void InitializeNewPath(CorePoint location, double areaLimit)
        {
            if (PathCollection.Count > _activeSplitterCount) return;

            var shadow = new Path
            {
                Fill = Fill,
                Visibility = Visibility,
                StrokeDashArray = StrokeDashArray,
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(location.X, areaLimit)
                        }
                    }
                }
            };

            Panel.SetZIndex(shadow, Panel.GetZIndex(this));
            Panel.SetZIndex(shadow, Panel.GetZIndex(this));
            Core.Chart.View.EnsureElementBelongsToCurrentDrawMargin(shadow);

            var line = new Path
            {
                Visibility = Visibility,
                StrokeDashArray = StrokeDashArray,
                StrokeThickness = StrokeThickness,
                Stroke = Stroke,
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(location.X, areaLimit)
                        }
                    }
                }
            };

            Panel.SetZIndex(line, Panel.GetZIndex(this));
            Panel.SetZIndex(line, Panel.GetZIndex(this));
            Core.Chart.View.EnsureElementBelongsToCurrentDrawMargin(line);

            PathCollection.Add(new LineSeriesPathHelper(location, areaLimit)
            {
                StrokePath = line,
                ShadowPath = shadow
            });

            _splittersCollector++;

            var splitter = PathCollection[_activeSplitterCount];
            splitter.SplitterCollectorIndex = _splittersCollector;
            PathCollection[_activeSplitterCount].ShadowFigure.Segments.Remove(splitter.Bottom);
            PathCollection[_activeSplitterCount].ShadowFigure.Segments.Remove(splitter.Left);
            PathCollection[_activeSplitterCount].ShadowFigure.Segments.Insert(0, splitter.Bottom);
            PathCollection[_activeSplitterCount].ShadowFigure.Segments.Insert(1, splitter.Left);
        }

        private void InitializeDefuaults()
        {
            // ToDo: verify if EnqueueUpdateCallback method is attached to property change
            StrokeThicknessProperty.OverrideMetadata(typeof(LineSeries), new PropertyMetadata(2d));

            Func<ChartPoint, string> defaultLabel = x => Core.CurrentYAxis.GetFormatter()(x.Y);

            SetCurrentValue(LabelPointProperty, defaultLabel);

            DefaultFillOpacity = 0.15;
            PathCollection = new List<LineSeriesPathHelper>();
        }

        #endregion

    }
}
