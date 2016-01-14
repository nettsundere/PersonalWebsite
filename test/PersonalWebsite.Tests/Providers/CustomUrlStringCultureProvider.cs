using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Localization;
using PersonalWebsite.Providers;
using PersonalWebsite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonalWebsite.Tests.Providers
{
    public class CustomUrlStringCultureProviderTest
    {
        private readonly RequestCultureProvider _cultureProvider;

        public CustomUrlStringCultureProviderTest()
        {
            var languageManipulationService = new LanguageManipulationService();
            _cultureProvider = new CustomUrlStringCultureProvider(
                languageManipulationService);

        }

        [Theory]
        [InlineData("En-US", "en-US")]
        [InlineData("DE-DE", "de-DE")]
        [InlineData("ru-rU", "ru-RU")]
        public void CanDetermineCulture(string pathInput, string expectedCulture)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Path = $"/{pathInput}/something";
            var task =_cultureProvider.DetermineProviderCultureResult(httpContext);

            var taskResult = task.Result;
            Assert.Equal(expectedCulture, taskResult.Cultures[0]);
        }
    }
}
