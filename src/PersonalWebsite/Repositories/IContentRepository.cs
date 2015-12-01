using Microsoft.Data.Entity;
using PersonalWebsite.Lib;
using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.ViewModels.Content;

namespace PersonalWebsite.Repositories
{
    public interface IContentRepository : IDisposable
    {
        ContentViewModel FindTranslatedContent(LanguageDefinition langDefinition, string urlName); 
    }
}
