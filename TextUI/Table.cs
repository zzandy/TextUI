using System;
using System.Collections.Generic;
using System.Linq;

namespace TextUI.Layouts
{
    public sealed class Table<TRow> : ITable
    {
        public Column<TRow>[] columns;
        private readonly IEnumerable<TRow> data;

        public Table(IEnumerable<TRow> data, params Column<TRow>[] columns)
        {
            this.columns = columns;
            this.data = data;
        }

        public IEnumerable<string[]> Rows => data.Select(row => columns.Select(col => col.get(row)).ToArray());
        public string[] Columns => columns.Select(col => col.name).ToArray();

        public TextAlign GetTextAlign(int i)
        {
            return columns[i].align;
        }
    }
}