using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Std.Text.DotEnv.Serialization;

public class EnvScalarNode : EnvNode
{
    public EnvScalarNode(Mark start, Mark end, ReadOnlySpan<char> value)
        : base(start, end, value)
    {
    }
}