using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;

namespace ProjectTasksTrackService.BusinessLogic.MsTests
{
    [TestClass]
    public sealed class ProjectsServiceTests
    {
        private readonly Mock<IProjectsRepository> _projectsRepositoryMock;
        private readonly ProjectsService _projectsService;

        public ProjectsServiceTests()
        {
            _projectsRepositoryMock = new Mock<IProjectsRepository>();
            _projectsService = new ProjectsService(_projectsRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Create_ProjectIsValidAndFull_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields();

            var id = await _projectsService.Create(project);

            Assert.IsTrue(id > 0);
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }

        [TestMethod]
        public async Task Create_ProjectIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields();

            var id = await _projectsService.Create(project);

            id.Should().BeGreaterThan(0);
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }

        [TestMethod]
        public async Task Create_ProjectIsValid_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var id = await _projectsService.Create(project);

            Assert.IsTrue(id > 0);
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }

        [TestMethod]
        public async Task Create_ProjectIsValid_ShouldReturnOk_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var intProjectId = await _projectsService.Create(project);

            intProjectId.Should().BeGreaterThan(0);
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }
    }
}
