using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Controllers;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;
using PersonalWebsite.Services;
using PersonalWebsite.Tests.Helpers;
using PersonalWebsite.ViewModels.Content;
using WebsiteContent.Lib;
using WebsiteContent.Models;
using Xunit;

namespace PersonalWebsite.Tests.Controllers;

/// <summary>
/// Tests for <see cref="ContentsController"/>.
/// </summary>
public class ContentsControllerTests: IDisposable
{
    /// <summary>
    /// Subject <see cref="ContentsController"/>.
    /// </summary>
    private readonly ContentsController _contentsController;

    /// <summary>
    /// Data DB context.
    /// </summary>
    private readonly DataDbContext _dataDbContext;
        
    /// <summary>
    /// Create <see cref="ContentsControllerTests"/>.
    /// </summary>
    public ContentsControllerTests()
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
            
        var pageConfiguration = new PageConfiguration();

        _dataDbContext = serviceProvider.GetRequiredService<DataDbContext>();
        _dataDbContext.Database.EnsureCreated();
            
        var contentRepository = new ContentViewerRepository(_dataDbContext);
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
    public async Task ReturnsContentNotFound(string language, string urlName)
    {
        SetupContent();

        var actionResult = await _contentsController.Show(language, urlName);

        Assert.IsType<NotFoundResult>(actionResult);
    }

    [Theory]
    [InlineData("En-US", "ReSuMe")]
    [InlineData("de-DE", "Lebenslauf")]
    [InlineData("", "ReSUME")] // Default language value case
    public async Task ReturnsSuccess(string language, string urlName)
    {
        SetupContent();

        var actionResult = await _contentsController.Show(language, urlName);

        Assert.IsType<ViewResult>(actionResult);
    }

    [Theory]
    [InlineData("En-US", "ReSuMe", LanguageDefinition.en_us)]
    [InlineData("de-DE", "Lebenslauf", LanguageDefinition.de_de)]
    public async Task SetsProperLanguage(
        string language,
        string urlName,
        LanguageDefinition expectedModelLanguage)
    {
        SetupContent();

        var result = await _contentsController.Show(language, urlName) as ViewResult;

        var resultModel = result?.ViewData.Model as PageViewModel;
        Assert.Equal(expectedModelLanguage, resultModel?.Language);
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
                        ContentMarkup = string.Empty,
                        CustomHeaderMarkup = string.Empty,
                        Description = string.Empty,
                        State = DataAvailabilityState.published,
                        Version = LanguageDefinition.en_us
                    },
                    new Translation {
                        UrlName = "lebenslauf",
                        Title = "Lebenslauf",
                        ContentMarkup = string.Empty,
                        CustomHeaderMarkup = string.Empty,
                        Description = string.Empty,
                        State = DataAvailabilityState.published,
                        Version = LanguageDefinition.de_de
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