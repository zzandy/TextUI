using System;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class Frame : IRender
    {
        public static readonly BorderDefinition DoubleBorder = new BorderDefinition
        {
            Type = BorderType.Double,
            Vertical = (char)186,
            Horizontal = (char)205,
            TopLeft = (char)201,
            TopRight = (char)187,
            BottomLeft = (char)200,
            BottomRight = (char)188
        };

        public static readonly BorderDefinition SingleBorder = new BorderDefinition
        {
            Type = BorderType.Single,
            Vertical = (char)179,
            Horizontal = (char)196,
            TopLeft = (char)218,
            TopRight = (char)191,
            BottomLeft = (char)192,
            BottomRight = (char)217
        };

        private readonly BorderDefinition border;
        private readonly bool fill;
        private readonly IRender content;

        public Frame(IRender content, BorderDefinition border)
        {
            fill = content == null;
            this.content = content;
            this.border = border;
        }

        public void Render(ICanvas canvas)
        {
            Feedback feedback = null;

            if (content != null)
                if (content is IBorderFeedback borderFeedback)
                    feedback = borderFeedback.Render(canvas.Area(1, 1, canvas.Width - 2, canvas.Height - 2));
                else
                    content.Render(canvas.Area(1, 1, canvas.Width - 2, canvas.Height - 2));

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

            if (feedback != null)
            {
                var brdr = border.Type;
                const BorderType none = BorderType.None;

                foreach (var pair in feedback.Top)
                    canvas.Put(pair.Key + 1, 0, BoxArt.Get(none, brdr, pair.Value, brdr));

                foreach (var pair in feedback.Bottom)
                    canvas.Put(pair.Key + 1, canvas.Height - 1, BoxArt.Get(pair.Value, brdr, none, brdr));

                foreach (var pair in feedback.Left)
                    canvas.Put(0, pair.Key + 1, BoxArt.Get(brdr, pair.Value, brdr, none));

                foreach (var pair in feedback.Right)
                    canvas.Put(canvas.Width - 1, pair.Key + 1, BoxArt.Get(brdr, none, brdr, pair.Value));
            }
        }
    }
}