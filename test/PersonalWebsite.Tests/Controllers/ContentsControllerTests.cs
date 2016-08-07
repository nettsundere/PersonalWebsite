using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Lib;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;
using PersonalWebsite.Services;
using PersonalWebsite.ViewModels.Content;
using System;
using Xunit;

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
            services.AddEntityFrameworkInMemoryDatabase()
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

            Assert.IsType(typeof(NotFoundResult), actionResult);
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

        [Theory]
        [InlineData("En-US", "ReSuMe", LanguageDefinition.en_us)]
        [InlineData("de-DE", "Lebenslauf", LanguageDefinition.de_de)]
        public void SetsProperLanguage(
            string language,
            string urlName,
            LanguageDefinition expectedModelLanguage)
        {
            SetupContent();

            var result = _contentsController.Show(language, urlName) as ViewResult;

            var resultModel = result.ViewData.Model as PageViewModel;
            Assert.Equal(expectedModelLanguage, resultModel.Language);
        }

        private void SetupContent()
        {
            _dataDbContext.Content.Add(
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
