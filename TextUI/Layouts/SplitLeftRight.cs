using TextUI.Extensions;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class SplitLeftRight : IRender, IBorderFeedback
    {
        private readonly IRender left;
        private readonly IRender right;
        private readonly BorderType? border;

        public SplitLeftRight(IRender left, IRender right, BorderType? border = null)
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
                feedback.Top[splitAt] = feedback.Bottom[splitAt] = border.Value;

            Feedback feedbackLeft = new Feedback();
            Feedback feedbackRight = new Feedback();

            if (left is IBorderFeedback fb1)
            {
                feedbackLeft = fb1.Render(canvas.Area(0, 0, splitAt, canvas.Height));
                feedback.Apply(feedbackLeft.NotRight());
            }
            else
                left.Render(canvas.Area(0, 0, splitAt, canvas.Height));

            if (right is IBorderFeedback fb2)
            {
                var i = (border.HasValue ? 1 : 0);
                feedbackRight = fb2.Render(canvas.Area(splitAt + i, 0, canvas.Width - splitAt +i, canvas.Height));
                feedback.Apply(feedbackRight.NotLeft(), splitAt + i, 0);
            }
            else
                right.Render(canvas.Area(splitAt, 0, canvas.Width - splitAt, canvas.Height));

            if (border.HasValue)
                for (int i = 0; i < canvas.Height; i++)
                {
                    var c = BoxArt.Get(
                        border.Value,
                        feedbackRight.Left.TryGetValue(i, out var bl) ? bl : BorderType.None,
                        border.Value,
                        feedbackLeft.Right.TryGetValue(i, out var br) ? br : BorderType.None
                    );

                    canvas.Put(splitAt, i, c); ;
                }

            return feedback;
        }
    }
}