using System;
using PersonalWebsite.ViewModels.Content;

namespace PersonalWebsite.Repositories
{
    /// <summary>
    /// Content editor repository.
    /// </summary>
    public interface IContentEditorRepository : IDisposable
    {
        ContentEditViewModel Create(ContentEditViewModel contentEditViewModel);
        ContentIndexViewModel ReadList();
        ContentEditViewModel Read(int contentId);
        ContentEditViewModel Update(ContentEditViewModel contentEditViewModel);
        void Delete(int contentId);
    }
}
