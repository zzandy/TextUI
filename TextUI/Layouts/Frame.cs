using TextUI.Extensions;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class Frame : IRender
    {
        private readonly BorderDefinition border;
        private readonly BorderType borderType;
        private readonly IRender content;
        private readonly bool fill;

        public Frame(IRender content, BorderType border)
        {
            fill = content == null;
            this.content = content;
            borderType = border;
            this.border = BoxArt.Border[border];
        }

        public void Render(IRenderContext ctx)
        {
            const BorderType none = BorderType.None;
            IRenderContext inner = null;

            var width = ctx.Width - 1;
            var height = ctx.Height - 1;

            if (content != null)
            {
                inner = ctx.Scope(1, 1, ctx.Width - 2, ctx.Height - 2);
                content.Render(inner);
            }

            ctx.Put(0, 0, border.TopLeftCorner);
            ctx.Put(width, 0, border.TopRightCorner);
            ctx.Put(0, height, border.BottomLeftCorner);
            ctx.Put(width, height, border.BottomRightCorner);

            for (var x = 1; x < width; ++x)
            {
                ctx.Put(x, 0, BoxArt.Get(none, borderType, inner?.GetJoinTop(x - 1) ?? BorderType.None, borderType));
                ctx.Put(x, height, BoxArt.Get(inner?.GetJoinBottom(x - 1) ?? BorderType.None, borderType, none, borderType));
            }

            for (var y = 1; y < height; ++y)
            {
                ctx.Put(0, y, BoxArt.Get(borderType, inner?.GetJoinLeft(y - 1) ?? BorderType.None, borderType, none));
                ctx.Put(width, y, BoxArt.Get(borderType, none, borderType, inner?.GetJoinRight(y - 1) ?? BorderType.None));
            }
        }
    }
}