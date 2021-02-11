namespace TextUI.Layouts
{
    public sealed class Column<TRow>
    {
        internal readonly string name;
        internal readonly Getter get;
        internal readonly TextAlign align;

        public delegate string Getter(TRow row);

        public Column(string name, Getter getter, TextAlign align = TextAlign.Left)
        {
            this.name = name;
            this.get = getter;
            this.align = align;
        }
    }
}