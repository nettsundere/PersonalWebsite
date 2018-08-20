using System;
using WebsiteContent.Models;

namespace PersonalWebsite.Repositories
{
    internal class ContentAndTranslation
    {
        public Content Content { get; private set; }
        public Translation Translation { get; private set; }

        public ContentAndTranslation(Content content, Translation translation)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Translation = translation ?? throw new ArgumentNullException(nameof(translation));
        }
    }
}