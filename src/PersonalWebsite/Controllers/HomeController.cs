﻿using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Services;
using System;
using System.Threading.Tasks;
using WebsiteContent.Lib;

namespace PersonalWebsite.Controllers;

/// <summary>
/// Home controller.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Human-readable content service.
    /// </summary>
    private readonly IHumanReadableContentRetrievalService _humanReadableContentService;
        
    /// <summary>
    /// Language manipulation service. 
    /// </summary>
    private readonly ILanguageManipulationService _languageManipulationService;

    /// <summary>
    /// Page configuration.
    /// </summary>
    private readonly IPageConfiguration _pageConfiguration;

    /// <summary>
    /// Create <see cref="HomeController"/>.
    /// </summary>
    /// <param name="pageConfiguration">Page configuration.</param>
    /// <param name="humanReadableContentService">Human-readable content retrieval service.</param>
    /// <param name="languageManipulationService">Language manipulation service.</param>
    public HomeController(
        IPageConfiguration pageConfiguration,
        IHumanReadableContentRetrievalService humanReadableContentService, 
        ILanguageManipulationService languageManipulationService) 
    {
        _pageConfiguration = pageConfiguration ?? throw new ArgumentNullException(nameof(pageConfiguration));
        _humanReadableContentService = humanReadableContentService ?? throw new ArgumentNullException(nameof(humanReadableContentService));
        _languageManipulationService = languageManipulationService ?? throw new ArgumentNullException(nameof(languageManipulationService));
    }

    /// <summary>
    /// Index route.
    /// </summary>
    /// <param name="language">language</param>
    /// <returns>Index page.</returns>
    public async Task<IActionResult> Index(string language)
    {
        LanguageDefinition languageDefinition;
        if (string.IsNullOrWhiteSpace(language))
        {
            languageDefinition = _pageConfiguration.DefaultLanguage;
        }
        else
        {
            try
            {
                languageDefinition = _languageManipulationService.LanguageRepresentationToLanguageDefinition(language);
            }
            catch
            {
                return NotFound();
            }
        }

        try
        {
            var pageVm = await _humanReadableContentService.GetPageByInternalCaptionAsync(
                languageDefinition,
                _pageConfiguration.DefaultPageInternalCaption);

            return View(pageVm);
        }
        catch(InvalidOperationException)
        {
            return NotFound();
        }
    }
}