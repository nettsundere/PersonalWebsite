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
    public class ContentsControllerTests
    {
        private readonly IPageConfiguration _pageConfiguration;
        private readonly IHumanReadableContentService _humanReadableContentService;
        private readonly ILanguageManipulationService _languageManipulationService;

        private readonly IContentRepository _contentRepository;
        private readonly PersonalWebsite.Controllers.ContentsController _contentsController;
        private readonly DataDbContext _dataDbContext;

        public ContentsControllerTests()
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
            _pageConfiguration = new PageConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<DataDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            _dataDbContext = new DataDbContext(optionsBuilder.Options);

            _contentRepository = new ContentRepository(_dataDbContext);
            _humanReadableContentService = new HumanReadableContentService(_pageConfiguration, _contentRepository);

            _languageManipulationService = new LanguageManipulationService();

            // Controller initialization
            _contentsController = new PersonalWebsite.Controllers.ContentsController(
                _pageConfiguration, 
                _humanReadableContentService, 
                _languageManipulationService
            );
        }

        [Theory]
        [InlineData("fake", "fake")]
        [InlineData("ru-RU", "fake")]
        [InlineData("", "fake")]
        public void ReturnsContentNotFound(string language, string urlName)
        {
            SetupContent();

            var actionResult = _contentsController.Show(language, urlName);

            Assert.IsType(typeof(HttpNotFoundResult), actionResult);
        }

        [Theory]
        [InlineData("En-US", "ReSuMe")]
        [InlineData("de-DE", "Lebenslauf")]
        [InlineData("", "ReSUME")] // Default language value case
        public void ReturnsSuccess(string language, string urlName)
        {
            SetupContent();

            var actionResult = _contentsController.Show(language, urlName);

            Assert.IsType(typeof(ViewResult), actionResult);
        }

        private void SetupContent()
        {
            _dataDbContext.Contents.Add(
                new Content
                {
                    InternalCaption = "Something",
                    Translations = new[]
                    {
                        new Translation {
                            UrlName = "resume",
                            Title = "Resume",
                            ContentMarkup = String.Empty,
                            Description = String.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.en_us
                        },
                        new Translation {
                            UrlName = "lebenslauf",
                            Title = "Lebenslauf",
                            ContentMarkup = String.Empty,
                            Description = String.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.de_de
                        }
                    }
                }
            );

            _dataDbContext.SaveChanges();
        }
    }
}
