using System;
using TextUI.Extensions;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class Frame : IRender
    {
        private readonly bool fill;
        private readonly IRender content;
        private readonly BorderDefinition border;
        private readonly BorderType borderType;

        public Frame(IRender content, BorderType border)
        {
            fill = content == null;
            this.content = content;
            this.borderType = border;
            this.border = BoxArt.Border[border];
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
                        if (left) canvas.Put(j, i, border.TopLeftCorner);
                        else if (right) canvas.Put(j, i, border.TopRightCorner);
                        else canvas.Put(j, i, border.Horizontal);
                    }
                    else if (bottom)
                    {
                        if (left) canvas.Put(j, i, border.BottomLeftCorner);
                        else if (right) canvas.Put(j, i, border.BottomRightCorner);
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
                const BorderType none = BorderType.None;

                foreach (var pair in feedback.Top)
                    canvas.Put(pair.Key + 1, 0, BoxArt.Get(none, borderType, pair.Value, borderType));

                foreach (var pair in feedback.Bottom)
                    canvas.Put(pair.Key + 1, canvas.Height - 1, BoxArt.Get(pair.Value, borderType, none, borderType));

                foreach (var pair in feedback.Left)
                    canvas.Put(0, pair.Key + 1, BoxArt.Get(borderType, pair.Value, borderType, none));

                foreach (var pair in feedback.Right)
                    canvas.Put(canvas.Width - 1, pair.Key + 1, BoxArt.Get(borderType, none, borderType, pair.Value));
            }
        }
    }
}