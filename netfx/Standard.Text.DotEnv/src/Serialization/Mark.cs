using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Std.Text.DotEnv.Serialization;

public readonly struct Mark
{
    public Mark(int lineNumber, int columnNumber)
    {
        this.LineNumber = lineNumber;
        this.ColumnNumber = columnNumber;
    }

    public int LineNumber { get; }

    public int ColumnNumber { get; }
}