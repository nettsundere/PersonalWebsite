using System;
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
    public class HomeControllerTests 
    {
        private readonly IPageConfiguration _pageConfiguration;

        /// <summary>
        /// Subject <see cref="HomeController"/>.
        /// </summary>
        private readonly HomeController _homeController;

        /// <summary>
        /// Service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        
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
            const string databaseName = "HomeTest";

            // Database setup
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<DataDbContext>(options =>
                        options.UseInMemoryDatabase(databaseName),
                        ServiceLifetime.Transient
                    );

            _serviceProvider = services.BuildServiceProvider();

            // Dependencies initializations
            _pageConfiguration = new FakePageConfiguration();

            var contentRepository = new ContentViewerRepository(_serviceProvider);
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
        /// Setup content.
        /// </summary>
        private void SetupContent()
        {
            using (var dataDbContext = _serviceProvider.GetService<DataDbContext>())
            {
                dataDbContext.Content.Add(
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

                dataDbContext.SaveChanges();
            }
        }
    }
}
