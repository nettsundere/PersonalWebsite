using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Repositories;
using PersonalWebsite.ViewModels.Content;
using System;

namespace PersonalWebsite.Areas.Private.Controllers
{
    /// <summary>
    /// Contents controller - private. 
    /// Introduces editorial possibilities.
    /// </summary>
    [Authorize]
    [Area(nameof(Private))]
    public class ContentsController : Controller
    {
        private readonly IContentEditorRepository _contentEditorRepository;

        /// <summary>
        /// Create <see cref="ContentsController"/>.
        /// </summary>
        /// <param name="contentEditorRepository">Content editor repository.</param>
        public ContentsController(IContentEditorRepository contentEditorRepository)
        {
            _contentEditorRepository = contentEditorRepository ?? throw new ArgumentNullException(nameof(contentEditorRepository));
        }

        /// <summary>
        /// Get Index, list all content.
        /// </summary>
        /// <returns>All content list representation.</returns>
        public IActionResult Index()
        {
            ContentIndexViewModel content;
            using(_contentEditorRepository)
            {
                content = _contentEditorRepository.ReadList();
            }
            return View(content);
        }

        /// <summary>
        /// Get an interface to create a content.
        /// </summary>
        /// <returns>Interface to create a content.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create a content.
        /// </summary>
        /// <param name="content">Content data.</param>
        /// <returns>Redirect to a list or error.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContentEditViewModel content)
        {
            if (ModelState.IsValid)
            {
                using (_contentEditorRepository)
                {
                    _contentEditorRepository.Create(content);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(content);
        }

        /// <summary>
        /// Get Edit, edit particular content.
        /// </summary>
        /// <param name="id">Content id</param>
        /// <returns>Interface to edit a content.</returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContentEditViewModel content;

            using(_contentEditorRepository)
            {
                content = _contentEditorRepository.Read(id.Value);
            }

            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        /// <summary>
        /// Post Edit, change particular content.
        /// </summary>
        /// <param name="content">New content data.</param>
        /// <returns>Content editor interface.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ContentEditViewModel content)
        {
            if (ModelState.IsValid)
            {
                using(_contentEditorRepository)
                {
                    _contentEditorRepository.Update(content);
                }

                return RedirectToAction(nameof(Edit), new { id = content.Id });
            }
            return View(content);
        }

        /// <summary>
        /// Get Delete, get content removal confirmation.
        /// </summary>
        /// <param name="id">Content id to remove.</param>
        /// <returns>Interface to confirm a removal.</returns>
        [ActionName(nameof(Delete))]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContentEditViewModel content;

            using(_contentEditorRepository)
            {
                content = _contentEditorRepository.Read(id.Value);
            }
            
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        /// <summary>
        /// Delete a content.
        /// </summary>
        /// <param name="id">Content id to delete.</param>
        /// <returns>Redirect to content list.</returns>
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using(_contentEditorRepository)
            {
                _contentEditorRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
