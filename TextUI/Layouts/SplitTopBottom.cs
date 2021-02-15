using TextUI.Extensions;
using TextUI.Interfaces;

namespace TextUI.Layouts
{
    public sealed class SplitTopBottom : IRender
    {
        private readonly IRender top;
        private readonly IRender bottom;
        private readonly BorderType? border;

        public SplitTopBottom(IRender top, IRender bottom, BorderType? border = null)
        {
            this.top = top;
            this.bottom = bottom;
            this.border = border;
        }

        public void Render(IRenderContext ctx)
        {
            var splitAt = ctx.Height / 2;

            if (top is ILayout layout && layout.DesiredHeight < splitAt)
            {
                splitAt = layout.DesiredHeight;
            }

            if (border.HasValue)
                ctx.JoinLeftRight(splitAt, border.Value);

            var innerTop = ctx.Scope(0, 0, ctx.Width, splitAt, Side.NotBottom);
            top.Render(innerTop);

            var b = (border.HasValue ? 1 : 0);
            var innerBottom = ctx.Scope(0, splitAt + b, ctx.Width, ctx.Height - splitAt - b, Side.NotTop);
            bottom.Render(innerBottom);

            if (border.HasValue)
                for (int i = 0; i < ctx.Width; i++)
                {
                    var c = BoxArt.Get(innerTop.GetJoinBottom(i), border.Value, innerBottom.GetJoinTop(i), border.Value);

                    ctx.Put(i, splitAt, c); ;
                }
        }
    }
}