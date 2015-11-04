using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Lib
{
    public class LanguageProcessor : ILanguageProcessor
    {
        public string[] Supported { get; }

        public string DefaultLanguage { get; }

        public LanguageProcessor()
        {
            DefaultLanguage = LanguageDefinition.en.ToString();
            Supported = Enum.GetNames(typeof(LanguageDefinition));
        }

        /// <summary>
        /// Converts string language representation to <code>LanguageDefinition</code>
        /// </summary>
        /// <param name="language">Language string representation.</param>
        /// <returns>LanguageDefinition</returns>
        public LanguageDefinition ConvertToDefinition(string language)
        {
            if(String.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentException("Null or whitespace", nameof(language));
            }

            var parsedDefinition = Enum.Parse(typeof(LanguageDefinition), language.ToLowerInvariant());
            if(parsedDefinition == null)
                throw new ArgumentException("Wrong language definition", nameof(language));
            else
                return (LanguageDefinition)parsedDefinition;
        }
    }
}
