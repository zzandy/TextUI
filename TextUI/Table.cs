using System.Collections.Generic;
using System.Linq;
using TextUI.Interfaces;
using TextUI.Layouts;

namespace TextUI
{
    public sealed class Table<TRow> : ITable
    {
        private readonly Column<TRow>[] columns;
        private readonly IEnumerable<TRow> data;

        public Table(IEnumerable<TRow> data, params Column<TRow>[] columns)
        {
            this.columns = columns;
            this.data = data;
        }

        public IEnumerable<string[]> Rows => data.Select(row => columns.Select(col => col.Get(row)).ToArray());
        public string[] Columns => columns.Select(col => col.Name).ToArray();

        public TextAlign GetTextAlign(int i)
        {
            return columns[i].Align;
        }
    }
}