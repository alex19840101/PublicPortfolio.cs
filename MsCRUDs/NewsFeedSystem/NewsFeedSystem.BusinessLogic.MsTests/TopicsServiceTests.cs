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
        public void Create_TopicIsNull_ShouldThrowArgumentNullException()
        {
            throw new NotImplementedException(); //TODO: Create_TopicIsNull_ShouldThrowArgumentNullException()
        }


        [TestMethod]
        public void Create_TopicNameIsNullOrWhiteSpace_ShouldReturnCreateResult_TOPIC_NAME_SHOULD_NOT_BE_EMPTY()
        {
            throw new NotImplementedException(); //TODO: Create_TopicNameIsNullOrWhiteSpace_ShouldReturnCreateResult_TOPIC_NAME_SHOULD_NOT_BE_EMPTY
        }

        [TestMethod]
        public void Create_TopicIsValid_ShouldReturnOk()
        {
            throw new NotImplementedException(); //TODO: Create_TagIsValid_ShouldReturnOk
        }

        [TestMethod]
        public void DeleteTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetTopicsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void UpdateTest()
        {
            throw new NotImplementedException();
        }
    }
}