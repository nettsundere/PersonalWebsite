using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Areas.Private.Controllers;
using Xunit;

namespace PersonalWebsite.Tests.Unit.Areas.Private.Controllers
{
    /// <summary>
    /// Tests for <see cref="DashboardController"/>.
    /// </summary>
    public class DashboardControllerTests
    {
        /// <summary>
        /// Subject of testing.
        /// </summary>
        private readonly DashboardController _subject = new DashboardController();

        [Fact]
        public void ReturnsSuccess()
        {
            var actionResult = _subject.Index();

            Assert.IsType<ViewResult>(actionResult);
        }
    }
}
