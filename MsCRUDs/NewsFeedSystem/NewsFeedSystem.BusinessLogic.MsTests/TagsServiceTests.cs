using System;
using System.Threading.Tasks;
using Moq;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;
using System.Text;

namespace NewsFeedSystem.BusinessLogic.MsTests
{
    [TestClass]
    public sealed class TagsServiceTests
    {
        private readonly Mock<ITagsRepository> _tagsRepositoryMock;
        private readonly TagsService _tagsService;

        public TagsServiceTests()
        {
            _tagsRepositoryMock = new Mock<ITagsRepository>();
            _tagsService = new TagsService(_tagsRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Create_TagIsNull_ShouldThrowArgumentNullException()
        {
            Tag tag = null;
            CreateResult createResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => createResult = await _tagsService.Create(tag!));

            _tagsRepositoryMock.Verify(tr => tr.Create(tag!), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(createResult);
            Assert.AreEqual(ErrorStrings.TAG_RARAM_NAME, exception.ParamName);
        }


        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Create_TagNameIsNullOrWhiteSpace_ShouldReturnCreateResult_TAG_NAME_SHOULD_NOT_BE_EMPTY(string tagName)
        {
            var tag = new Tag(id: 0, name: tagName);

            var createResult = await _tagsService.Create(tag);
            _tagsRepositoryMock.Verify(tr => tr.Create(tag!), Times.Never);
            
            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TAG_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);

        }

        [TestMethod]
        public async Task Create_TagIsValid_ShouldReturnOk()
        {
            string tagName = TestFixtures.TestFixtures.GenerateString();
            var tag = new Tag(id: 0, name: tagName);

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _tagsRepositoryMock.Setup(nr => nr.Create(tag))
                .ReturnsAsync(new CreateResult
                {
                    Id = expectedId,
                    StatusCode = System.Net.HttpStatusCode.Created
                });

            var createResult = await _tagsService.Create(tag);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _tagsRepositoryMock.Verify(nr => nr.Create(tag), Times.Once);
        }


        [TestMethod]
        public async Task Delete_NotFound_ShouldReturnDeleteResult_TAG_NOT_FOUND()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _tagsRepositoryMock.Setup(nr => nr.Delete(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = Core.ErrorStrings.TAG_NOT_FOUND
                });

            var deleteResult = await _tagsService.Delete(id);

            _tagsRepositoryMock.Verify(nr => nr.Delete(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.TAG_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_OK_ShouldReturnDeleteResult_OK()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _tagsRepositoryMock.Setup(nr => nr.Delete(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = Core.ErrorStrings.OK
                });

            var deleteResult = await _tagsService.Delete(id);

            _tagsRepositoryMock.Verify(nr => nr.Delete(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task GetTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task GetTagsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            throw new NotImplementedException();
        }
    }
}