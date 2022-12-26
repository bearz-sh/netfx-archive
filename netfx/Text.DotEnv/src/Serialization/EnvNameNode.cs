using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bearz.Text.DotEnv.Serialization;

public class EnvNameNode : EnvNode
{
    public EnvNameNode(int lineNumber, int columnNumber, ReadOnlySpan<char> value)
        : base(new Mark(lineNumber, columnNumber), new Mark(lineNumber, columnNumber + value.Length), value)
    {
    }
}