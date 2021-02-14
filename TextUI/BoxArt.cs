using System;

namespace TextUI.Layouts
{
    public static class BoxArt
    {
        private static readonly char[] cross;

        static BoxArt()
        {
            var none = BorderType.None;
            var one = BorderType.Single;
            var two = BorderType.Double;

            cross = new char[81];

            for (var i = 0; i < 81; ++i) cross[i] = '?';

            cross[0] = ' ';

            cross[Ternary(one, one, one, one)] = (char)197;
            cross[Ternary(two, two, two, two)] = (char)206;
            cross[Ternary(two, one, two, one)] = (char)215;
            cross[Ternary(one, two, one, two)] = (char)216;

            cross[Ternary(one, one, none, one)] = (char)193;
            cross[Ternary(none, one, one, one)] = (char)194;
            cross[Ternary(one, none, one, one)] = (char)180;
            cross[Ternary(one, one, one, none)] = (char)195;

            cross[Ternary(two, one, none, one)] = (char)208;
            cross[Ternary(none, one, two, one)] = (char)210;
            cross[Ternary(one, none, one, two)] = (char)181;
            cross[Ternary(one, two, one, none)] = (char)198;

            cross[Ternary(one, two, none, two)] = (char)207;
            cross[Ternary(none, two, one, two)] = (char)209;
            cross[Ternary(two, none, two, one)] = (char)182;
            cross[Ternary(two, one, two, none)] = (char)199;

            cross[Ternary(two, two, none, two)] = (char)202;
            cross[Ternary(none, two, two, two)] = (char)203;
            cross[Ternary(two, none, two, two)] = (char)185;
            cross[Ternary(two, two, two, none)] = (char)204;

            // Double line
            cross[Ternary(two, none, two, none)] = (char)186;
            cross[Ternary(none, two, none, two)] = (char)205;
            cross[Ternary(two, none, none, two)] = (char)201;
            cross[Ternary(two, two, none, none)] = (char)187;
            cross[Ternary(none, none, two, two)] = (char)200;
            cross[Ternary(none, two, two, none)] = (char)188;

            // Single line
            cross[Ternary(one, none, one, none)] = (char)179;
            cross[Ternary(none, one, none, one)] = (char)196;
            cross[Ternary(one, none, none, one)] = (char)218;
            cross[Ternary(one, one, none, none)] = (char)191;
            cross[Ternary(none, none, one, one)] = (char)192;
            cross[Ternary(none, one, one, none)] = (char)217;
        }

        public static char Get(BorderType topBottom, BorderType leftRight)
        {
            return Get(topBottom, leftRight, topBottom, leftRight);
        }

        public static char Get(BorderType top, BorderType leftRight, BorderType bottom)
        {
            return Get(top, leftRight, bottom, leftRight);
        }

        public static char Get(BorderType top, BorderType right, BorderType bottom, BorderType left)
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
                case BorderType.None: return 0;
                case BorderType.Single: return 1;
                case BorderType.Double: return 2;
            }

            throw new NotSupportedException("Unknown BorderType: " + type.ToString());
        }
    }
}