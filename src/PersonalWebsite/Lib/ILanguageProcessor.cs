namespace PersonalWebsite.Lib
{
    public interface ILanguageProcessor
    {
        string[] Supported { get; }
        LanguageDefinition ConvertToDefinition(string language);
    }
}