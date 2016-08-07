using System;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Lib;
using PersonalWebsite.ViewModels.Content;
using PersonalWebsite.Services;

namespace PersonalWebsite.Controllers
{
    public class ContentsController : Controller
    {
        private readonly IHumanReadableContentService _humanReadableContentService;
        private readonly ILanguageManipulationService _languageManipulationService;
        private readonly IPageConfiguration _pageConfiguration;

        public ContentsController(
            IPageConfiguration pageConfiguration,
            IHumanReadableContentService humanReadableContentService,
            ILanguageManipulationService languageManipulationService) {

            if(pageConfiguration == null)
            {
                throw new ArgumentNullException(nameof(pageConfiguration));
            }

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
            _pageConfiguration = pageConfiguration;
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
                    languageDefinition = _pageConfiguration.DefaultLanguage;
                }
                else
                {
                    try
                    {
                        languageDefinition = _languageManipulationService.LanguageRepresentationToLanguageDefinition(language);
                    }
                    catch
                    {
                        return NotFound();
                    }
                }

                pageVM = _humanReadableContentService.GetPageByHumanReadableName(languageDefinition, urlName);
                if (pageVM == null)
                {
                    return NotFound();
                }
            }

            return View(pageVM);
        }
    }
}
