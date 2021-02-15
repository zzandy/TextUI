﻿using System;
using System.Linq;
using TextUI.Extensions;
using TextUI.Interfaces;

namespace TextUI.Layouts
{
    public class TableView : IRender
    {
        private readonly ITable table;

        private static readonly char Vertical = BoxArt.Get(BorderType.Single, BorderType.None);
        private static readonly char Horizontal = BoxArt.Get(BorderType.None, BorderType.Single);
        private static readonly char Cross = BoxArt.Get(BorderType.Single, BorderType.Single);

        public TableView(ITable table)
        {
            this.table = table;
        }

        public void Render(IRenderContext ctx)
        {
            var data = table.Rows.Take(ctx.Height - 2).ToArray();

            var widths = table.Columns.Select((c, i) => Math.Max(c.Length, data.Max(r => r[i].Length))).ToArray();

            var availableWidth = ctx.Width - 2 - 3 * (widths.Length - 1);
            var fullDesiredWidth = widths.Sum();
            var ratio = (float)availableWidth / fullDesiredWidth;

            var sum = 0;
            for (var i = 0; i < widths.Length - 1; ++i)
            {
                sum += widths[i] = Math.Max(1, (int)(widths[i] * ratio));

                ctx.JoinTopDown(sum + 2 + i * 3, BorderType.Single);
            }

            widths[widths.Length - 1] = availableWidth - sum;

            ctx.JoinLeftRight(1, BorderType.Single);

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

                    while (n < text.Length && x < ctx.Width)
                        ctx.Put(x++, line, text[n++]);

                    if (x > ctx.Width)
                        break;

                    if (i != widths.Length - 1)
                    {
                        ctx.Put(x++, line, border);
                    }
                }

                return ++line >= ctx.Height;
            }

            if (Row(i => Pad(table.Columns[i], widths[i], table.GetTextAlign(i) == TextAlign.Right), Vertical))
                return;

            if (Row(i => new string(Horizontal, widths[i] + 2), Cross))
                return;

            foreach (var row in data)
            {
                if (Row(i => Pad(row[i], widths[i], table.GetTextAlign(i) == TextAlign.Right), Vertical))
                    return;
            }
        }
    }
}