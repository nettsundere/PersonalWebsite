using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.ViewModels.Error;
using Microsoft.AspNetCore.Http;

namespace PersonalWebsite.Controllers
{
    /// <summary>
    /// Errors controller.
    /// </summary>
    [Route("/errors", Order = -1)]
    public class ErrorsController : Controller
    {
        /// <summary>
        /// Fallback status code.
        /// </summary>
        private const int FallbackCode = StatusCodes.Status404NotFound;
        
        /// <summary>
        /// Display an error page for the specific code.
        /// </summary>
        /// <param name="code">Error code.</param>
        [Route("{code:int}")]
        public  ActionResult<ErrorViewModel> Show([Range(400, 599)] int code)
        {
            if (ModelState.IsValid)
            {
                var errorViewModel = new ErrorViewModel(code);
                return View(errorViewModel); 
            }
            else
            {
                var errorViewModel = new ErrorViewModel(FallbackCode);
                Response.StatusCode = FallbackCode;
                return View(errorViewModel);
            }
        }
    }
}
