using Microsoft.Data.Entity;
using PersonalWebsite.Lib;
using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Repositories
{
    public interface IContentRepository : IDisposable
    {
        Content FindByUrlName(LanguageDefinition langDefinition, string urlName); 
    }
}
