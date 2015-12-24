using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PersonalWebsite.ViewModels.Content;
using PersonalWebsite.Services;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private IHumanReadableContentService _humanReadableContentService;
        private ILanguageManipulationService _languageManipulationService;

        public HomeController(
            IHumanReadableContentService humanReadableContentService, 
            ILanguageManipulationService languageManipulationService) 
        {
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

        public IActionResult Index(string language)
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

                pageVM = _humanReadableContentService.GetPageByInternalCaption(languageDefinition, PredefinedPages.Welcome.ToString());
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
