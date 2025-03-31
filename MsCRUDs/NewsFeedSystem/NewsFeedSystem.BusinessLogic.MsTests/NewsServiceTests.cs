using System;
using System.Threading.Tasks;
using Moq;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;
using System.Text;
using AutoFixture;
using System.Collections.Generic;

namespace NewsFeedSystem.BusinessLogic.MsTests
{
    [TestClass]
    public sealed class NewsServiceTests
    {
        private readonly Mock<INewsRepository> _newsRepositoryMock;
        private readonly NewsService _newsService;

        public NewsServiceTests()
        {
            _newsRepositoryMock = new Mock<INewsRepository>();
            _newsService = new NewsService(_newsRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Create_NewsPostIsNull_ShouldThrowArgumentNullException()
        {
            NewsPost newsPost = null;
            CreateResult createResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => createResult = await _newsService.Create(newsPost!));

            _newsRepositoryMock.Verify(nr => nr.Create(newsPost!), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(createResult);
            Assert.AreEqual(ErrorStrings.NEWSPOST_RARAM_NAME, exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Create_HeadlineIsNullOrWhiteSpace_ShouldReturnCreateResult_HEADLINE_SHOULD_NOT_BE_EMPTY_400(string headline)
        {
            NewsPost newsPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields(generateHeadline: false, headline: headline);
            var createResult = await _newsService.Create(newsPost);

            _newsRepositoryMock.Verify(nr => nr.Create(newsPost), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.HEADLINE_SHOULD_NOT_BE_EMPTY, createResult.Message);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Create_TextIsNullOrWhiteSpace_ShouldReturnCreateResult_TEXT_SHOULD_NOT_BE_EMPTY_400(string text)
        {
            NewsPost newsPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields(generateText: false, text: text);
            var createResult = await _newsService.Create(newsPost);

            _newsRepositoryMock.Verify(nr => nr.Create(newsPost), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TEXT_SHOULD_NOT_BE_EMPTY, createResult.Message);
        }

        [TestMethod]
        public async Task Create_TagsIsNull_ShouldReturnCreateResult_TAGS_SHOULD_NOT_BE_NULL_OR_EMPTY_400()
        {
            NewsPost newsPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields(generateTags: false);
            var createResult = await _newsService.Create(newsPost);

            _newsRepositoryMock.Verify(nr => nr.Create(newsPost), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TAGS_SHOULD_NOT_BE_NULL_OR_EMPTY, createResult.Message);
        }

        [TestMethod]
        public async Task Create_TagsIsEmptyList_ShouldReturnCreateResult_TAGS_SHOULD_NOT_BE_NULL_OR_EMPTY_400()
        {
            NewsPost newsPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields();
            newsPost.UpdateTags([]);
            var createResult = await _newsService.Create(newsPost);

            _newsRepositoryMock.Verify(nr => nr.Create(newsPost), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TAGS_SHOULD_NOT_BE_NULL_OR_EMPTY, createResult.Message);
        }


        [TestMethod]
        public async Task Create_TopicsIsNull_ShouldReturnCreateResult_TOPICS_SHOULD_NOT_BE_NULL_OR_EMPTY_400()
        {
            NewsPost newsPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields(generateTopics: false);
            var createResult = await _newsService.Create(newsPost);

            _newsRepositoryMock.Verify(nr => nr.Create(newsPost), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TOPICS_SHOULD_NOT_BE_NULL_OR_EMPTY, createResult.Message);
        }

        [TestMethod]
        public async Task Create_TopicsIsEmptyList_ShouldReturnCreateResult_TOPICS_SHOULD_NOT_BE_NULL_OR_EMPTY_400()
        {
            NewsPost newsPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields(generateTopics: false);
            newsPost.UpdateTopics([]);
            var createResult = await _newsService.Create(newsPost);

            _newsRepositoryMock.Verify(nr => nr.Create(newsPost), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TOPICS_SHOULD_NOT_BE_NULL_OR_EMPTY, createResult.Message);
        }

        [TestMethod]
        public async Task Create_NewsPostIsValid_ShouldReturnOk()
        {
            NewsPost newsPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _newsRepositoryMock.Setup(nr => nr.Create(newsPost))
                .ReturnsAsync(new CreateResult
                {
                    Id = expectedId,
                    StatusCode = System.Net.HttpStatusCode.Created
                });

            var createResult = await _newsService.Create(newsPost);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _newsRepositoryMock.Verify(nr => nr.Create(newsPost), Times.Once);
        }

        [TestMethod]
        public async Task Create_NewsPostIsValidAndFull_ShouldReturnOk()
        {
            NewsPost newsPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _newsRepositoryMock.Setup(nr => nr.Create(newsPost))
                .ReturnsAsync(new CreateResult
                {
                    Id = expectedId,
                    StatusCode = System.Net.HttpStatusCode.Created
                });

            var createResult = await _newsService.Create(newsPost);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _newsRepositoryMock.Verify(nr => nr.Create(newsPost), Times.Once);
        }

        [TestMethod]
        public async Task DeleteNewsPost_NotFound_ShouldReturnDeleteResult_NEWS_NOT_FOUND()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _newsRepositoryMock.Setup(nr => nr.DeleteNewsPost(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = Core.ErrorStrings.NEWS_NOT_FOUND
                });

            var deleteResult = await _newsService.DeleteNewsPost(id);

            _newsRepositoryMock.Verify(nr => nr.DeleteNewsPost(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.NEWS_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteNewsPost_OK_ShouldReturnDeleteResult_OK()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _newsRepositoryMock.Setup(nr => nr.DeleteNewsPost(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = Core.ErrorStrings.OK
                });

            var deleteResult = await _newsService.DeleteNewsPost(id);

            _newsRepositoryMock.Verify(nr => nr.DeleteNewsPost(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task Get_NotExistingNewsPost_ShouldReturnNull()
        {
            NewsPost newsPost = null;
            var id = TestFixtures.TestFixtures.GenerateId();

            var existingPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields(setId: id);
            _newsRepositoryMock.Setup(nr => nr.Get(id))
                .ReturnsAsync(newsPost);

            newsPost = await _newsService.Get(id);

            _newsRepositoryMock.Verify(nr => nr.Get(id), Times.Once);

            Assert.IsNull(newsPost);
        }

        [TestMethod]
        public async Task Get_ExistingNewsPost_ShouldReturnNewsPost()
        {
            NewsPost newsPost;
            var id = TestFixtures.TestFixtures.GenerateId();

            var existingPost = TestFixtures.TestFixtures.GetNewsPostFixtureWithAllFields(setId: id);
            _newsRepositoryMock.Setup(nr => nr.Get(id))
                .ReturnsAsync(existingPost);

            newsPost = await _newsService.Get(id);

            _newsRepositoryMock.Verify(nr => nr.Get(id), Times.Once);

            Assert.IsNotNull(newsPost);
            Assert.AreEqual(id, newsPost.Id);
            Assert.AreEqual(existingPost, newsPost);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow(null, 100500)]
        [DataRow(100500, null)]
        [DataRow(100500, 100500)]
        [DataRow(1, 100)]
        public async Task GetHeadlines_ShouldCallGetHeadlinesMethodOfRepository(int? minNewsId, int? maxNewsId)
        {
            var result = await _newsService.GetHeadlines((uint?)minNewsId, (uint?)maxNewsId);
            _newsRepositoryMock.Verify(nr => nr.GetHeadlines((uint?)minNewsId, (uint?)maxNewsId), Times.Once);
        }

        [DataTestMethod]
        [DataRow(100501, 100500)]
        [DataRow(2, 1)]
        public async Task GetHeadlines_MinNewsIdMoreThanMaxNewsId_ShouldReturnEmptyList(int? minNewsId, int? maxNewsId)
        {
            var result = await _newsService.GetHeadlines((uint?)minNewsId, (uint?)maxNewsId);
            _newsRepositoryMock.Verify(nr => nr.GetHeadlines((uint?)minNewsId, (uint?)maxNewsId), Times.Never);
        }

        [TestMethod]
        public async Task GetHeadlinesByTag()
        {
            var tagId = TestFixtures.TestFixtures.GenerateId();
            var minNewsId = TestFixtures.TestFixtures.GenerateId();

            var result = await _newsService.GetHeadlinesByTag(tagId, minNewsId);
            _newsRepositoryMock.Verify(nr => nr.GetHeadlinesByTag(tagId, minNewsId), Times.Once);
        }

        [TestMethod]
        public async Task GetHeadlinesByTopic()
        {
            var topicId = TestFixtures.TestFixtures.GenerateId();
            var minNewsId = TestFixtures.TestFixtures.GenerateId();

            var result = await _newsService.GetHeadlinesByTopic(topicId, minNewsId);
            _newsRepositoryMock.Verify(nr => nr.GetHeadlinesByTopic(topicId, minNewsId), Times.Once);
        }

        [TestMethod]
        public async Task Update_NewsPostIsNull_ShouldThrowArgumentNullException()
        {
            throw new NotImplementedException(); //TODO: Update_NewsPostIsNull_ShouldThrowArgumentNullException()
        }

        [TestMethod]
        public async Task Update_HeadlineIsNullOrWhiteSpace_ShouldReturnCreateResult_HEADLINE_SHOULD_NOT_BE_EMPTY_400()
        {
            throw new NotImplementedException(); //TODO: Update_HeadlineIsNullOrWhiteSpace_ShouldReturnCreateResult_HEADLINE_SHOULD_NOT_BE_EMPTY_400()
        }

        [TestMethod]
        public async Task Update_TextIsNullOrWhiteSpace_ShouldReturnCreateResult_TEXT_SHOULD_NOT_BE_EMPTY_400()
        {
            throw new NotImplementedException(); //TODO: Update_TextIsNullOrWhiteSpace_ShouldReturnCreateResult_TEXT_SHOULD_NOT_BE_EMPTY_400()
        }

        [TestMethod]
        public async Task Update_NewsPostIsValid_ShouldReturnOk()
        {
            throw new NotImplementedException(); //TODO: Update_NewsPostIsValid_ShouldReturnOk
        }

        [TestMethod]
        public async Task Update_NewsPostIsValidAndFull_ShouldReturnOk()
        {
            throw new NotImplementedException(); //TODO: Update_NewsPostIsValidAndFull_ShouldReturnOk
        }
    }
}