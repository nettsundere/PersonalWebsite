using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalWebsite.Areas.Private.Controllers;
using PersonalWebsite.Repositories;
using PersonalWebsite.ViewModels.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PersonalWebsite.Tests.Areas.Private.Controllers
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
                    new ContentIndexViewModel
                    {
                        Contents = Enumerable.Empty<ContentIndexLinkUI>()
                    }
                };
                yield return new[]
                {
                    new ContentIndexViewModel
                    {
                        Contents = new []
                        {
                            new ContentIndexLinkUI { Id = -1, InternalCaption = "fake1" }
                        }
                    }
                };
                yield return new[]
                {
                    new ContentIndexViewModel
                    {
                        Contents = new []
                        {
                            new ContentIndexLinkUI { Id = -1, InternalCaption = "fake1" },
                            new ContentIndexLinkUI { Id = 3, InternalCaption = "fake3" }
                        }
                    }
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
                    new ContentEditViewModel
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
        /// <param name="content">Content stub.</param>
        /// <param name="expectedResultType">Expected result type.</param>
        [Theory]
        [MemberData(nameof(ContentDataExamples))]
        public void ReturnsContentEditor(ContentEditViewModel content, Type expectedResultType)
        {
            var fakeId = -1;
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.Read(fakeId)).Returns(content);
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
            var fakeContent = new ContentEditViewModel();
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.Update(fakeContent)).Returns(fakeContent);
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.Edit(fakeContent);

            fakeContentRepository.Verify(x => x.Update(fakeContent));
            Assert.IsType<RedirectToActionResult>(actionResult);
        }

        /// <summary>
        /// Test Post create creates a content.
        /// </summary>
        [Fact]
        public void CreatesContent()
        {
            var fakeContent = new ContentEditViewModel();
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.Create(fakeContent)).Returns(fakeContent);
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.Create(fakeContent);

            fakeContentRepository.Verify(x => x.Create(fakeContent));
            Assert.IsType<RedirectToActionResult>(actionResult);
        }


        /// <summary>
        /// Test Get Delete returns removal confirmation.
        /// </summary>
        [Fact]
        public void ConfirmsContentRemoval()
        {
            var fakeContent = new ContentEditViewModel { Id = -1 };
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
            var fakeContent = new ContentEditViewModel { Id = -1 };
            var fakeContentRepository = new Mock<IContentEditorRepository>();
            fakeContentRepository.Setup(x => x.Delete(fakeContent.Id));
            var subject = new ContentsController(fakeContentRepository.Object);

            var actionResult = subject.DeleteConfirmed(fakeContent.Id);

            Assert.IsType<RedirectToActionResult>(actionResult);
        }
    }
}
