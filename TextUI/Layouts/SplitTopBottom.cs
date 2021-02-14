using TextUI.Extensions;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class SplitTopBottom : IRender, IBorderFeedback
    {
        private readonly IRender top;
        private readonly IRender bottom;
        private readonly BorderDefinition? borderDefinition;

        public SplitTopBottom(IRender top, IRender bottom, BorderDefinition? borderDefinition = null)
        {
            this.top = top;
            this.bottom = bottom;
            this.borderDefinition = borderDefinition;
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

            if (top is IBorderFeedback fb)
                feedback.Apply(fb.Render(canvas.Area(0, 0, canvas.Width, splitAt)).NotBottom());
            else
                top.Render(canvas.Area(0, 0, canvas.Width, splitAt));

            if (bottom is IBorderFeedback borderFeedback)
                feedback.Apply(borderFeedback.Render(canvas.Area(0, splitAt, canvas.Width, canvas.Height - splitAt)).NotTop(), 0, splitAt);
            else
                bottom.Render(canvas.Area(0, splitAt, canvas.Width, canvas.Height - splitAt));

            return feedback;
        }
    }
}