using System.Collections.Generic;

namespace TextUI.Layouts
{
    public interface ITable
    {
        IEnumerable<string[]> Rows { get; }
        string[] Columns { get; }

        TextAlign GetTextAlign(int i);
    }
}