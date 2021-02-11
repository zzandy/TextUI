using System.Collections.Generic;
using TextUI.Interfaces;
using TextUI.Rendering;

namespace TextUI.Layouts
{
    public sealed class HorizontalSplit : IRender, IBorderFeedback
    {
        private readonly IRender left;
        private readonly IRender right;

        public HorizontalSplit(IRender left, IRender right)
        {
            this.left = left;
            this.right = right;
        }

        public void Render(ICanvas canvas)
        {
            ((IBorderFeedback)this).Render(canvas);
        }

        Feedback IBorderFeedback.Render(ICanvas canvas)
        {
            var feedback = new Feedback();

            var splitAt = canvas.Width / 2;

            if (left is IBorderFeedback)
                feedback.Apply((left as IBorderFeedback).Render(canvas.Area(0, 0, splitAt, canvas.Height)).NotRight());
            else
                left.Render(canvas.Area(0, 0, splitAt, canvas.Height));

            if (right is IBorderFeedback)
                feedback.Apply((right as IBorderFeedback).Render(canvas.Area(splitAt, 0, canvas.Width - splitAt, canvas.Height)).NotLeft(), splitAt, 0);
            else
                right.Render(canvas.Area(splitAt, 0, canvas.Width - splitAt, canvas.Height));

            return feedback;
        }
    }

    public static class FeedbackExtensions
    {
        private static void CopyTo(this Dictionary<int, BorderType> source, Dictionary<int, BorderType> target, int offset)
        {
            foreach (var kvsrc in source)
            {
                target[kvsrc.Key + offset] = kvsrc.Value;
            }
        }

        public static Feedback Apply(this Feedback fb, Feedback other, int dx, int dy)
        {
            other.Top.CopyTo(fb.Top, dx);
            other.Bottom.CopyTo(fb.Bottom, dx);
            other.Left.CopyTo(fb.Left, dy);
            other.Right.CopyTo(fb.Right, dy);

            return fb;
        }

        public static Feedback Apply(this Feedback fb, Feedback other)
        {
            return fb.Apply(other, 0, 0);
        }

        public static Feedback NotRight(this Feedback b)
        {
            var fb = new Feedback();

            b.Top.CopyTo(fb.Top, 0);
            b.Bottom.CopyTo(fb.Bottom, 0);
            b.Left.CopyTo(fb.Left, 0);

            return fb;
        }


        public static Feedback NotLeft(this Feedback b)
        {
            var fb = new Feedback();

            b.Top.CopyTo(fb.Top, 0);
            b.Right.CopyTo(fb.Right, 0);
            b.Bottom.CopyTo(fb.Bottom, 0);

            return fb;
        }

        public static Feedback NotTop(this Feedback b)
        {
            var fb = new Feedback();

            b.Right.CopyTo(fb.Right, 0);
            b.Bottom.CopyTo(fb.Bottom, 0);
            b.Left.CopyTo(fb.Left, 0);

            return fb;
        }


        public static Feedback NotBottom(this Feedback b)
        {
            var fb = new Feedback();

            b.Top.CopyTo(fb.Top, 0);
            b.Right.CopyTo(fb.Right, 0);
            b.Left.CopyTo(fb.Left, 0);

            return fb;
        }
    }
}