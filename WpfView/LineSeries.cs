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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;
using LiveCharts.Series;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Points;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The line series displays trends between points, you must add this series to a cartesian chart. 
    /// </summary>
    public class LineSeries : Series, ILineSeriesView, IAreaPointView, IFondeable
    {
        public Path Path { get; set; }
        public PathFigure Figure { get; set; }
        public bool IsNew { get; set; }
        public bool IsPathInitialized  { get; set; }
        public virtual void StartSegment(int atIndex, CorePoint location)
        {
        }
        public virtual void EndSegment(int atIndex, CorePoint location)
        {
        }
        //ToDo: Remove all these ^^

        #region Fields

        private int _activeSplitters;
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
        public LineSeries(object configuration)
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

        double ILineSeriesView.LineSmoothness { get { return LineSmoothness; } }

        double ILineSeriesView.AreaLimit { get { return AreaLimit; } }

        double IAreaPointView.PointMaxRadius { get { return (PointGeometry == null ? 0 : PointGeometrySize) / 2; } }

        void ILineSeriesView.StartSegment(CorePoint location, double areaLimit, TimeSpan animationsSpeed)
        {
            InitializeNewPath(location, areaLimit);

            var splitter = PathCollection[_activeSplitters];
            splitter.SplitterCollectorIndex = _splittersCollector;

            if (animationsSpeed == TimeSpan.Zero)
            {
                PathCollection[_activeSplitters].LineFigure.StartPoint = new Point(location.X, location.Y);
                PathCollection[_activeSplitters].ShadowFigure.StartPoint = new Point(location.X, areaLimit);

                //if (splitter.IsNew)
                //{
                //    splitter.Bottom.Point = new Point(location.X, Core.Chart.View.DrawMarginHeight);
                //    splitter.Left.Point = new Point(location.X, Core.Chart.View.DrawMarginHeight);
                //}

                splitter.Bottom.Point = new Point(location.X, areaLimit);
                splitter.Left.Point = location.AsPoint();
            }
            else
            {
                PathCollection[_activeSplitters]
                    .LineFigure.BeginAnimation(
                        PathFigure.StartPointProperty,
                        new PointAnimation(new Point(location.X, location.Y), animationsSpeed));

                PathCollection[_activeSplitters]
                    .ShadowFigure.BeginAnimation(
                        PathFigure.StartPointProperty,
                        new PointAnimation(new Point(location.X, areaLimit), animationsSpeed));

                splitter.Bottom.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(location.X, areaLimit), animationsSpeed));

                splitter.Left.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(location.AsPoint(), animationsSpeed));
            }
        }

        void ILineSeriesView.EndSegment(int atIndex, CorePoint location)
        {
            var splitter = PathCollection[_activeSplitters];

            var animSpeed = Core.Chart.View.AnimationsSpeed;
            var noAnim = Core.Chart.View.DisableAnimations;

            var areaLimit = ChartFunctions.ToDrawMargin(double.IsNaN(AreaLimit)
                ? Core.Chart.View.SecondDimension[ScalesYAt].Core.FirstSeparator
                : AreaLimit, AxisOrientation.Y, Core.Chart, ScalesYAt);

            var uw = Core.Chart.View.FirstDimension[ScalesXAt].Core.EvaluatesUnitWidth
                ? ChartFunctions.GetUnitWidth(AxisOrientation.X, Core.Chart, ScalesXAt) / 2
                : 0;
            location.X -= uw;

            //if (splitter.IsNew)
            //{
            //    splitter.Right.Point = new Point(location.X, Core.Chart.View.DrawMarginHeight);
            //}

            PathCollection[_activeSplitters].ShadowFigure.Segments.Remove(splitter.Right);
            if (noAnim)
                splitter.Right.Point = new Point(location.X, areaLimit);
            else
                splitter.Right.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(location.X, areaLimit), animSpeed));
            PathCollection[_activeSplitters].ShadowFigure.Segments.Insert(atIndex, splitter.Right);

            _activeSplitters++;
        }

        #endregion

        #region Overridden Methods

        /// <inheritdoc cref="Series.OnSeriesUpdateStart" />
        protected override void OnSeriesUpdateStart()
        {
            _activeSplitters = 0;
            if (_splittersCollector != int.MaxValue - 1) return;
            PathCollection.ForEach(s => s.SplitterCollectorIndex = 0);
            _splittersCollector = 0;
        }

        /// <inheritdoc cref="Series.OnSeriesUpdateFinish" />
        protected override void OnSeriesUpdateFinish()
        {
            base.OnSeriesUpdateFinish();

            for (var i = _activeSplitters; i < PathCollection.Count; i++)
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
                Segment = new BezierSegment(),
                ShadowContainer = PathCollection[_activeSplitters].ShadowFigure,
                LineContainer = PathCollection[_activeSplitters].LineFigure,
                IsNew = true
            };

            chartView.AddToDrawMargin(pointView.Shape);
            chartView.AddToDrawMargin(pointView.DataLabel);

            return pointView;
        }

        /// <inheritdoc cref="Series.GetPointView" />
        protected override IChartPointView GetPointView(ChartPoint point, string label)
        {
            var mhr = PointGeometrySize < 10 ? 10 : PointGeometrySize;

            var pbv = (HorizontalBezierPointView) point.View;

            //if (Core.Chart.View.RequiresHoverShape && pbv.HoverShape == null)
            //{
            //    pbv.HoverShape = new Rectangle
            //    {
            //        Fill = Brushes.Transparent,
            //        StrokeThickness = 0,
            //        Width = mhr,
            //        Height = mhr
            //    };

            //    Panel.SetZIndex(pbv.HoverShape, int.MaxValue);
            //    Core.Chart.View.EnableHoveringFor(pbv.HoverShape);
            //    Core.Chart.View.AddToDrawMargin(pbv.HoverShape);
            //}

            //if (pbv.HoverShape != null) pbv.HoverShape.Visibility = Visibility;

            if (PointGeometry != null && pbv.Shape == null)
            {
                if (PointGeometry != null)
                {
                    pbv.Shape = new Path
                    {
                        Stretch = Stretch.Fill,
                        StrokeThickness = StrokeThickness
                    };
                }

                Core.Chart.View.AddToDrawMargin(pbv.Shape);
            }

            if (pbv.Shape != null)
            {
                pbv.Shape.Fill = PointForeground;
                pbv.Shape.Stroke = Stroke;
                pbv.Shape.StrokeThickness = StrokeThickness;
                pbv.Shape.Width = PointGeometrySize;
                pbv.Shape.Height = PointGeometrySize;
                pbv.Shape.Data = PointGeometry;
                pbv.Shape.Visibility = Visibility;
                Panel.SetZIndex(pbv.Shape, Panel.GetZIndex(this) + 1);

                if (point.Stroke != null) pbv.Shape.Stroke = (Brush)point.Stroke;
                if (point.Fill != null) pbv.Shape.Fill = (Brush)point.Fill;
            }

            if (DataLabels)
            {
                pbv.DataLabel = UpdateLabelContent(new DataLabelViewModel
                {
                    FormattedText = label,
                    Point = point
                }, pbv.DataLabel);
            }

            if (!DataLabels && pbv.DataLabel != null)
            {
                Core.Chart.View.RemoveFromDrawMargin(pbv.DataLabel);
                pbv.DataLabel = null;
            }

            return pbv;
        }

        /// <inheritdoc cref="Erase" />
        protected override void Erase(bool removeFromView = true)
        {
            ((ISeriesView)this).ActualValues.GetPoints(this).ForEach(p =>
            {
                if (p.View != null)
                    p.View.RemoveFromView(Core.Chart);
            });

            if (removeFromView)
            {
                PathCollection.ForEach(s =>
                {
                    Core.Chart.View.RemoveFromDrawMargin(s.LinePath);
                    Core.Chart.View.RemoveFromDrawMargin(s.ShadowPath);
                });
                Core.Chart.View.RemoveFromView(this);
            }
        }

        #endregion

        #region Private Methods

        private void InitializeNewPath(CorePoint location, double areaLimit)
        {
            if (PathCollection.Count > _activeSplitters) return;

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
                },

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
                ShadowFigure = ((PathGeometry)shadow.Data).Figures[0],
                LineFigure = ((PathGeometry)line.Data).Figures[0],
                LinePath = line,
                ShadowPath = shadow
            });

            _splittersCollector++;

            var splitter = PathCollection[_activeSplitters];
            splitter.SplitterCollectorIndex = _splittersCollector;
            PathCollection[_activeSplitters].ShadowFigure.Segments.Remove(splitter.Bottom);
            PathCollection[_activeSplitters].ShadowFigure.Segments.Remove(splitter.Left);
            PathCollection[_activeSplitters].ShadowFigure.Segments.Insert(0, splitter.Bottom);
            PathCollection[_activeSplitters].ShadowFigure.Segments.Insert(1, splitter.Left);
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
