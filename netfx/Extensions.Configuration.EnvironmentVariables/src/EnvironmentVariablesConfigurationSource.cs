using Microsoft.Extensions.Configuration;

namespace Bearz.Extensions.Configuration.EnvironmentVariables;

/// <summary>
/// Represents environment variables as an <see cref="IConfigurationSource"/>.
/// </summary>
public class EnvironmentVariablesConfigurationSource : IConfigurationSource
{
    /// <summary>
    /// Gets or sets the prefix used to filter environment variables.
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Gets or sets the delimiter used to separate configuration keys from environment variables.
    /// </summary>
    public string Delimiter { get; set; } = "__";

    /// <summary>
    /// Builds the <see cref="EnvironmentVariablesConfigurationProvider"/> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
    /// <returns>A <see cref="EnvironmentVariablesConfigurationProvider"/>.</returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new EnvironmentVariablesConfigurationProvider(this.Prefix, this.Delimiter);
    }
}