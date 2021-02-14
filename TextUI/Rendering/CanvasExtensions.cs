﻿using System.Diagnostics;
using TextUI.Interfaces;

namespace TextUI.Rendering
{
    public static class CanvasExtensions
    {
        public static ICanvas Area(this ICanvas canvas, int x, int y, int width, int height)
        {
            return new CanvasArea(canvas, x, y, width, height);
        }

        private sealed class CanvasArea : ICanvas
        {
            private readonly ICanvas parent;
            private readonly int x;
            private readonly int y;

            public CanvasArea(ICanvas parent, int originX, int originY, int width, int height)
            {
                this.parent = parent;
                this.x = originX;
                this.y = originY;

                Width = width;
                Height = height;
            }

            public int Width { get; }

            public int Height { get; }

            public void Put(int x, int y, char character)
            {
                //Debug.Assert(0 <= x && x <  Width);
                //Debug.Assert(0 <= y && y <  Height);

                parent.Put(x + this.x, y + this.y, character);
            }
        }
    }
}