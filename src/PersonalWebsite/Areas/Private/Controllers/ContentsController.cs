using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.ViewModels.Content;
using System;
using System.Threading.Tasks;
using WebsiteContent.Repositories;

namespace PersonalWebsite.Areas.Private.Controllers;

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
    public async Task<IActionResult> Index()
    {
        var content = await _contentEditorRepository.ReadListAsync();
        var viewModel = new ContentIndexViewModel(content);

        return View(viewModel);
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
    /// <param name="content">Content and translation data.</param>
    /// <returns>Redirect to a list or error.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentAndTranslationsEditViewModel content)
    {
        if (ModelState.IsValid)
        {
            await _contentEditorRepository.CreateAsync(content.GetContentEditData());
            return RedirectToAction(nameof(Index));
        }
        return View(content);
    }

    /// <summary>
    /// Get Edit, edit particular content.
    /// </summary>
    /// <param name="id">Content id</param>
    /// <returns>Interface to edit a content.</returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            var content = await _contentEditorRepository.ReadAsync(id.Value);
                
            var viewModel = new ContentAndTranslationsEditViewModel(content);

            return View(viewModel);
        }
        catch 
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Post Edit, change particular content.
    /// </summary>
    /// <param name="content">New content and translation data.</param>
    /// <returns>Content editor interface.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContentAndTranslationsEditViewModel content)
    {
        if (ModelState.IsValid)
        {
            await _contentEditorRepository.UpdateAsync(content.GetContentEditData());

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
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            var content = await _contentEditorRepository.ReadAsync(id.Value);
            var viewModel = new ContentAndTranslationsEditViewModel(content);
            return View(viewModel);
        }
        catch 
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Delete a content.
    /// </summary>
    /// <param name="id">Content id to delete.</param>
    /// <returns>Redirect to content list.</returns>
    [HttpPost, ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _contentEditorRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}