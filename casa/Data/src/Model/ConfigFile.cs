namespace Casa.Data.Model;

public class ConfigFile
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? CheckSum { get; set; }
}