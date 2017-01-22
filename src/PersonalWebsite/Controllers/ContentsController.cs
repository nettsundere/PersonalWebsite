using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Lib;
using PersonalWebsite.Services;
using PersonalWebsite.ViewModels.Content;
using System;

namespace PersonalWebsite.Controllers
{
    /// <summary>
    /// Content retrieval controller.
    /// </summary>
    public class ContentsController : Controller
    {
        /// <summary>
        /// Human-readable content retrieval service.
        /// </summary>
        private readonly IHumanReadableContentRetrievalService _humanReadableContentService;

        /// <summary>
        /// Language manipulation service.
        /// </summary>
        private readonly ILanguageManipulationService _languageManipulationService;

        /// <summary>
        /// Page configuration.
        /// </summary>
        private readonly IPageConfiguration _pageConfiguration;

        /// <summary>
        /// Create <see cref="ContentsController"/>.
        /// </summary>
        /// <param name="pageConfiguration">Page configuration.</param>
        /// <param name="humanReadableContentService">Human-readable content retrieval service.</param>
        /// <param name="languageManipulationService">Language manipulation service.</param>
        public ContentsController(
            IPageConfiguration pageConfiguration,
            IHumanReadableContentRetrievalService humanReadableContentService,
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
