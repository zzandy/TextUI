using TextUI.Extensions;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class SplitLeftRight : IRender, IBorderFeedback
    {
        private readonly IRender left;
        private readonly IRender right;
        private readonly BorderDefinition? border;

        public SplitLeftRight(IRender left, IRender right, BorderDefinition? border = null)
        {
            this.left = left;
            this.right = right;
            this.border = border;
        }

        public void Render(ICanvas canvas)
        {
            ((IBorderFeedback)this).Render(canvas);
        }

        Feedback IBorderFeedback.Render(ICanvas canvas)
        {
            var feedback = new Feedback();

            var splitAt = canvas.Width / 2;
            if (border.HasValue)
                feedback.Top[splitAt - 1] = feedback.Bottom[splitAt - 1] = border.Value.Type;

            Feedback feedbackLeft = new Feedback();
            Feedback feedbackRight = new Feedback();

            if (left is IBorderFeedback fb1)
            {
                feedbackLeft = fb1.Render(canvas.Area(0, 0, splitAt - (border.HasValue ? 1 : 0), canvas.Height));
                feedback.Apply(feedbackLeft.NotRight());
            }
            else
                left.Render(canvas.Area(0, 0, splitAt, canvas.Height));

            if (right is IBorderFeedback fb2)
            {
                feedbackRight = fb2.Render(canvas.Area(splitAt, 0, canvas.Width - splitAt, canvas.Height));
                feedback.Apply(feedbackRight.NotLeft(), splitAt, 0);
            }
            else
                right.Render(canvas.Area(splitAt, 0, canvas.Width - splitAt, canvas.Height));

            if (border.HasValue)
                for (int i = 0; i < canvas.Height; i++)
                {
                    var c = BoxArt.Get(border.Value.Type,
                        feedbackRight.Left.TryGetValue(i, out var bl) ? bl : BorderType.None,
                        border.Value.Type,
                        feedbackLeft.Right.TryGetValue(i, out var br) ? br : BorderType.None
                    );

                    canvas.Put(splitAt - 1, i, c); ;
                }

            return feedback;
        }
    }
}