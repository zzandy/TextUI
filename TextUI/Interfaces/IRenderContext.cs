using TextUI.Layouts;

namespace TextUI.Interfaces
{
    public interface IRenderContext
    {
        int Width { get; }
        int Height { get; }

        void Put(int x, int y, char character);

        void JoinTop(int x, BorderType border);
        void JoinBottom(int x, BorderType border);
        void JoinLeft(int y, BorderType border);
        void JoinRight(int y, BorderType border);

        BorderType GetJoinTop(int x);
        BorderType GetJoinBottom(int x);
        BorderType GetJoinRight(int y);
        BorderType GetJoinLeft(int y);
    }
}