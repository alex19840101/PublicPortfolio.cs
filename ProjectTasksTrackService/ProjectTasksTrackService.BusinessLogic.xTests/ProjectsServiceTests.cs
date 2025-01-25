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

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(expectedId);

            var id = await _projectsService.Create(project);

            Assert.True(id > 0);
            Assert.Equal(expectedId, id);
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(expectedId);

            var id = await _projectsService.Create(project);

            id.Should().BeGreaterThan(0);
            id.Should().Be(expectedId);
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValid_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(expectedId);

            var id = await _projectsService.Create(project);

            Assert.True(id > 0);
            Assert.Equal(expectedId, id);
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValid_ShouldReturnOk_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(expectedId);

            var id = await _projectsService.Create(project);

            id.Should().BeGreaterThan(0);
            id.Should().Be(expectedId);
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }
    }
}
