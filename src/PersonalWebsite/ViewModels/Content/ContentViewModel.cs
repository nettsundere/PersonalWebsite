using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.ViewModels.Content;

/// <summary>
/// Content view model.
/// </summary>
public class ContentViewModel : ContentPublicViewData
{
    /// <summary>
    /// Create <see cref="ContentViewModel"/>.
    /// </summary>
    /// <param name="contentPublicViewData">Public content data.</param>
    public ContentViewModel(ContentPublicViewData contentPublicViewData): 
        base(contentPublicViewData.Title, contentPublicViewData.CustomHeaderMarkup, contentPublicViewData.Markup, 
            contentPublicViewData.Description, contentPublicViewData.InternalCaption, contentPublicViewData.UrlNames)
    {
    }
}