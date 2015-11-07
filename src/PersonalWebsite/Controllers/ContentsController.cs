using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PersonalWebsite.Lib;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;

namespace PersonalWebsite.Controllers
{
    public class ContentsController : Controller
    {
        private ILanguageProcessor _languageProcessor;

        private IContentRepository _contentRepository;

        public ContentsController(ILanguageProcessor languageProcessor, IContentRepository contentRespository)
        {
            _languageProcessor = languageProcessor;
            _contentRepository = contentRespository;
        }

        public IActionResult Show(string language, string urlName)
        {
            var languageDefinition = _languageProcessor.ConvertToDefinition(language);

            var content = _contentRepository.FindByUrlName(languageDefinition, urlName);
            return View();
        }
    }
}
