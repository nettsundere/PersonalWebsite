using System;
using WebsiteContent.Repositories.DTO;

namespace PersonalWebsite.ViewModels.Content
{
    /// <summary>
    /// View model for private content list displaying.
    /// </summary>
    public class ContentIndexViewModel : ContentPrivateEditListData
    {
        /// <summary>
        /// Create <see cref="ContentIndexViewModel"/>.
        /// </summary>
        /// <param name="content">Content list private data.</param>
        public ContentIndexViewModel(ContentPrivateEditListData content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Contents = content.Contents;
        }
    }
}
