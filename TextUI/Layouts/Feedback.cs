using System.Collections.Generic;

namespace TextUI.Layouts
{
    public sealed class Feedback
    {
        public Dictionary<int, BorderType> Top { get; } = new Dictionary<int, BorderType>();
        public Dictionary<int, BorderType> Bottom { get; } = new Dictionary<int, BorderType>();
        public Dictionary<int, BorderType> Left { get; } = new Dictionary<int, BorderType>();
        public Dictionary<int, BorderType> Right { get; } = new Dictionary<int, BorderType>();
    }
}