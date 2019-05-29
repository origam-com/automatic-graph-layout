using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Msagl.Core.Geometry
{
    /// <summary>
    /// Pack rectangles (without rotation) into a given aspect ratio
    /// </summary>
    public class TopAlignedRectanglePacking<TData> :OptimalRectanglePacking<TData>
    {
      
        public TopAlignedRectanglePacking(IEnumerable<RectangleToPack<TData>> rectangles, double aspectRatio) : base(rectangles, aspectRatio)
        {
        }

        private bool RectanglesAreAlignedToBottomEdge() {
            var bottomCoords = rectangles
                .Select(x => x.Rectangle.Bottom)
                .ToArray();
            double maxBottom = bottomCoords.Max();
            return bottomCoords
                .Select(x => Math.Abs(x - maxBottom))
                .All(x => x < 0.01);
        }

        protected override void RunInternal()
        {
            base.RunInternal();
            if (rectangles.Count <= 1 ) return;
            if (RectanglesAreAlignedToBottomEdge()) {
                AlignRectanglesToTopEdge();
            }

        }

        private void AlignRectanglesToTopEdge() {
            double maxTopCoordinate = rectangles
                .Select(x => x.Rectangle.Top)
                .Max();

            foreach (var pack in rectangles)
            {
                var rectangle = pack.Rectangle;
                double dy = maxTopCoordinate - rectangle.Top;
                rectangle.Bottom += dy;
                rectangle.Top += dy;
                pack.Rectangle = rectangle;
            }
        }
    }
}