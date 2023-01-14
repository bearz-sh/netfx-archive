using System.ComponentModel;

namespace Ze.Cli.Docker;

public enum NetworkDriver
{
    [Description("bridge")]
    Bridge,

    [Description("host")]
    Host,

    [Description("macvlan")]
    MacVlan,

    [Description("null")]
    Null,

    [Description("overlay")]
    Overlay,

    [Description("remote")]
    Remote,

    [Description("custom")]
    Custom,
}