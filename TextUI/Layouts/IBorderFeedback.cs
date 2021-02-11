using TextUI.Interfaces;

namespace TextUI.Layouts
{
    public interface IBorderFeedback
    {
        Feedback Render(ICanvas canvas);
    }
}