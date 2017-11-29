using System;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Content view model.
    /// </summary>
    public class ContentViewModel : ContentPublicViewData
    {
        /// <summary>
        /// Create <see cref="ContentViewModel"/>.
        /// </summary>
        public ContentViewModel() : this(new ContentPublicViewData())
        {
        }

        /// <summary>
        /// Create <see cref="ContentViewModel"/>.
        /// </summary>
        /// <param name="contentPublicViewData">Public content data.</param>
        public ContentViewModel(ContentPublicViewData contentPublicViewData)
        {
            if (contentPublicViewData == null)
            {
                throw new ArgumentNullException(nameof(contentPublicViewData));
            }

            Description = contentPublicViewData.Description;
            InternalCaption = contentPublicViewData.InternalCaption;
            Markup = contentPublicViewData.Markup;
            Title = contentPublicViewData.Title;
            UrlNames = contentPublicViewData.UrlNames;
        }
    }
}
