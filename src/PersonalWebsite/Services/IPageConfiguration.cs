using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Services
{
    public interface IPageConfiguration
    {
        LanguageDefinition DefaultLanguage { get; }
        string DefaultPageInternalCaption { get; }
    }
}
