using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Controllers;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;
using PersonalWebsite.Services;
using PersonalWebsite.ViewModels.Content;
using WebsiteContent.Lib;
using WebsiteContent.Models;
using Xunit;

namespace PersonalWebsite.Tests.Integration.Controllers
{
    /// <summary>
    /// Test <see cref="HomeController"/>.
    /// </summary>
    public class HomeControllerTests: IDisposable
    {
        /// <summary>
        /// Page configuration.
        /// </summary>
        private readonly IPageConfiguration _pageConfiguration;

        /// <summary>
        /// Subject <see cref="HomeController"/>.
        /// </summary>
        private readonly HomeController _homeController;

        /// <summary>
        /// Data DB context.
        /// </summary>
        private readonly DataDbContext _dataDbContext;

        /// <summary>
        /// Fake page configuration.
        /// </summary>
        private class FakePageConfiguration : IPageConfiguration
        {
            public LanguageDefinition DefaultLanguage => LanguageDefinition.de_de;

            public string DefaultPageInternalCaption => "someCaption";
        }

        /// <summary>
        /// Create <see cref="HomeControllerTests"/>.
        /// </summary>
        public HomeControllerTests()
        {
            // Dependencies initializations
            var connection = InMemoryConnectionHelper.SetupConnection();
 
            var services = new ServiceCollection();
            services.AddEntityFrameworkSqlite()
                .AddDbContext<DataDbContext>(options =>
                        options.UseSqlite(connection),
                    ServiceLifetime.Transient
                );

            var serviceProvider = services.BuildServiceProvider();

            _pageConfiguration = new FakePageConfiguration();

            _dataDbContext = serviceProvider.GetService<DataDbContext>();
            _dataDbContext.Database.EnsureCreated();
            
            var contentRepository = new ContentViewerRepository(_dataDbContext);
            var humanReadableContentService = new HumanReadableContentRetrievalService(_pageConfiguration, contentRepository);

            ILanguageManipulationService languageManipulationService = new LanguageManipulationService();

            // Controller initialization
            _homeController = new HomeController(
                _pageConfiguration,
                humanReadableContentService,
                languageManipulationService
            );
        }

        [Theory]
        [InlineData("en-ENZ")]
        [InlineData("ru-RU-EN")]
        [InlineData("SOMETHING")]
        public async Task ReturnsContentNotFound(string language)
        {
            SetupContent();

            var actionResult = await _homeController.Index(language);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Theory]
        [InlineData("En-US")]
        [InlineData("de-DE")]
        [InlineData("RU-ru")]
        [InlineData("")] // Default language value case
        public async Task ReturnsSuccess(string language)
        {
            SetupContent();

            var actionResult = await _homeController.Index(language);

            Assert.IsType<ViewResult>(actionResult);
        }

        [Theory]
        [InlineData("En-US", LanguageDefinition.en_us)]
        [InlineData("de-DE", LanguageDefinition.de_de)]
        public async Task SetsProperLanguage(
            string language,
            LanguageDefinition expectedModelLanguage)
        {
            SetupContent();

            var result = await _homeController.Index(language) as ViewResult;

            var resultModel = result?.ViewData.Model as PageViewModel;
            Assert.Equal(expectedModelLanguage, resultModel?.Language);
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
                            CustomHeaderMarkup = string.Empty,
                            ContentMarkup = string.Empty,
                            Description = string.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.en_us
                        },
                        new Translation {
                            UrlName = "url2",
                            Title = "Lebenslauf",
                            CustomHeaderMarkup = string.Empty,
                            ContentMarkup = string.Empty,
                            Description = string.Empty,
                            State = DataAvailabilityState.published,
                            Version = LanguageDefinition.de_de
                        },
                        new Translation {
                            UrlName = "url3",
                            Title = "Lebenslauf",
                            CustomHeaderMarkup = string.Empty,
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

        public void Dispose()
        {
            _dataDbContext.Dispose();
        }
    }
}
