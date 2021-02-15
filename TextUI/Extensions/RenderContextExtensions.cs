using System.Collections.Generic;
using System.Diagnostics;
using TextUI.Interfaces;

namespace TextUI.Extensions
{
    public static class RenderContextExtensions
    {
        public static void JoinTopDown(this IRenderContext renderContext, int x, BorderType border)
        {
            renderContext.JoinTop(x, border);
            renderContext.JoinBottom(x, border);
        }

        public static void JoinLeftRight(this IRenderContext renderContext, int y, BorderType border)
        {
            renderContext.JoinLeft(y, border);
            renderContext.JoinRight(y, border);
        }

        public static IRenderContext Scope(this IRenderContext renderContext, int ox, int oy, int width, int height, Side passJoins = Side.None)
        {
            return new ScopedRenderContext(renderContext, ox, oy, width, height, passJoins);
        }

        private sealed class ScopedRenderContext : IRenderContext
        {
            private readonly IRenderContext parent;
            private readonly int ox;
            private readonly int oy;
            private readonly Side passJoins;

            private readonly Dictionary<int, BorderType> top = new Dictionary<int, BorderType>();
            private readonly Dictionary<int, BorderType> left = new Dictionary<int, BorderType>();
            private readonly Dictionary<int, BorderType> bottom = new Dictionary<int, BorderType>();
            private readonly Dictionary<int, BorderType> right = new Dictionary<int, BorderType>();

            public ScopedRenderContext(IRenderContext parent, int ox, int oy, int width, int height, Side passJoins)
            {
                this.parent = parent;
                this.ox = ox;
                this.oy = oy;
                this.passJoins = passJoins;
                Width = width;
                Height = height;
            }

            public int Width { get; }
            public int Height { get; }

            public void Put(int x, int y, char character)
            {
                Debug.Assert(0 <= x && x < Width);
                Debug.Assert(0 <= y && y < Height);

                parent.Put(ox + x, oy + y, character);
            }

            public void JoinTop(int x, BorderType border)
            {
                top[x] = border;
                
                if(passJoins.HasFlag(Side.Top))
                    parent.JoinTop(ox+x, border);
            }

            public void JoinBottom(int x, BorderType border)
            {
                bottom[x] = border;

                if (passJoins.HasFlag(Side.Bottom))
                    parent.JoinBottom(ox + x, border);
            }

            public void JoinLeft(int y, BorderType border)
            {
                left[y] = border;

                if (passJoins.HasFlag(Side.Left))
                    parent.JoinLeft(oy + y, border);
            }

            public void JoinRight(int y, BorderType border)
            {
                right[y] = border;

                if (passJoins.HasFlag(Side.Right))
                    parent.JoinRight(oy + y, border);
            }

            public BorderType GetJoinTop(int x)
            {
                return top.TryGetValue(x, out var value) ? value : BorderType.None;
            }

            public BorderType GetJoinBottom(int x)
            {
                return bottom.TryGetValue(x, out var value) ? value : BorderType.None;
            }

            public BorderType GetJoinRight(int y)
            {
                return right.TryGetValue(y, out var value) ? value : BorderType.None;
            }

            public BorderType GetJoinLeft(int y)
            {
                return left.TryGetValue(y, out var value) ? value : BorderType.None;
            }
        }
    }
}