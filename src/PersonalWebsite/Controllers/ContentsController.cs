using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Services;
using PersonalWebsite.ViewModels.Content;
using System;
using WebsiteContent.Lib;

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
            _humanReadableContentService = humanReadableContentService ?? throw new ArgumentNullException(nameof(humanReadableContentService));
            _languageManipulationService = languageManipulationService ?? throw new ArgumentNullException(nameof(languageManipulationService));
            _pageConfiguration = pageConfiguration ?? throw new ArgumentNullException(nameof(pageConfiguration));
        }

        /// <summary>
        /// Show Human-readable url content.
        /// </summary>
        /// <param name="language">Language version of a content.</param>
        /// <param name="urlName">Human-readable content url path representation.</param>
        /// <returns>Content representation.</returns>
        public IActionResult Show(string language, string urlName)
        {
            LanguageDefinition languageDefinition;
            if (string.IsNullOrWhiteSpace(language))
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
            var pageVM = _humanReadableContentService.GetPageByHumanReadableName(languageDefinition, urlName);
            if (pageVM == null)
            {
                return NotFound();
            }

            return View(pageVM);
        }
    }
}
