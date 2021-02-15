using System;

namespace TextUI
{
    [Flags]
    public enum Side
    {
        None = 0,
        Top = 1,
        Left = 2,
        Bottom = 4,
        Right = 8,
        NotTop = Left | Right | Bottom,
        NotLeft = Top | Bottom | Right,
        NotBottom = Top | Left | Right,
        NotRight = Top | Left | Bottom,
        All = Top | Left | Bottom | Right
    }
}