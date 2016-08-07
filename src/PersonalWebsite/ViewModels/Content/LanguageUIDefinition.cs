using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Language definition wrapper.
    /// </summary>
    public struct LanguageUIDefinition
    {
        /// <summary>
        /// Language definition to be presented.
        /// </summary>
        public LanguageDefinition Language { get; }

        /// <summary>
        /// Language UI representation.
        /// </summary>
        public string Header { get; }

        /// <summary>
        /// Create language UI presentation.
        /// </summary>
        /// <param name="language">The language to be presented</param>
        /// <param name="header">The header for a language value</param>
        public LanguageUIDefinition(LanguageDefinition language, string header)
        {
            if(String.IsNullOrWhiteSpace(header))
            {
                throw new ArgumentException("Language header cannot be empty");
            }

            Language = language;
            Header = header;
        }
    }
}
