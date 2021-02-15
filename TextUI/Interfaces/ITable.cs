using System.Collections.Generic;
using TextUI.Layouts;

namespace TextUI.Interfaces
{
    public interface ITable
    {
        IEnumerable<string[]> Rows { get; }
        string[] Columns { get; }

        TextAlign GetTextAlign(int i);
    }
}