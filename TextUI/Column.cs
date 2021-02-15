using TextUI.Layouts;

namespace TextUI
{
    public sealed class Column<TRow>
    {
        internal readonly string Name;
        internal readonly Getter Get;
        internal readonly TextAlign Align;

        public delegate string Getter(TRow row);

        public Column(string name, Getter getter, TextAlign align = TextAlign.Left)
        {
            Name = name;
            Get = getter;
            Align = align;
        }
    }
}