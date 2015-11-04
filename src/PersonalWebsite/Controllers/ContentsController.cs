using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Controllers
{
    public class ContentsController : Controller
    {
        private ILanguageProcessor _languageProcessor;

        public ContentsController(ILanguageProcessor languageProcessor)
        {
            _languageProcessor = languageProcessor;
        }

        public IActionResult Show(string language, string urlName)
        {
            var languageDefinition = _languageProcessor.ConvertToDefinition(language);
            return View();
        }
    }
}
