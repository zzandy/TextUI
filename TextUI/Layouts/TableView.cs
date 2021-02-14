using System;
using System.Diagnostics;
using System.Linq;
using TextUI.Interfaces;

namespace TextUI.Layouts
{
    public class TableView : IRender, IBorderFeedback
    {
        private readonly ITable table;

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
            var data = table.Rows.Take(canvas.Height - 2).ToArray();

            var widths = table.Columns.Select((c, i) => Math.Max(c.Length, data.Max(r => r[i].Length))).ToArray();

            var feedback = new Feedback();

            var availableWidth = canvas.Width - 2 - 3 * (widths.Length - 1);
            var fullDesiredWidth = widths.Sum();
            var ratio = (float)availableWidth / fullDesiredWidth;

            var sum = 0;
            for (var i = 0; i < widths.Length - 1; ++i)
            {
                sum += widths[i] = Math.Max(1, (int)(widths[i] * ratio));
                feedback.Top[sum + 2 + i * 3] = feedback.Bottom[sum + 2 + i * 3] = BorderType.Single;
            }

            widths[widths.Length - 1] = availableWidth - sum;

            feedback.Left[1] = BorderType.Single;
            feedback.Right[1] = BorderType.Single;

            var line = 0;

            string Align(string value, int width, bool rightAlign) => rightAlign ? value.PadLeft(width) : value.PadRight(width);
            string Justify(string value, int width, bool rightAlign) => value.Length > width ? value.Substring(0, width) : Align(value, width, rightAlign);
            string Pad(string value, int width, bool rightAlign) => ' ' + Justify(value, width, rightAlign) + ' ';

            bool Row(Func<int, string> value, char border)
            {
                var x = 0;
                for (var i = 0; i < widths.Length; ++i)
                {
                    var n = 0;
                    var text = value(i);

                    while (n < text.Length)
                        canvas.Put(x++, line, text[n++]);

                    if (x > canvas.Width)
                        break;

                    if (i != widths.Length - 1)
                    {
                        canvas.Put(x++, line, border);
                    }
                }

                return ++line >= canvas.Height;
            }

            if (Row(i => Pad(table.Columns[i], widths[i], table.GetTextAlign(i) == TextAlign.Right), Frame.SingleBorder.Vertical))
                return feedback;

            if (Row(i => new string(Frame.SingleBorder.Horizontal, widths[i] + 2), BoxArt.Get(BorderType.Single, BorderType.Single)))
                return feedback;

            foreach (var row in data)
            {
                if (Row(i => Pad(row[i], widths[i], table.GetTextAlign(i) == TextAlign.Right), Frame.SingleBorder.Vertical))
                    return feedback;
            }

            return feedback;
        }
    }
}