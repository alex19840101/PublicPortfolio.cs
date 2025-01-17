using AutoFixture;
using FluentAssertions;
using Moq;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.BusinessLogic.xTests
{
    public class ProjectsServiceTests
    {
        private readonly Mock<IProjectsRepository> _projectsRepositoryMock;
        private readonly ProjectsService _projectsService;

        public ProjectsServiceTests()
        {
            _projectsRepositoryMock = new Mock<IProjectsRepository>();
            _projectsService = new ProjectsService(_projectsRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_ProjectIsValidAndFull_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields();

            var intProjectId = await _projectsService.Create(project);

            Assert.Equal(project.IntProjectId, intProjectId);
            _projectsRepositoryMock.Verify(repo => repo.Add(project), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields();

            var intProjectId = await _projectsService.Create(project);

            intProjectId.Should().Be(project.IntProjectId);
            _projectsRepositoryMock.Verify(repo => repo.Add(project), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValid_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var intProjectId = await _projectsService.Create(project);

            Assert.Equal(project.IntProjectId, intProjectId);
            _projectsRepositoryMock.Verify(repo => repo.Add(project), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValid_ShouldReturnOk_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var intProjectId = await _projectsService.Create(project);

            intProjectId.Should().Be(project.IntProjectId);
            _projectsRepositoryMock.Verify(repo => repo.Add(project), Times.Once);
        }
    }
}
