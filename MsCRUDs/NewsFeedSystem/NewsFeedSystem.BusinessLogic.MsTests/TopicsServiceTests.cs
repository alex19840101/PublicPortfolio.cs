using System;
using System.Threading.Tasks;
using Moq;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;

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

            _topicsRepositoryMock.Setup(tr => tr.Create(topic))
                .ReturnsAsync(new CreateResult
                {
                    Id = expectedId,
                    StatusCode = System.Net.HttpStatusCode.Created
                });

            var createResult = await _topicsService.Create(topic);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _topicsRepositoryMock.Verify(tr => tr.Create(topic), Times.Once);
        }

        [TestMethod]
        public async Task Delete_NotFound_ShouldReturnDeleteResult_TOPIC_NOT_FOUND()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _topicsRepositoryMock.Setup(tr => tr.Delete(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = Core.ErrorStrings.TOPIC_NOT_FOUND
                });

            var deleteResult = await _topicsService.Delete(id);

            _topicsRepositoryMock.Verify(tr => tr.Delete(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.TOPIC_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_OK_ShouldReturnDeleteResult_OK()
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _topicsRepositoryMock.Setup(tr => tr.Delete(id))
                .ReturnsAsync(new DeleteResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = Core.ErrorStrings.OK
                });

            var deleteResult = await _topicsService.Delete(id);

            _topicsRepositoryMock.Verify(tr => tr.Delete(id), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task Get_ExistingTopic_ShouldReturnTopic()
        {
            string topicName = TestFixtures.TestFixtures.GenerateString();
            var id = TestFixtures.TestFixtures.GenerateId();
            var existingTopic = new Topic(id: id, name: topicName);

            _topicsRepositoryMock.Setup(tr => tr.Get(id))
                .ReturnsAsync(existingTopic);

            var resultTopic = await _topicsService.Get(id);

            _topicsRepositoryMock.Verify(tr => tr.Get(id), Times.Once);

            Assert.IsNotNull(resultTopic);
            Assert.AreEqual(id, resultTopic.Id);
            Assert.AreEqual(existingTopic, resultTopic);
        }

        [TestMethod]
        public async Task Get_NotExistingTopic_ShouldReturnNull()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            Topic notExistingTopic = null;
            _topicsRepositoryMock.Setup(tr => tr.Get(id))
                .ReturnsAsync(notExistingTopic);

            var resultTopic = await _topicsService.Get(id);

            _topicsRepositoryMock.Verify(tr => tr.Get(id), Times.Once);

            Assert.IsNull(resultTopic);
            Assert.AreEqual(notExistingTopic, resultTopic);
        }

        [DataTestMethod]
        [DataRow(100501, 100500)]
        [DataRow(2, 1)]
        public async Task GetTopics_MinTopicIdMoreThanMaxTopicId_ShouldReturnEmptyList(int? minTopicId, int? maxTopicId)
        {
            var result = await _topicsService.GetTopics((uint?)minTopicId, (uint?)maxTopicId);
            _topicsRepositoryMock.Verify(tr => tr.GetTopics((uint?)minTopicId, (uint?)maxTopicId), Times.Never);
        }


        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow(null, 100500)]
        [DataRow(100500, null)]
        [DataRow(100500, 100500)]
        [DataRow(1, 100)]
        public async Task GetTopics_ShouldCallGetTopicsMethodOfRepository(int? minTopicId, int? maxTopicId)
        {
            var result = await _topicsService.GetTopics((uint?)minTopicId, (uint?)maxTopicId);
            _topicsRepositoryMock.Verify(tr => tr.GetTopics((uint?)minTopicId, (uint?)maxTopicId), Times.Once);
        }

        [TestMethod]
        public async Task Update_TagIsNull_ShouldThrowArgumentNullException()
        {
            Topic topic = null;
            UpdateResult updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _topicsService.Update(topic!));

            _topicsRepositoryMock.Verify(tr => tr.Update(topic!), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(updateResult);
            Assert.AreEqual(ErrorStrings.TOPIC_RARAM_NAME, exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Update_TopicNameIsNullOrWhiteSpace_ShouldReturnCreateResult_TOPIC_NAME_SHOULD_NOT_BE_EMPTY(string topicName)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var topic = new Topic(id: id, name: topicName);

            var createResult = await _topicsService.Update(topic);
            _topicsRepositoryMock.Verify(tr => tr.Update(topic!), Times.Never);

            Assert.IsNotNull(createResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TOPIC_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);
        }

        [TestMethod]
        public async Task Update_TopicNotFound_ShouldReturn_TOPIC_NOT_FOUND()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            string topicName = TestFixtures.TestFixtures.GenerateString();
            var topic = new Topic(id: id, name: topicName);
            _topicsRepositoryMock.Setup(tr => tr.Update(topic))
               .ReturnsAsync(new UpdateResult
               {
                   Message = Core.ErrorStrings.TOPIC_NOT_FOUND,
                   StatusCode = System.Net.HttpStatusCode.NotFound
               });

            var updateResult = await _topicsService.Update(topic);
            
            _topicsRepositoryMock.Verify(tr => tr.Update(topic!), Times.Once);
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TOPIC_NOT_FOUND, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_TopicIsActual_ShouldReturn_TOPIC_IS_ACTUAL()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            string topicName = TestFixtures.TestFixtures.GenerateString();
            var topic = new Topic(id: id, name: topicName);
            _topicsRepositoryMock.Setup(tr => tr.Update(topic))
               .ReturnsAsync(new UpdateResult
               {
                   Message = Core.ErrorStrings.TOPIC_IS_ACTUAL,
                   StatusCode = System.Net.HttpStatusCode.OK
               });

            var updateResult = await _topicsService.Update(topic);

            _topicsRepositoryMock.Verify(tr => tr.Update(topic!), Times.Once);
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TOPIC_IS_ACTUAL, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_TopicIsUpdated_ShouldReturn_TOPIC_UPDATED()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            string topicName = TestFixtures.TestFixtures.GenerateString();
            var topic = new Topic(id: id, name: topicName);
            _topicsRepositoryMock.Setup(tr => tr.Update(topic))
               .ReturnsAsync(new UpdateResult
               {
                   Message = Core.ErrorStrings.TOPIC_UPDATED,
                   StatusCode = System.Net.HttpStatusCode.OK
               });

            var updateResult = await _topicsService.Update(topic);

            _topicsRepositoryMock.Verify(tr => tr.Update(topic!), Times.Once);
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.AreEqual(ErrorStrings.TOPIC_UPDATED, updateResult.Message);
        }
    }
}