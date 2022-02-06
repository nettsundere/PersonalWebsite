using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalWebsite.Areas.Private.Controllers;
using PersonalWebsite.ViewModels.Content;
using WebsiteContent.Repositories;
using WebsiteContent.Repositories.DTO;
using Xunit;

namespace PersonalWebsite.Tests.Areas.Private.Controllers;

/// <summary>
/// Tests for <see cref="ContentsController"/>
/// </summary>
public class ContentsControllerTests
{
    /// <summary>
    /// Content list test examples.
    /// </summary>
    public static IEnumerable<object[]> ContentListExamples
    {
        get
        {
            yield return new[]
            {
                new ContentIndexViewModel(
                    new ContentPrivateEditListData(Enumerable.Empty<ContentPrivateLinksData>())
                )
            };
            yield return new[]
            {
                new ContentIndexViewModel(
                    new ContentPrivateEditListData(new []
                        {
                            new ContentPrivateLinksData(-1, "fake1")
                        }
                    )
                )
            };
            yield return new[]
            {
                new ContentIndexViewModel(
                    new ContentPrivateEditListData(new []
                        {
                            new ContentPrivateLinksData(-1,"fake1"),
                            new ContentPrivateLinksData(3, "fake3")
                        }
                    )
                )
            };
        }
    }

    /// <summary>
    /// Content data examples.
    /// </summary>
    public static IEnumerable<object[]> ContentDataExamples
    {
        get
        {
            yield return new object[] { null!, typeof(NotFoundResult) };

            yield return new object[]
            {
                new ContentPrivateEditData(-2, "Test", new List<TranslationPrivateEditData>(0)), typeof(ViewResult)
            };
        }
    }

    /// <summary>
    /// Test Get Index.
    /// </summary>
    /// <param name="contentList">Content List view model stub.</param>
    [Theory]
    [MemberData(nameof(ContentListExamples))]
    public async Task ReturnsListOfContents(ContentIndexViewModel contentList)
    {
        var fakeContentRepository = new Mock<IContentEditorRepository>();
        fakeContentRepository.Setup(x => x.ReadListAsync()).ReturnsAsync(contentList);
        var subject = new ContentsController(fakeContentRepository.Object);

        var actionResult = await subject.Index();
        var resultModel = ((actionResult as ViewResult)?.Model as ContentIndexViewModel);

        Assert.Equal(contentList.Contents.Count(), resultModel?.Contents.Count());
    }

    /// <summary>
    /// Test Get edit returns content editor.
    /// </summary>
    /// <param name="privateEditData">Private edit data.</param>
    /// <param name="expectedResultType">Expected result type.</param>
    [Theory]
    [MemberData(nameof(ContentDataExamples))]
    public async Task ReturnsContentEditor(ContentPrivateEditData privateEditData, Type expectedResultType)
    {
        var fakeId = -1;
        var fakeContentRepository = new Mock<IContentEditorRepository>();

        fakeContentRepository.Setup(x => x.ReadAsync(fakeId)).ReturnsAsync(privateEditData);
        var subject = new ContentsController(fakeContentRepository.Object);

        var actionResult = await subject.Edit(fakeId);

        Assert.IsType(expectedResultType, actionResult);
    }

    /// <summary>
    /// Test Post edit changes a content.
    /// </summary>
    [Fact]
    public async Task ChangesContent()
    {
        var fakeContent = new ContentPrivateEditData(100, "fake", new List<TranslationPrivateEditData>(0));
        var fakeContentRepository = new Mock<IContentEditorRepository>();
        fakeContentRepository.Setup(x => x.UpdateAsync(fakeContent)).ReturnsAsync(fakeContent);
        var subject = new ContentsController(fakeContentRepository.Object);

        var actionResult = await subject.Edit(new ContentAndTranslationsEditViewModel(fakeContent));

        fakeContentRepository.Verify(x => x.UpdateAsync(It.IsAny<ContentPrivateEditData>()));
        Assert.IsType<RedirectToActionResult>(actionResult);
    }

    /// <summary>
    /// Test Post create creates a content.
    /// </summary>
    [Fact]
    public async Task CreatesContent()
    {
        var fakeContent = new ContentPrivateEditData(0, "fake", new List<TranslationPrivateEditData>(0));
        var fakeContentRepository = new Mock<IContentEditorRepository>();
        fakeContentRepository.Setup(x => x.CreateAsync(fakeContent)).ReturnsAsync(fakeContent);
        var subject = new ContentsController(fakeContentRepository.Object);

        var actionResult = await subject.Create(new ContentAndTranslationsEditViewModel(fakeContent));

        fakeContentRepository.Verify(x => x.CreateAsync(It.IsAny<ContentPrivateEditData>()));
        Assert.IsType<RedirectToActionResult>(actionResult);
    }


    /// <summary>
    /// Test Get Delete returns removal confirmation.
    /// </summary>
    [Fact]
    public async Task ConfirmsContentRemoval()
    {
        var fakeContent = new ContentPrivateEditData(-1, "fake", new List<TranslationPrivateEditData>(0));
        var fakeContentRepository = new Mock<IContentEditorRepository>();
        fakeContentRepository.Setup(x => x.ReadAsync(fakeContent.Id)).ReturnsAsync(fakeContent);
        var subject = new ContentsController(fakeContentRepository.Object);

        var actionResult = await subject.Delete(fakeContent.Id);
   
        Assert.IsType<ViewResult>(actionResult);
    }

    /// <summary>
    /// Test Post Delete deletes a content.
    /// </summary>
    [Fact]
    public async Task DeletesContent()
    {
        var fakeContent = new ContentPrivateEditData(-1, "fake", new List<TranslationPrivateEditData>(0));
        var fakeContentRepository = new Mock<IContentEditorRepository>();
        fakeContentRepository.Setup(x => x.DeleteAsync(fakeContent.Id)).Returns(Task.CompletedTask);
        var subject = new ContentsController(fakeContentRepository.Object);

        var actionResult = await subject.DeleteConfirmed(fakeContent.Id);

        Assert.IsType<RedirectToActionResult>(actionResult);
    }
}