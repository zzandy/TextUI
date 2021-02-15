using System.Linq;
using TextUI.Interfaces;

namespace TextUI.Layouts
{
    public class Line : IRender, ILayout
    {
        private readonly string[] values;

        public Line(params string[] lines)
        {
            values = lines;
            DesiredWidth = lines.Sum(line => line.Length) + 2 * lines.Length - 2;
        }

        public int DesiredWidth { get; private set; }

        public int DesiredHeight => 1;

        public void Render(IRenderContext ctx)
        {
            var x = 0;
            if (ctx.Height > 0)
            {
                foreach (var value in values)
                {
                    foreach (var c in value)
                    {
                        ctx.Put(x++, 0, c);
                        if (x >= ctx.Width) break;
                    }

                    if (x < ctx.Width - 3)
                    {
                        ctx.Put(x++, 0, ' ');
                        ctx.Put(x++, 0, ' ');
                    }
                }

                while (x < ctx.Width)
                    ctx.Put(x++, 0, ' ');
            }
        }
    }
}