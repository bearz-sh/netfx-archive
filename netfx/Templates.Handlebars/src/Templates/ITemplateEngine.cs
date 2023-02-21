namespace Bearz.Templates;

public interface ITemplateEngine
{
    string Render(string template, object model);

    void RenderFile(string templateFile, object model, string outputFile);
}