using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsite.Areas.Private.Controllers
{
    /// <summary>
    /// Private dashboard controller.
    /// </summary>
    [Authorize]
    [Area(nameof(Private))]
    public class DashboardController : Controller
    {
        /// <summary>
        /// Render list of all content.
        /// </summary>
        /// <returns>List of all content.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
