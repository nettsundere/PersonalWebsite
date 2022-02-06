using System;
using System.Collections.Generic;
using WebsiteContent.Lib;

namespace WebsiteContent.Repositories.DTO;

public class ContentPublicViewData
{
    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; protected set; }

    /// <summary>
    /// Custom Header Markup.
    /// </summary>
    public string? CustomHeaderMarkup { get; protected set; }
        
    /// <summary>
    /// Html markup.
    /// </summary>
    public string Markup { get; protected set; }

    /// <summary>
    /// Page description.
    /// </summary>
    public string Description { get; protected set; }

    /// <summary>
    /// Internal caption.
    /// </summary>
    public string InternalCaption { get; protected set; }

    /// <summary>
    /// Url names (different translations).
    /// </summary>
    public IDictionary<LanguageDefinition, string> UrlNames { get; protected set; }

    /// <summary>
    /// Create <see cref="ContentPublicViewData"/>
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="customHeaderMarkup">Custom header markup</param>
    /// <param name="markup">Markup</param>
    /// <param name="description">Description</param>
    /// <param name="internalCaption">Internal caption</param>
    /// <param name="urlNames">Url names for different translations.</param>
    public ContentPublicViewData(string title, string? customHeaderMarkup, string markup, string description,
        string internalCaption, IDictionary<LanguageDefinition, string> urlNames)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        CustomHeaderMarkup = customHeaderMarkup;
        Markup = markup ?? throw new ArgumentNullException(nameof(markup));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        InternalCaption = internalCaption ?? throw new ArgumentNullException(nameof(internalCaption));
        UrlNames = urlNames ?? throw new ArgumentNullException(nameof(urlNames));
    }
}