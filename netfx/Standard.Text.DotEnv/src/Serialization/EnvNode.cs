using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Std.Text.DotEnv.Serialization;

public class EnvNode
{
    public EnvNode(Mark start, Mark end, ReadOnlySpan<char> value)
    {
        this.Start = start;
        this.End = end;
        this.Value = new ReadOnlyMemory<char>(value.ToArray());
    }

    public Mark Start { get; }

    public Mark End { get; }

    public ReadOnlyMemory<char> Value { get; }
}