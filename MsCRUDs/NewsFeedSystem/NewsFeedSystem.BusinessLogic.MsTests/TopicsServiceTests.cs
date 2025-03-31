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
    public class TopicsServiceTests
    {
        private readonly Mock<ITopicsRepository> _topicsRepositoryMock;
        private readonly TopicsService _topicsService;

        public TopicsServiceTests()
        {
            _topicsRepositoryMock = new Mock<ITopicsRepository>();
            _topicsService = new TopicsService(_topicsRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Create_TopicIsNull_ShouldThrowArgumentNullException()
        {
            Topic topic = null;
            CreateResult createResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => createResult = await _topicsService.Create(topic!));

            _topicsRepositoryMock.Verify(tr => tr.Create(topic!), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(createResult);
            Assert.AreEqual(ErrorStrings.TOPIC_RARAM_NAME, exception.ParamName);
        }


        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Create_TopicNameIsNullOrWhiteSpace_ShouldReturnCreateResult_TOPIC_NAME_SHOULD_NOT_BE_EMPTY(string topicName)
        {
            var topic = new Topic(id: 0, name: topicName);

            var createResult = await _topicsService.Create(topic);
            _topicsRepositoryMock.Verify(tr => tr.Create(topic!), Times.Never);

            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TOPIC_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);
        }

        [TestMethod]
        public async Task Create_TopicIsValid_ShouldReturnOk()
        {
            string topicName = TestFixtures.TestFixtures.GenerateString();
            var topic = new Topic(id: 0, name: topicName);

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _topicsRepositoryMock.Setup(nr => nr.Create(topic))
                .ReturnsAsync(new CreateResult
                {
                    Id = expectedId,
                    StatusCode = System.Net.HttpStatusCode.Created
                });

            var createResult = await _topicsService.Create(topic);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _topicsRepositoryMock.Verify(nr => nr.Create(topic), Times.Once);
        }

        [TestMethod]
        public async Task Delete_NotFound_ShouldReturnDeleteResult_TOPIC_NOT_FOUND()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _topicsRepositoryMock.Setup(nr => nr.Delete(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = Core.ErrorStrings.TOPIC_NOT_FOUND
                });

            var deleteResult = await _topicsService.Delete(id);

            _topicsRepositoryMock.Verify(nr => nr.Delete(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.TOPIC_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_OK_ShouldReturnDeleteResult_OK()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _topicsRepositoryMock.Setup(nr => nr.Delete(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = Core.ErrorStrings.OK
                });

            var deleteResult = await _topicsService.Delete(id);

            _topicsRepositoryMock.Verify(nr => nr.Delete(id), Times.Once);

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
        public async Task GetTopicsTest()
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