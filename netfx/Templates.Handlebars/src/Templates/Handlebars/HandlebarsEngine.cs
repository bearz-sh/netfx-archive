using Bearz.Extensions.Secrets;

using HandlebarsDotNet;

using Microsoft.Extensions.Configuration;

namespace Bearz.Templates.Handlebars;

public class HandlebarsEngine : ITemplateEngine
{
    private readonly IHandlebars hb;

    public HandlebarsEngine(
        HandlebarsConfiguration? options = null,
        IConfiguration? configuration = null,
        ISecretsVault? vault = null)
    {
        this.hb = HandlebarsDotNet.Handlebars.Create(options);
        if (configuration is not null)
            this.hb.RegisterConfHelpers(configuration);

        if (vault is not null)
            this.hb.RegisterSecretHelpers(vault);

        this.hb.RegisterEnvHelpers();
        this.hb.RegisterJsonHelpers();
        this.hb.RegisterRegexHelpers();
        this.hb.RegisterDateTimeHelpers();
        this.hb.RegisterStringHelpers();
    }

    public string Render(string template, object model)
    {
        var hbs = this.hb.Compile(template);
        return hbs(model);
    }

    public void RenderFile(string templateFile, object model, string outputFile)
    {
        using var fs = File.OpenText(templateFile);
        using var sw = File.CreateText(outputFile);
        var hbs = this.hb.Compile(fs);
        hbs(sw, model);
    }
}