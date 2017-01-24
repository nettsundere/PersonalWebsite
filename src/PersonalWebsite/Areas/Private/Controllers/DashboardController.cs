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
        /// Get Index, Render dashboard - main page.
        /// </summary>
        /// <returns>Dashboard.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
