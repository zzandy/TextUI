using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class VerticalSplit : IRender, IBorderFeedback
    {
        private readonly IRender top;
        private readonly IRender bottom;

        public VerticalSplit(IRender top, IRender bottom)
        {
            this.top = top;
            this.bottom = bottom;
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

            if (top is IBorderFeedback)
                feedback.Apply((top as IBorderFeedback).Render(canvas.Area(0, 0, canvas.Width, splitAt)).NotBottom());
            else
                top.Render(canvas.Area(0, 0, canvas.Width, splitAt));

            if (bottom is IBorderFeedback borderFeedback)
                feedback.Apply(borderFeedback.Render(canvas.Area(0, splitAt, canvas.Width, canvas.Height - splitAt)).NotTop(),0,  splitAt);
            else
                bottom.Render(canvas.Area(0, splitAt, canvas.Width, canvas.Height - splitAt));

            return feedback;
        }
    }
}