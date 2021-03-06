﻿// The MIT License(MIT)

// Copyright(c) 2021 Alberto Rodriguez Orozco & LiveCharts Contributors

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using LiveChartsCore.Context;
using LiveChartsCore.Drawing;
using System;
using System.Drawing;

namespace LiveChartsCore
{
    public class StyleBuilder<TDrawingContext>
        where TDrawingContext : DrawingContext
    {
        private Color[]? colors;
        private SeriesInitializer<TDrawingContext>? seriesInitializer;
        private readonly DefaultPaintTask<TDrawingContext> defaultPaintTask = new DefaultPaintTask<TDrawingContext>();

        public DefaultPaintTask<TDrawingContext> DefaultPaintTask => defaultPaintTask;

        public StyleBuilder<TDrawingContext> UseColors(params Color[] colors)
        {
            this.colors = colors;
            return this;
        }

        public StyleBuilder<TDrawingContext> UseSeriesInitializer(SeriesInitializer<TDrawingContext> seriesInitializer)
        {
            this.seriesInitializer = seriesInitializer;
            return this;
        }

        internal void ConstructSeries(IDrawableSeries<TDrawingContext> series)
        {
            if (seriesInitializer == null)
                throw new NullReferenceException(
                    $"An instance of {nameof(SeriesInitializer<TDrawingContext>)} is no configured yet, " +
                    $"please register an instance using {nameof(UseSeriesInitializer)}() method.");

            seriesInitializer.ConstructSeries(series);
        }

        internal void ResolveDefaults(IDrawableSeries<TDrawingContext> series)
        {
            if (seriesInitializer == null)
                throw new NullReferenceException(
                    $"An instance of {nameof(SeriesInitializer<TDrawingContext>)} is no configured yet, " +
                    $"please register an instance using {nameof(UseSeriesInitializer)}() method.");

            if (colors == null || colors.Length == 0)
                throw new NullReferenceException(
                    $"A color pack is not registered yet or is not valid, please register a pack using the {nameof(UseColors)}() method.");

            seriesInitializer.ResolveDefaults(colors[series.SeriesId % colors.Length], series);
        }
    }
}
