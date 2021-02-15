using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TextUI.Layouts;
using TextUI.Rendering;

namespace TextUI
{
    public static class BoxArt
    {
        private static readonly char[] Cross;

        public static ImmutableDictionary<BorderType, BorderDefinition> Border;

        static BoxArt()
        {
            var nil = BorderType.None;
            var one = BorderType.Single;
            var two = BorderType.Double;

            Cross = new char[81];

            for (var i = 0; i < 81; ++i) Cross[i] = '?';

            Cross[0] = ' ';

            Cross[Ternary(one, one, one, one)] = (char)197;
            Cross[Ternary(two, two, two, two)] = (char)206;
            Cross[Ternary(two, one, two, one)] = (char)215;
            Cross[Ternary(one, two, one, two)] = (char)216;

            Cross[Ternary(two, two, two, one)] = (char)204;
            Cross[Ternary(two, two, one, two)] = (char)202;
            Cross[Ternary(two, one, two, two)] = (char)185;
            Cross[Ternary(one, two, two, two)] = (char)204;

            Cross[Ternary(one, one, one, two)] = (char)204;
            Cross[Ternary(one, one, two, one)] = (char)202;
            Cross[Ternary(one, two, one, one)] = (char)185;
            Cross[Ternary(two, one, one, one)] = (char)204;

            Cross[Ternary(one, one, nil, one)] = (char)193;
            Cross[Ternary(nil, one, one, one)] = (char)194;
            Cross[Ternary(one, nil, one, one)] = (char)180;
            Cross[Ternary(one, one, one, nil)] = (char)195;

            Cross[Ternary(two, one, nil, one)] = (char)208;
            Cross[Ternary(nil, one, two, one)] = (char)210;
            Cross[Ternary(one, nil, one, two)] = (char)181;
            Cross[Ternary(one, two, one, nil)] = (char)198;

            Cross[Ternary(one, two, nil, two)] = (char)207;
            Cross[Ternary(nil, two, one, two)] = (char)209;
            Cross[Ternary(two, nil, two, one)] = (char)182;
            Cross[Ternary(two, one, two, nil)] = (char)199;

            Cross[Ternary(two, two, nil, two)] = (char)202;
            Cross[Ternary(nil, two, two, two)] = (char)203;
            Cross[Ternary(two, nil, two, two)] = (char)185;
            Cross[Ternary(two, two, two, nil)] = (char)204;

            // Double line
            Cross[Ternary(two, nil, two, nil)] = (char)186;
            Cross[Ternary(nil, two, nil, two)] = (char)205;
            Cross[Ternary(two, nil, nil, two)] = (char)188;
            Cross[Ternary(two, two, nil, nil)] = (char)200;
            Cross[Ternary(nil, nil, two, two)] = (char)187;
            Cross[Ternary(nil, two, two, nil)] = (char)201;

            // Single line
            Cross[Ternary(one, nil, one, nil)] = (char)179;
            Cross[Ternary(nil, one, nil, one)] = (char)196;
            Cross[Ternary(one, nil, nil, one)] = (char)217;
            Cross[Ternary(one, one, nil, nil)] = (char)192;
            Cross[Ternary(nil, nil, one, one)] = (char)191;
            Cross[Ternary(nil, one, one, nil)] = (char)218;

            Border = new Dictionary<BorderType, BorderDefinition>
            {
                {
                    BorderType.Single,
                    new BorderDefinition(Get(one, nil), Get(nil, one),
                        Get(nil, one, one, nil),
                        Get(nil, nil, one, one),
                        Get(one, one, nil, nil),
                        Get(one, nil, nil, one))
                },
                {
                    BorderType.Double,
                    new BorderDefinition(Get(two, nil), Get(nil, two),
                        Get(nil, two, two, nil),
                        Get(nil, nil, two, two),
                        Get(two, two, nil, nil),
                        Get(two, nil, nil, two))
                }
            }.ToImmutableDictionary();
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
            return Cross[Ternary(top, right, bottom, left)];
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

            throw new NotSupportedException("Unknown BorderType: " + type);
        }
    }
}