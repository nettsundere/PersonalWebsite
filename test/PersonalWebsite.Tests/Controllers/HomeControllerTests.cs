using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Controllers;
using PersonalWebsite.Lib;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;
using PersonalWebsite.Services;
using PersonalWebsite.ViewModels.Content;
using System;
using WebsiteContent.Lib;
using WebsiteContent.Models;
using WebsiteContent.Repositories;
using Xunit;

namespace PersonalWebsite.Tests.Controllers
{
    /// <summary>
    /// Test <see cref="HomeController"/>.
    /// </summary>
    public class HomeControllerTests : IDisposable
    {
        private readonly IPageConfiguration _pageConfiguration;
        private readonly IHumanReadableContentRetrievalService _humanReadableContentService;
        private readonly ILanguageManipulationService _languageManipulationService;

        private readonly IContentViewerRepository _contentRepository;
        private readonly HomeController _homeController;
        private readonly DataDbContext _dataDbContext;

        /// <summary>
        /// Fake page configuration.
        /// </summary>
        private class FakePageConfiguration : IPageConfiguration
        {
            public LanguageDefinition DefaultLanguage
            {
                get
                {
                    return LanguageDefinition.de_de;
                }
            }

            public string DefaultPageInternalCaption
            {
                get
                {
                    return "someCaption";
                }
            }
        }

        /// <summary>
        /// Create <see cref="HomeControllerTests"/>.
        /// </summary>
        public HomeControllerTests()
        {
            const string databaseName = "HomeTest";

            // Database setup
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<DataDbContext>(options =>
                        options.UseInMemoryDatabase(databaseName)
                    );

            // Dependencies initializations
            _pageConfiguration = new FakePageConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<DataDbContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName);

            _dataDbContext = new DataDbContext(optionsBuilder.Options);

            _contentRepository = new ContentViewerRepository(_dataDbContext);
            _humanReadableContentService = new HumanReadableContentRetrievalService(_pageConfiguration, _contentRepository);

            _languageManipulationService = new LanguageManipulationService();

            // Controller initialization
            _homeController = new HomeController(
                _pageConfiguration,
                _humanReadableContentService,
                _languageManipulationService
            );
        }

        [Theory]
        [InlineData("en-ENZ")]
        [InlineData("ru-RU-EN")]
        [InlineData("SOMETHING")]
        public void ReturnsContentNotFound(string language)
        {
            SetupContent();

            var actionResult = _homeController.Index(language);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Theory]
        [InlineData("En-US")]
        [InlineData("de-DE")]
        [InlineData("RU-ru")]
        [InlineData("")] // Default language value case
        public void ReturnsSuccess(string language)
        {
            SetupContent();

            var actionResult = _homeController.Index(language);

            Assert.IsType<ViewResult>(actionResult);
        }

        [Theory]
        [InlineData("En-US", LanguageDefinition.en_us)]
        [InlineData("de-DE", LanguageDefinition.de_de)]
        public void SetsProperLanguage(
            string language,
            LanguageDefinition expectedModelLanguage)
        {
            SetupContent();

            var result = _homeController.Index(language) as ViewResult;

            var resultModel = result.ViewData.Model as PageViewModel;
            Assert.Equal(expectedModelLanguage, resultModel.Language);
        }

        /// <summary>
        /// Global dispose.
        /// </summary>
        public void Dispose()
        {
            _humanReadableContentService.Dispose();
        }

        /// <summary>
        /// Setup content.
        /// </summary>
        private void SetupContent()
        {
            _dataDbContext.Content.Add(
                new Content
                {
                    InternalCaption = _pageConfiguration.DefaultPageInternalCaption,
                    Translations = new[]
                    {
                        new Translation {
                            UrlName = "url1",
                            Title = "Resume",
                            ContentMarkup = string.Empty,
                            Description = string.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.en_us
                        },
                        new Translation {
                            UrlName = "url2",
                            Title = "Lebenslauf",
                            ContentMarkup = string.Empty,
                            Description = string.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.de_de
                        },
                        new Translation {
                            UrlName = "url3",
                            Title = "Lebenslauf",
                            ContentMarkup = string.Empty,
                            Description = string.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.ru_ru
                        }
                    }
                }
            );

            _dataDbContext.SaveChanges();
        }
    }
}
