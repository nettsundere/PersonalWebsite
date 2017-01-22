using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using PersonalWebsite.Providers;
using PersonalWebsite.Services;
using Xunit;

namespace PersonalWebsite.Tests.Providers
{
    /// <summary>
    /// Tests for <see cref="CustomUrlStringCultureProvider"/>.
    /// </summary>
    public class CustomUrlStringCultureProviderTest
    {
        private readonly RequestCultureProvider _cultureProvider;

        /// <summary>
        /// Create <see cref="CustomUrlStringCultureProviderTest"/>.
        /// </summary>
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
