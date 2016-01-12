using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;
using PersonalWebsite.Services;
using System;
using Xunit;
using PersonalWebsite.Lib;

namespace PersonalWebsite.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly IPageConfiguration _pageConfiguration;
        private readonly IHumanReadableContentService _humanReadableContentService;
        private readonly ILanguageManipulationService _languageManipulationService;

        private readonly IContentRepository _contentRepository;
        private readonly PersonalWebsite.Controllers.HomeController _homeController;
        private readonly DataDbContext _dataDbContext;

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

        public HomeControllerTests()
        {
            // Database setup
            var services = new ServiceCollection();
            services.AddEntityFramework()
                    .AddSqlServer()
                    .AddInMemoryDatabase()
                    .AddDbContext<DataDbContext>(options =>
                        options.UseInMemoryDatabase()
                    );

            // Dependencies initializations
            _pageConfiguration = new FakePageConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<DataDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            _dataDbContext = new DataDbContext(optionsBuilder.Options);

            _contentRepository = new ContentRepository(_dataDbContext);
            _humanReadableContentService = new HumanReadableContentService(_pageConfiguration, _contentRepository);

            _languageManipulationService = new LanguageManipulationService();

            // Controller initialization
            _homeController = new PersonalWebsite.Controllers.HomeController(
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

            Assert.IsType(typeof(HttpNotFoundResult), actionResult);
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

            Assert.IsType(typeof(ViewResult), actionResult);
        }

        private void SetupContent()
        {
            _dataDbContext.Contents.Add(
                new Content
                {
                    InternalCaption = _pageConfiguration.DefaultPageInternalCaption,
                    Translations = new[]
                    {
                        new Translation {
                            UrlName = "url1",
                            Title = "Resume",
                            ContentMarkup = String.Empty,
                            Description = String.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.en_us
                        },
                        new Translation {
                            UrlName = "url2",
                            Title = "Lebenslauf",
                            ContentMarkup = String.Empty,
                            Description = String.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.de_de
                        },
                        new Translation {
                            UrlName = "url3",
                            Title = "Lebenslauf",
                            ContentMarkup = String.Empty,
                            Description = String.Empty,
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
