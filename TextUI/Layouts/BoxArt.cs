using System;

namespace TextUI.Layouts
{
    public static class BoxArt
    {
        private static readonly char[] cross;

        static BoxArt()
        {
            var none = BorderType.Default;
            var one = BorderType.Single;
            var two = BorderType.Double;


            cross = new char[81];

            for (var i = 0; i < 81; ++i) cross[i] = '?';

            cross[0] = ' ';
            cross[Ternary(one, one, one, one)] = (char)197;
            cross[Ternary(two, two, two, two)] = (char)206;
            cross[Ternary(none, one, none, one)] = (char)196;
        }

        public static char Get(BorderType topBottom, BorderType leftRight)
        {
            return Get(topBottom, leftRight, topBottom, leftRight);
        }

        public static char Get(BorderType top, BorderType leftRight, BorderType bottom)
        {
            return Get(top, leftRight, bottom, leftRight);
        }

        private static char Get(BorderType top, BorderType right, BorderType bottom, BorderType left)
        {
            return cross[Ternary(top, right, bottom, left)];
        }

        private static uint Ternary(BorderType top, BorderType right, BorderType bottom, BorderType left)
        {
            return 3 * 3 * 3 * Ternary(top) + 3 * 3 * Ternary(right) + 3 * Ternary(bottom) + Ternary(left);

        }

        private static uint Ternary(BorderType type)
        {
            switch (type)
            {
                case BorderType.Default: return 0;
                case BorderType.Single: return 1;
                case BorderType.Double: return 2;
            }

            throw new NotSupportedException("Unknown BorderType: " + type.ToString());
        }
    }
}