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

            _tagsRepositoryMock.Setup(tr => tr.Create(tag))
                .ReturnsAsync(new CreateResult
                {
                    Id = expectedId,
                    StatusCode = System.Net.HttpStatusCode.Created
                });

            var createResult = await _tagsService.Create(tag);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _tagsRepositoryMock.Verify(tr => tr.Create(tag), Times.Once);
        }


        [TestMethod]
        public async Task Delete_NotFound_ShouldReturnDeleteResult_TAG_NOT_FOUND()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _tagsRepositoryMock.Setup(tr => tr.Delete(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = Core.ErrorStrings.TAG_NOT_FOUND
                });

            var deleteResult = await _tagsService.Delete(id);

            _tagsRepositoryMock.Verify(tr => tr.Delete(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.TAG_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_OK_ShouldReturnDeleteResult_OK()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _tagsRepositoryMock.Setup(tr => tr.Delete(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = Core.ErrorStrings.OK
                });

            var deleteResult = await _tagsService.Delete(id);

            _tagsRepositoryMock.Verify(tr => tr.Delete(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task Get_ExistingTag_ShouldReturnTag()
        {
            string tagName = TestFixtures.TestFixtures.GenerateString();
            var id = TestFixtures.TestFixtures.GenerateId();
            var existingTag = new Tag(id: id, name: tagName);

            _tagsRepositoryMock.Setup(tr => tr.Get(id))
                .ReturnsAsync(existingTag);

            var resultTag = await _tagsService.Get(id);

            _tagsRepositoryMock.Verify(tr => tr.Get(id), Times.Once);

            Assert.IsNotNull(resultTag);
            Assert.AreEqual(id, resultTag.Id);
            Assert.AreEqual(existingTag, resultTag);
        }

        [TestMethod]
        public async Task Get_ExistingTag_ShouldReturnNull()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            Tag notExistingTag = null;

            _tagsRepositoryMock.Setup(tr => tr.Get(id))
                .ReturnsAsync(notExistingTag);

            var resultTag = await _tagsService.Get(id);

            _tagsRepositoryMock.Verify(tr => tr.Get(id), Times.Once);

            Assert.IsNull(resultTag);
            Assert.AreEqual(notExistingTag, resultTag);
        }

        [DataTestMethod]
        [DataRow(100501, 100500)]
        [DataRow(2, 1)]
        public async Task GetTags_MinTagIdMoreThanMaxTagId_ShouldReturnEmptyList(int? minTagId, int? maxTagId)
        {
            var result = await _tagsService.GetTags((uint?)minTagId, (uint?)maxTagId);
            _tagsRepositoryMock.Verify(tr => tr.GetTags((uint?)minTagId, (uint?)maxTagId), Times.Never);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow(null, 100500)]
        [DataRow(100500, null)]
        [DataRow(100500, 100500)]
        [DataRow(1, 100)]
        public async Task GetTags_ShouldCallGetTagsMethodOfRepository(int? minTagId, int? maxTagId)
        {
            var result = await _tagsService.GetTags((uint?)minTagId, (uint?)maxTagId);
            _tagsRepositoryMock.Verify(tr => tr.GetTags((uint?)minTagId, (uint?)maxTagId), Times.Once);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            throw new NotImplementedException();
        }
    }
}