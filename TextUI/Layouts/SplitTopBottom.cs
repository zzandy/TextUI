﻿using System.Data;
using TextUI.Extensions;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class SplitTopBottom : IRender, IBorderFeedback
    {
        private readonly IRender top;
        private readonly IRender bottom;
        private readonly BorderType? border;

        public SplitTopBottom(IRender top, IRender bottom, BorderType? border = null)
        {
            this.top = top;
            this.bottom = bottom;
            this.border = border;
        }

        public void Render(ICanvas canvas)
        {
            ((IBorderFeedback)this).Render(canvas);
        }

        Feedback IBorderFeedback.Render(ICanvas canvas)
        {
            var splitAt = canvas.Height / 2;

            if (top is ILayout layout)
            {
                splitAt = layout.DesiredHeight;
            }

            var feedback = new Feedback();

            if (border.HasValue)
                feedback.Left[splitAt] = feedback.Right[splitAt] = border.Value;

            Feedback feedbackTop = new Feedback();
            Feedback feedbackBottom = new Feedback();

            if (top is IBorderFeedback fb1)
            {
                feedbackTop = fb1.Render(canvas.Area(0, 0, canvas.Width, splitAt));
                feedback.Apply(feedbackTop.NotBottom());
            }
            else
                top.Render(canvas.Area(0, 0, canvas.Width, splitAt));


            if (bottom is IBorderFeedback fb2)
            {
                var i = (border.HasValue ? 1 : 0);
                feedbackBottom = fb2.Render(canvas.Area(0, splitAt + i, canvas.Width, canvas.Height - splitAt));
                feedback.Apply(feedbackBottom.NotTop(), 0, splitAt + i);
            }
            else
                bottom.Render(canvas.Area(0, splitAt, canvas.Width, canvas.Height - splitAt));

            if (border.HasValue)
                for (int i = 0; i < canvas.Width; i++)
                {
                    var c = BoxArt.Get(
                        feedbackBottom.Top.TryGetValue(i, out var bl) ? bl : BorderType.None,
                        border.Value,
                        feedbackTop.Bottom.TryGetValue(i, out var br) ? br : BorderType.None,
                        border.Value
                    );

                    canvas.Put(i, splitAt, c); ;
                }

            return feedback;
        }
    }
}