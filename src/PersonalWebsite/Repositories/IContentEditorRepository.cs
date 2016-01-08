using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalWebsite.ViewModels.Content;

namespace PersonalWebsite.Repositories
{
    public interface IContentEditorRepository : IDisposable
    {
        ContentEditViewModel Create(ContentEditViewModel contentEditViewModel);
        ContentIndexViewModel ReadList();
        ContentEditViewModel Read(int contentId);
        ContentEditViewModel Update(ContentEditViewModel contentEditViewModel);
        void Delete(int contentId);
    }
}
