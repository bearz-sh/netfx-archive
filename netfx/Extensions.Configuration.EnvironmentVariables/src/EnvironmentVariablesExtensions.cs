using Bearz.Extensions.Configuration.EnvironmentVariables;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration;

/// <summary>
/// Extension methods for registering <see cref="EnvironmentVariablesConfigurationProvider"/> with <see cref="IConfigurationBuilder"/>.
/// </summary>
public static class EnvironmentVariablesExtensions
{
    /// <summary>
    /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables.
    /// </summary>
    /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddBearzEnvironmentVariables(this IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Add(new EnvironmentVariablesConfigurationSource());
        return configurationBuilder;
    }

    /// <summary>
    /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables
    /// with a specified prefix.
    /// </summary>
    /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="prefix">The prefix that environment variable names must start with. The prefix will be removed from the environment variable names.</param>
    /// <param name="delimiter">The delimiter used to parse the environment variables into configuration entries.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddBearzEnvironmentVariables(
        this IConfigurationBuilder configurationBuilder,
        string? prefix,
        string delimiter = "__")
    {
        configurationBuilder.Add(new EnvironmentVariablesConfigurationSource { Prefix = prefix, Delimiter = delimiter });
        return configurationBuilder;
    }

    /// <summary>
    /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configureSource">Configures the source.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddBearzEnvironmentVariables(
        this IConfigurationBuilder builder,
        Action<EnvironmentVariablesConfigurationSource>? configureSource)
        => builder.Add(configureSource);
}