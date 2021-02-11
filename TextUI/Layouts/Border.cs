using System;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class Border : IRender
    {
        public static readonly BorderDefinition DoubleBorder = new BorderDefinition
        {
            Vertical = (char)186,
            Horizontal = (char)205,
            TopLeft = (char)201,
            TopRight = (char)187,
            BottomLeft = (char)200,
            BottomRight = (char)188
        };

        public static readonly BorderDefinition SingleBorder = new BorderDefinition
        {
            Vertical = (char)179,
            Horizontal = (char)196,
            TopLeft = (char)218,
            TopRight = (char)191,
            BottomLeft = (char)192,
            BottomRight = (char)217
        };

        [Flags]
        private enum BorderTeeSpec
        {
            FromSingle = 0,
            ToSingle = 0,
            FromDouble = 0x0001,
            ToDouble = 0x0002,
            Down = 0x0000,
            Up = 0x0004,
            Left = 0x0008,
            Right = Up | Left
        }

        private static readonly char[] x = new char[16];

        static Border()
        {
            x = new char[16];

            x[(int)(BorderTeeSpec.FromSingle | BorderTeeSpec.ToSingle | BorderTeeSpec.Up)] = (char)193;
            x[(int)(BorderTeeSpec.FromSingle | BorderTeeSpec.ToSingle | BorderTeeSpec.Down)] = (char)194;
            x[(int)(BorderTeeSpec.FromSingle | BorderTeeSpec.ToSingle | BorderTeeSpec.Left)] = (char)180;
            x[(int)(BorderTeeSpec.FromSingle | BorderTeeSpec.ToSingle | BorderTeeSpec.Right)] = (char)195;

            x[(int)(BorderTeeSpec.FromSingle | BorderTeeSpec.ToDouble | BorderTeeSpec.Up)] = (char)208;
            x[(int)(BorderTeeSpec.FromSingle | BorderTeeSpec.ToDouble | BorderTeeSpec.Down)] = (char)210;
            x[(int)(BorderTeeSpec.FromSingle | BorderTeeSpec.ToDouble | BorderTeeSpec.Left)] = (char)181;
            x[(int)(BorderTeeSpec.FromSingle | BorderTeeSpec.ToDouble | BorderTeeSpec.Right)] = (char)198;

            x[(int)(BorderTeeSpec.FromDouble | BorderTeeSpec.ToSingle | BorderTeeSpec.Up)] = (char)207;
            x[(int)(BorderTeeSpec.FromDouble | BorderTeeSpec.ToSingle | BorderTeeSpec.Down)] = (char)209;
            x[(int)(BorderTeeSpec.FromDouble | BorderTeeSpec.ToSingle | BorderTeeSpec.Left)] = (char)182;
            x[(int)(BorderTeeSpec.FromDouble | BorderTeeSpec.ToSingle | BorderTeeSpec.Right)] = (char)199;

            x[(int)(BorderTeeSpec.FromDouble | BorderTeeSpec.ToDouble | BorderTeeSpec.Up)] = (char)202;
            x[(int)(BorderTeeSpec.FromDouble | BorderTeeSpec.ToDouble | BorderTeeSpec.Down)] = (char)203;
            x[(int)(BorderTeeSpec.FromDouble | BorderTeeSpec.ToDouble | BorderTeeSpec.Left)] = (char)185;
            x[(int)(BorderTeeSpec.FromDouble | BorderTeeSpec.ToDouble | BorderTeeSpec.Right)] = (char)204;
        }

        private readonly BorderDefinition border;
        private readonly bool fill;
        private readonly IRender content;

        public Border(IRender content, BorderDefinition border)
        {
            fill = content == null;
            this.content = content;
            this.border = border;
        }

        public void Render(ICanvas canvas)
        {
            for (var i = 0; i < canvas.Height; ++i)
            {
                for (var j = 0; j < canvas.Width; ++j)
                {
                    bool top = i == 0;
                    bool bottom = i == canvas.Height - 1;
                    bool left = j == 0;
                    bool right = j == canvas.Width - 1;

                    if (top)
                    {
                        if (left) canvas.Put(j, i, border.TopLeft);
                        else if (right) canvas.Put(j, i, border.TopRight);
                        else canvas.Put(j, i, border.Horizontal);
                    }

                    else if (bottom)
                    {
                        if (left) canvas.Put(j, i, border.BottomLeft);
                        else if (right) canvas.Put(j, i, border.BottomRight);
                        else canvas.Put(j, i, border.Horizontal);
                    }

                    else
                    {
                        if (left || right) canvas.Put(j, i, border.Vertical);
                        else if (fill) canvas.Put(j, i, ' ');
                    }
                }
            }

            if (content != null)
            {
                if (content is IBorderFeedback)
                {
                    var feedback = (content as IBorderFeedback).Render(canvas.Area(1, 1, canvas.Width - 2, canvas.Height - 2));

                    foreach (var pair in feedback.Top)
                    {
                        var c = GetTeeChar(pair.Value, BorderTeeSpec.Down);

                        canvas.Put(pair.Key, 0, c);
                    }

                    foreach (var pair in feedback.Bottom)
                    {
                        var c = GetTeeChar(pair.Value, BorderTeeSpec.Up);

                        canvas.Put(pair.Key, canvas.Height - 1, c);
                    }

                    foreach (var pair in feedback.Left)
                    {
                        var c = GetTeeChar(pair.Value, BorderTeeSpec.Right);

                        canvas.Put(0, pair.Key, c);
                    }

                    foreach (var pair in feedback.Right)
                    {
                        var c = GetTeeChar(pair.Value, BorderTeeSpec.Left);

                        canvas.Put(canvas.Width - 1, pair.Key, c);
                    }
                }
                else
                    content.Render(canvas.Area(1, 1, canvas.Width - 2, canvas.Height - 2));
            }
        }

        private char GetTeeChar(BorderType connection, BorderTeeSpec dir)
        {
            var to = connection == BorderType.Double ? BorderTeeSpec.ToDouble : BorderTeeSpec.ToSingle;

            var from = Object.Equals(border, SingleBorder) ? BorderTeeSpec.FromSingle : BorderTeeSpec.FromDouble;

            if (connection == BorderType.Default)
                to = from;


            return x[(int)(from | to | dir)];
        }
    }
}