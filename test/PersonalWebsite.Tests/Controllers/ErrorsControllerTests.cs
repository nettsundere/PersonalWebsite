using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Controllers;
using PersonalWebsite.ViewModels.Error;
using Xunit;

namespace PersonalWebsite.Tests.Controllers
{
    public class ErrorsControllerTests
    {
        private readonly ErrorsController _subject = new ErrorsController();
        
        [Theory]
        [InlineData(400)]
        [InlineData(404)]
        [InlineData(500)]
        [InlineData(599)]
        public void ReturnsContentForKnownErrorCodes(int errorCode)
        {
            var result = _subject.Show(errorCode).Result;
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult) result;

            Assert.Equal(errorCode, ((ErrorViewModel) viewResult.Model).StatusCode);
        }
    }
}
