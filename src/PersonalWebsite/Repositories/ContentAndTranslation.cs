using System;
using WebsiteContent.Models;

namespace PersonalWebsite.Repositories
{
    internal class ContentAndTranslation
    {
        public Content Content { get; }
        public Translation Translation { get; }

        public ContentAndTranslation(Content content, Translation translation)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Translation = translation ?? throw new ArgumentNullException(nameof(translation));
        }
    }
}