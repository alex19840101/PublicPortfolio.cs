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
        public void Create_TagIsNull_ShouldThrowArgumentNullException()
        {
            throw new NotImplementedException(); //TODO: Create_TagIsNull_ShouldThrowArgumentNullException()
        }


        [TestMethod]
        public void Create_TagNameIsNullOrWhiteSpace_ShouldReturnCreateResult_TAG_NAME_SHOULD_NOT_BE_EMPTY()
        {
            throw new NotImplementedException(); //TODO: Create_TagNameIsNullOrWhiteSpace_ShouldReturnCreateResult_TAG_NAME_SHOULD_NOT_BE_EMPTY
        }

        [TestMethod]
        public void Create_TagIsValid_ShouldReturnOk()
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
        public void GetTagsTest()
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