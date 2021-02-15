using TextUI.Extensions;
using TextUI.Interfaces;

namespace TextUI.Layouts
{
    public sealed class SplitLeftRight : IRender
    {
        private readonly IRender left;
        private readonly IRender right;
        private readonly BorderType? border;

        public SplitLeftRight(IRender left, IRender right, BorderType? border = null)
        {
            this.left = left;
            this.right = right;
            this.border = border;
        }

        public void Render(IRenderContext ctx)
        {
            var splitAt = ctx.Width / 2;

            if (left is ILayout layout && layout.DesiredWidth < splitAt)
            {
                splitAt = layout.DesiredWidth;
            }

            if (border.HasValue)
                ctx.JoinTopDown(splitAt, border.Value);

            var innerLeft = ctx.Scope(0, 0, splitAt, ctx.Height, Side.NotRight);
            left.Render(innerLeft);

            var b = (border.HasValue ? 1 : 0);

            var innerRight = ctx.Scope(splitAt + b, 0, ctx.Width - splitAt - b, ctx.Height, Side.NotLeft);
            right.Render(innerRight);

            if (border.HasValue)
                for (int i = 0; i < ctx.Height; i++)
                {
                    var c = BoxArt.Get(border.Value, innerRight.GetJoinLeft(i), border.Value, innerLeft.GetJoinRight(i));

                    ctx.Put(splitAt, i, c); ;
                }
        }
    }
}