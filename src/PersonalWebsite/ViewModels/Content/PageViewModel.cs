using PersonalWebsite.Services;
using System;
using WebsiteContent.Lib;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// Represents view model required to render a page.
    /// </summary>
    public class PageViewModel
    {
        private readonly IPageConfiguration _pageConfiguration;

        public LanguageDefinition Language { get; private set; }
        public ContentViewModel ContentViewModel { get; private set; }
        public ContentLinksViewModel ContentLinksViewModel { get; private set; }

        /// <summary>
        /// Create <see cref="PageViewModel"/>.
        /// </summary>
        /// <param name="pageConfiguration">Page configuration.</param>
        /// <param name="language">Language.</param>
        /// <param name="contentPublicViewData">Content view data.</param>
        /// <param name="contentPublicLinksData">Content links data.</param>
        public PageViewModel(
            IPageConfiguration pageConfiguration,
            LanguageDefinition language,
            ContentPublicViewData contentPublicViewData,
            ContentPublicLinksData contentPublicLinksData)
        {
            if (contentPublicViewData == null)
            {
                throw new ArgumentNullException(nameof(contentPublicViewData));
            }

            if(contentPublicLinksData == null)
            {
                throw new ArgumentNullException(nameof(contentPublicLinksData));
            }

            _pageConfiguration = pageConfiguration ?? throw new ArgumentNullException(nameof(pageConfiguration));
            Language = language;
            ContentViewModel = new ContentViewModel(contentPublicViewData);
            ContentLinksViewModel = new ContentLinksViewModel(contentPublicLinksData);
        }

        /// <summary>
        /// Return a <c>bool</c> value indicating whether it is a default page.
        /// </summary>
        /// <returns>A <c>bool</c> value indicating whether it is a default page.</returns>
        public bool IsDefaultPage()
        {
            return ContentViewModel.InternalCaption == _pageConfiguration.DefaultPageInternalCaption;
        }

        /// <summary>
        /// Return a <c>bool</c> value indicating whether a page having <paramref name="internalContentCaption"/> is a default page.
        /// </summary>
        /// <returns>A <c>bool</c> value indicating whether <paramref name="internalContentCaption"/> is a default page.</returns>
        public bool IsDefaultPage(string internalContentCaption)
        {
            return internalContentCaption == _pageConfiguration.DefaultPageInternalCaption;
        }
    }
}
