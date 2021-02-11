using TextUI.Interfaces;

namespace TextUI.Layouts
{
    public class Line : IRender, ILayout
    {
        private readonly string[] values;

        public Line(params string[] lines)
        {
            values = lines;
        }

        public int DesiredWidth => 0;

        public int DesiredHeight => 1;

        public void Render(ICanvas canvas)
        {
            var x = 0;
            if (canvas.Height > 0)
            {
                foreach (var value in values)
                {
                    foreach (var c in value)
                    {
                        canvas.Put(x++, 0, c);
                        if (x >= canvas.Width) break;
                    }

                    if (x < canvas.Width - 3)
                    {
                        canvas.Put(x++, 0, ' ');
                        canvas.Put(x++, 0, ' ');
                    }
                }

                while (x < canvas.Width)
                    canvas.Put(x++, 0, ' ');
            }
        }
    }
}