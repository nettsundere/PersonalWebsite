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
    /// Tests for <see cref="ContentsController"/>.
    /// </summary>
    public class ContentsControllerTests
    {
        /// <summary>
        /// Subject <see cref="ContentsController"/>.
        /// </summary>
        private readonly ContentsController _contentsController;

        /// <summary>
        /// Service provider.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        
        /// <summary>
        /// Create <see cref="ContentsControllerTests"/>.
        /// </summary>
        public ContentsControllerTests()
        {
            // Database setup
            const string databaseName = "ContentsTest";

            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<DataDbContext>(options =>
                        options.UseInMemoryDatabase(databaseName),
                        ServiceLifetime.Transient
                    );

            _serviceProvider = services.BuildServiceProvider();

            // Dependencies initializations
            IPageConfiguration pageConfiguration = new PageConfiguration();

            var contentRepository = new ContentViewerRepository(_serviceProvider);
            var humanReadableContentService = new HumanReadableContentRetrievalService(pageConfiguration, contentRepository);

            var languageManipulationService = new LanguageManipulationService();

            // Controller initialization
            _contentsController = new ContentsController(
                pageConfiguration, 
                humanReadableContentService, 
                languageManipulationService
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

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Theory]
        [InlineData("En-US", "ReSuMe")]
        [InlineData("de-DE", "Lebenslauf")]
        [InlineData("", "ReSUME")] // Default language value case
        public void ReturnsSuccess(string language, string urlName)
        {
            SetupContent();

            var actionResult = _contentsController.Show(language, urlName);

            Assert.IsType<ViewResult>(actionResult);
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
            using (var dataDbContext = _serviceProvider.GetService<DataDbContext>())
            {
                dataDbContext.Content.Add(
                    new Content
                    {
                        InternalCaption = "Something",
                        Translations = new[]
                        {
                            new Translation {
                                UrlName = "resume",
                                Title = "Resume",
                                ContentMarkup = string.Empty,
                                Description = string.Empty,
                                State = DataAvailabilityState.published,
                                Version = LanguageDefinition.en_us
                            },
                            new Translation {
                                UrlName = "lebenslauf",
                                Title = "Lebenslauf",
                                ContentMarkup = string.Empty,
                                Description = string.Empty,
                                State = DataAvailabilityState.published,
                                Version = LanguageDefinition.de_de
                            }
                        }
                    }
                );

                dataDbContext.SaveChanges();
            }
        }
    }
}
