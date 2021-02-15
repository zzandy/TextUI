using TextUI.Layouts;

namespace TextUI.Rendering
{
    public readonly struct BorderDefinition
    {
        public readonly char Vertical;
        public readonly char Horizontal;
        public readonly char TopLeftCorner;
        public readonly char TopRightCorner;
        public readonly char BottomLeftCorner;
        public readonly char BottomRightCorner;

        public BorderDefinition(char vertical, char horizontal, char topLeftCorner, char topRightCorner, char bottomLeftCorner, char bottomRightCorner)
        {
            Vertical = vertical;
            Horizontal = horizontal;
            TopLeftCorner = topLeftCorner;
            TopRightCorner = topRightCorner;
            BottomLeftCorner = bottomLeftCorner;
            BottomRightCorner = bottomRightCorner;
        }
    }
}