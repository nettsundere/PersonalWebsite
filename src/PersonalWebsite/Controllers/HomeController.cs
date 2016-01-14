using System;
using Microsoft.AspNet.Mvc;
using PersonalWebsite.ViewModels.Content;
using PersonalWebsite.Services;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHumanReadableContentService _humanReadableContentService;
        private readonly ILanguageManipulationService _languageManipulationService;
        private readonly IPageConfiguration _pageConfiguration;

        public HomeController(
            IPageConfiguration pageConfiguration,
            IHumanReadableContentService humanReadableContentService, 
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
                        return HttpNotFound();
                    }
                }

                pageVM = _humanReadableContentService.GetPageByInternalCaption(
                    languageDefinition, 
                    _pageConfiguration.DefaultPageInternalCaption);
                if (pageVM == null)
                {
                    return HttpNotFound();
                }
            }

            return View(pageVM);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
