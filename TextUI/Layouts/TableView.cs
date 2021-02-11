using System;
using System.Linq;
using TextUI.Interfaces;

namespace TextUI.Layouts
{
    public class TableView : IRender, IBorderFeedback
    {
        private ITable table;

        public TableView(ITable table)
        {
            this.table = table;
        }

        public void Render(ICanvas canvas)
        {
            (this as IBorderFeedback).Render(canvas);
        }

        Feedback IBorderFeedback.Render(ICanvas canvas)
        {
            var data = new[] { table.Columns }.Union(table.Rows.Take(canvas.Height - 2)).ToArray();

            var widths = table.Columns.Select(c => c.Length).ToArray();

            var feedback = new Feedback();

            feedback.Left[2] = BorderType.Single;
            feedback.Right[2] = BorderType.Single;

            foreach (var row in data)
            {
                for (var i = 0; i < widths.Length; ++i)
                {
                    widths[i] = Math.Max(widths[i], row[i].Length);
                }
            }

            var totalWidth = widths.Sum();
            var availableWidth = canvas.Width - 2 * widths.Length;

            var cumulative = 0;
            var ratio = (float)availableWidth / totalWidth;

            for (var i = 0; i < widths.Length - 1; ++i)
            {
                widths[i] = (int)(widths[i] * ratio);
                if (widths[i] == 0) widths[i] = 1;
                cumulative += widths[i];

                var k = cumulative + 2 + 3 * i;

                if (k < canvas.Width)
                {
                    feedback.Top[k] = BorderType.Single;
                    feedback.Bottom[k] = BorderType.Single;
                }
            }

            widths[widths.Length - 1] = availableWidth - cumulative;

            var line = 0;
            var x = 0;

            for (var i = 0; i < widths.Length; ++i)
            {
                if (i > 0)
                {
                    if (x > canvas.Width - 4)
                        break;

                    canvas.Put(x++, line, ' ');
                    canvas.Put(x++, line, Border.SingleBorder.Vertical);
                    canvas.Put(x++, line, ' ');
                }

                var cell = (table.GetTextAlign(i) == TextAlign.Right ? table.Columns[i].PadLeft(widths[i]) : table.Columns[i].PadRight(widths[i])).Substring(0, widths[i]);

                foreach (var c in cell)
                {
                    canvas.Put(x++, line, c);
                    if (x >= canvas.Width)
                        break;
                }


                if (x >= canvas.Width)
                    break;
            }

            if (++line >= canvas.Height)
                return feedback;
            x = 0;
            for (var i = 0; i < widths.Length; ++i)
            {
                if (i > 0)
                {
                    if (x > canvas.Width - 4)
                        break;

                    canvas.Put(x++, line, Border.SingleBorder.Horizontal);
                    canvas.Put(x++, line, BoxArt.Get(BorderType.Single, BorderType.Single));
                    canvas.Put(x++, line, Border.SingleBorder.Horizontal);
                }

                var cell = new String(Border.SingleBorder.Horizontal, widths[i]);

                foreach (var c in cell)
                {
                    canvas.Put(x++, line, c);
                    if (x >= canvas.Width)
                        break;
                }


                if (x >= canvas.Width)
                    break;
            }

            if (++line >= canvas.Height)
                return feedback;

            foreach (var row in data)
            {
                x = 0;

                for (var i = 0; i < widths.Length; ++i)
                {
                    if (i > 0)
                    {
                        if (x > canvas.Width - 4)
                            break;

                        canvas.Put(x++, line, ' ');
                        canvas.Put(x++, line, Border.SingleBorder.Vertical);
                        canvas.Put(x++, line, ' ');
                    }

                    var cell = (table.GetTextAlign(i) == TextAlign.Right ? row[i].PadLeft(widths[i]) : row[i].PadRight(widths[i])).Substring(0, widths[i]); ;

                    foreach (var c in cell)
                    {
                        canvas.Put(x++, line, c);
                        if (x >= canvas.Width)
                            break;
                    }

                    if (x >= canvas.Width)
                        break;
                }

                if (++line >= canvas.Height)
                    break;
            }

            return feedback;
        }
    }
}