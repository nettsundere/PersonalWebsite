using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Lib;
using PersonalWebsite.Services;
using PersonalWebsite.ViewModels.Content;
using System;

namespace PersonalWebsite.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Human-readable content service.
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
        /// Create <see cref="HomeController"/>.
        /// </summary>
        /// <param name="pageConfiguration">Page configuration.</param>
        /// <param name="humanReadableContentService">Human-readable content retrieval service.</param>
        /// <param name="languageManipulationService">Language manipulation service.</param>
        public HomeController(
            IPageConfiguration pageConfiguration,
            IHumanReadableContentRetrievalService humanReadableContentService, 
            ILanguageManipulationService languageManipulationService) 
        {
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

            _pageConfiguration = pageConfiguration;
            _humanReadableContentService = humanReadableContentService;
            _languageManipulationService = languageManipulationService;
        }

        /// <summary>
        /// Index route.
        /// </summary>
        /// <param name="language">language</param>
        /// <returns>Index page.</returns>
        public IActionResult Index(string language)
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

                pageVM = _humanReadableContentService.GetPageByInternalCaption(
                    languageDefinition, 
                    _pageConfiguration.DefaultPageInternalCaption);
                if (pageVM == null)
                {
                    return NotFound();
                }
            }

            return View(pageVM);
        }
    }
}
