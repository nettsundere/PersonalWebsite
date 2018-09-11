using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalWebsite.Areas.Private.Controllers;
using PersonalWebsite.ViewModels.Content;
using WebsiteContent.Repositories;
using WebsiteContent.Repositories.DTO;
using Xunit;

namespace PersonalWebsite.Tests.Unit.Areas.Private.Controllers
{
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
                        new ContentPrivateEditListData()
                        {
                           Contents = Enumerable.Empty<ContentPrivateLinksData>()
                        }
                    )
                };
                yield return new[]
                {
                    new ContentIndexViewModel(
                        new ContentPrivateEditListData()
                        {
                            Contents = new []
                            {
                                new ContentPrivateLinksData { Id = -1, InternalCaption = "fake1" }
                            }
                        }
                    )
                };
                yield return new[]
                {
                    new ContentIndexViewModel(
                        new ContentPrivateEditListData()
                        {
                            Contents = new []
                            {
                                new ContentPrivateLinksData { Id = -1, InternalCaption = "fake1" },
                                new ContentPrivateLinksData { Id = 3, InternalCaption = "fake3" }
                            }
                        }
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
                yield return new object[] { null, typeof(NotFoundResult) };

                yield return new object[]
                {
                    new ContentPrivateEditData()
                    {
                        Id = -2,
                        InternalCaption = "Test",
                        Translations = null
                    },
                    typeof(ViewResult)
                };
            }
        }

        /// <summary>
        /// Test Get Index.
        /// </summary>
        /// <param name="contentList">Content List view model stub.</param>
        [Theory]
        [MemberData(nameof(ContentListExamples))]
        public void ReturnsListOfContents(ContentIndexViewModel contentList)
        {
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.ReadList()).Returns(contentList);
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.Index();
            var resultModel = ((actionResult as ViewResult).Model as ContentIndexViewModel);

            Assert.Equal(contentList.Contents.Count(), resultModel.Contents.Count());
        }

        /// <summary>
        /// Test Get edit returns content editor.
        /// </summary>
        /// <param name="privateEditData">Private edit data.</param>
        /// <param name="expectedResultType">Expected result type.</param>
        [Theory]
        [MemberData(nameof(ContentDataExamples))]
        public void ReturnsContentEditor(ContentPrivateEditData privateEditData, Type expectedResultType)
        {
            var fakeId = -1;
            var fakeContentRepository = new Mock<IContentEditorRepository>();

            fakeContentRepository.Setup(x => x.Read(fakeId)).Returns(privateEditData);
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.Edit(fakeId);

            Assert.IsType(expectedResultType, actionResult);
        }

        /// <summary>
        /// Test Post edit changes a content.
        /// </summary>
        [Fact]
        public void ChangesContent()
        {
            var fakeContent = new ContentPrivateEditData() { Id = 100 };
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.Update(fakeContent)).Returns(fakeContent);
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.Edit(new ContentAndTranslationsEditViewModel(fakeContent));

            fakeContentRepository.Verify(x => x.Update(It.IsAny<ContentPrivateEditData>()));
            Assert.IsType<RedirectToActionResult>(actionResult);
        }

        /// <summary>
        /// Test Post create creates a content.
        /// </summary>
        [Fact]
        public void CreatesContent()
        {
            var fakeContent = new ContentPrivateEditData();
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.Create(fakeContent)).Returns(fakeContent);
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.Create(new ContentAndTranslationsEditViewModel(fakeContent));

            fakeContentRepository.Verify(x => x.Create(It.IsAny<ContentPrivateEditData>()));
            Assert.IsType<RedirectToActionResult>(actionResult);
        }


        /// <summary>
        /// Test Get Delete returns removal confirmation.
        /// </summary>
        [Fact]
        public void ConfirmsContentRemoval()
        {
            var fakeContent = new ContentPrivateEditData { Id = -1 };
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.Read(fakeContent.Id)).Returns(fakeContent);
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.Delete(fakeContent.Id);
   
            Assert.IsType<ViewResult>(actionResult);
        }

        /// <summary>
        /// Test Post Delete deletes a content.
        /// </summary>
        [Fact]
        public void DeletesContent()
        {
            var fakeContent = new ContentPrivateEditData { Id = -1 };
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.Delete(fakeContent.Id));
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.DeleteConfirmed(fakeContent.Id);

            Assert.IsType<RedirectToActionResult>(actionResult);
        }
    }
}
