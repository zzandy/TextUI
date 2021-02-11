namespace TextUI.Interfaces
{
    public interface ICanvas
    {
        int Width { get; }
        int Height { get; }

        void Put(int x, int y, char character);
    }
}