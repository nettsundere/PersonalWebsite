using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PersonalWebsite.Lib;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;
using PersonalWebsite.ViewModels.Content;

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

        /// <summary>
        /// Show Human-readable url content.
        /// </summary>
        /// <param name="language">Language version of a content.</param>
        /// <param name="urlName">Human-readable content url path representation.</param>
        /// <returns>Content representation.</returns>
        public IActionResult Show(string language, string urlName)
        {
            var languageDefinition = _languageProcessor.ConvertToDefinition(language);

            ContentViewModel cvm;
            using(_contentRepository)
            {
                cvm = _contentRepository.FindTranslatedContent(languageDefinition, urlName);
            }

            if(cvm == null)
            {
                return new HttpNotFoundResult();
            }

            return View(cvm);
        }
    }
}
