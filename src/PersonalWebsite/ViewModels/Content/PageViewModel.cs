using PersonalWebsite.Lib;
using PersonalWebsite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public PageViewModel(
            IPageConfiguration pageConfiguration,
            LanguageDefinition language,
            ContentViewModel contentViewModel,
            ContentLinksViewModel contentLinksViewModel)
        {
            if(pageConfiguration == null)
            {
                throw new ArgumentNullException(nameof(pageConfiguration));
            }

            if (contentViewModel == null)
            {
                throw new ArgumentNullException(nameof(contentViewModel));
            }

            if(contentLinksViewModel == null)
            {
                throw new ArgumentNullException(nameof(contentLinksViewModel));
            }

            _pageConfiguration = pageConfiguration;
            Language = language;
            ContentViewModel = contentViewModel;
            ContentLinksViewModel = contentLinksViewModel;
        }

        public bool IsDefaultPage()
        {
            return ContentViewModel.InternalCaption == _pageConfiguration.DefaultPageInternalCaption;
        }

        public bool IsDefaultPage(string internalContentCaption)
        {
            return internalContentCaption == _pageConfiguration.DefaultPageInternalCaption;
        }
    }
}
