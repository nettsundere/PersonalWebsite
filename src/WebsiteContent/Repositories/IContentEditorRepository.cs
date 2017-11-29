using System;
using WebsiteContent.Repositories.DTO;

namespace WebsiteContent.Repositories
{
    /// <summary>
    /// Content editor repository.
    /// </summary>
    public interface IContentEditorRepository : IDisposable
    {
        /// <summary>
        /// Create a <see cref="ContentPrivateEditData"/> record.
        /// </summary>
        /// <param name="contentEditViewModel">Content editor private data.</param>
        /// <returns>Content editor private data.</returns>
        ContentPrivateEditData Create(ContentPrivateEditData contentEditViewModel);

        /// <summary>
        /// Read a list of private content editor records.
        /// </summary>
        /// <returns>The container containing the list of private editor data.</returns>
        ContentPrivateEditListData ReadList();

        /// <summary>
        /// Get the <see cref="ContentPrivateEditData"/> record by the <paramref name="contentId"/>.
        /// </summary>
        /// <param name="contentId">The unique identifier of a <see cref="ContentPrivateEditData"/> record.</param>
        /// <returns>Desired <see cref="ContentPrivateEditData"/> record.</returns>
        ContentPrivateEditData Read(int contentId);

        /// <summary>
        /// Update <see cref="ContentPrivateEditData"/> record.
        /// </summary>
        /// <param name="contentEditViewModel">Updated data.</param>
        /// <returns>Updated data.</returns>
        ContentPrivateEditData Update(ContentPrivateEditData contentEditViewModel);

        /// <summary>
        /// Delete a content by id.
        /// </summary>
        /// <param name="contentId">Id of a content to delete.</param>
        void Delete(int contentId);
    }
}
