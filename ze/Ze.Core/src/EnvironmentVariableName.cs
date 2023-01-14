using Bearz.Std;

namespace Ze;

/// <summary>
/// The name of an environment variable. This struct ensures that the name is coerced to uppercase
/// only once.
/// </summary>
public readonly struct EnvironmentVariableName
{
    private readonly string name;

    public EnvironmentVariableName(ReadOnlySpan<char> name)
    {
        this.name = name.ToEnvVarName();
    }

    public static implicit operator EnvironmentVariableName(string name) => new EnvironmentVariableName(name);

    public static implicit operator string(EnvironmentVariableName variableName) => variableName.name;

    public override string ToString()
    {
        return this.name;
    }
}