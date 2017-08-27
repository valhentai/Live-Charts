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
using LiveCharts.Series;
using LiveCharts.Wpf.PointViews;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The vertical line series is useful to compare trends, this is the inverted version of the LineSeries, this series must be added in a cartesian chart.
    /// </summary>
    public class VerticalLineSeries : LineSeries, ILineSeriesView
    {
        #region Fields

        private int _activeSplitterCount;
        private int _splittersCollector;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes an new instance of VerticalLineSeries class
        /// </summary>
        public VerticalLineSeries()
        {
            Core = new VerticalLineCore(this);
            InitializeDefuaults();
        }

        /// <summary>
        /// Initializes an new instance of VerticalLineSeries class, with a given mapper
        /// </summary>
        public VerticalLineSeries(BiDimensinalMapper configuration)
        {
            Core = new VerticalLineCore(this);
            Configuration = configuration;
            InitializeDefuaults();
        }

        #endregion

        #region Series View Implementation

        void ILineSeriesView.StartSegment(CorePoint location, double areaLimit, TimeSpan animationsSpeed)
        {
            InitializeNewPath(location, areaLimit);

            var splitter = PathCollection[_activeSplitterCount];
            splitter.SplitterCollectorIndex = _splittersCollector;

            if (animationsSpeed == TimeSpan.Zero)
            {
                PathCollection[_activeSplitterCount].StrokeFigure.StartPoint = new Point(location.X, location.Y);
                PathCollection[_activeSplitterCount].ShadowFigure.StartPoint = new Point(areaLimit, areaLimit);

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
                        new PointAnimation(new Point(areaLimit, location.Y), animationsSpeed));

                splitter.Bottom.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(areaLimit, location.Y), animationsSpeed));

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
                splitter.Right.Point = new Point(areaLimit, location.Y);
            }
            else
            {
                splitter.Right.BeginAnimation(LineSegment.PointProperty,
                    new PointAnimation(new Point(areaLimit, location.Y), animationsSpeed));
            }

            PathCollection[_activeSplitterCount].ShadowFigure.Segments.Insert(atIndex, splitter.Right);

            _activeSplitterCount++;
        }

        #endregion

        #region Overridden Methods

        /// <inheritdoc cref="Series.InitializePointView"/>
        protected override IChartPointView InitializePointView(IChart2DView chartView)
        {
            var pointView = new BaseBezierPointView
            {
                ShadowPath = PathCollection[_activeSplitterCount].ShadowFigure,
                StrokePath = PathCollection[_activeSplitterCount].StrokeFigure
            };

            return pointView;
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
