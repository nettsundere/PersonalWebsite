using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PersonalWebsite.Lib;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;
using PersonalWebsite.ViewModels.Content;
using PersonalWebsite.Services;

namespace PersonalWebsite.Controllers
{
    public class ContentsController : Controller
    {
        private IHumanReadableContentService _humanReadableContentService;
        private ILanguageManipulationService _languageManipulationService;

        public ContentsController(
            IHumanReadableContentService humanReadableContentService,
            ILanguageManipulationService languageManipulationService) {
            if(humanReadableContentService == null)
            {
                throw new ArgumentNullException(nameof(humanReadableContentService));
            }

            if(languageManipulationService == null)
            {
                throw new ArgumentNullException(nameof(languageManipulationService));
            }

            _humanReadableContentService = humanReadableContentService;
            _languageManipulationService = languageManipulationService;
        }

        /// <summary>
        /// Show Human-readable url content.
        /// </summary>
        /// <param name="language">Language version of a content.</param>
        /// <param name="urlName">Human-readable content url path representation.</param>
        /// <returns>Content representation.</returns>
        public IActionResult Show(string language, string urlName)
        {
            PageViewModel pageVM;
            using (_humanReadableContentService)
            {
                LanguageDefinition languageDefinition;
                if (String.IsNullOrWhiteSpace(language))
                {
                    languageDefinition = _languageManipulationService.DefaultLanguageDefinition;
                }
                else
                {
                    try
                    {
                        languageDefinition = _languageManipulationService.LanguageRepresentationToLanguageDefinition(language);
                    }
                    catch
                    {
                        return HttpNotFound();
                    }
                }

                _languageManipulationService.SetLanguage(languageDefinition);

                pageVM = _humanReadableContentService.GetPageByHumanReadableName(languageDefinition, urlName);
                if (pageVM == null)
                {
                    return HttpNotFound();
                }
            }

            return View(pageVM);
        }
    }
}
