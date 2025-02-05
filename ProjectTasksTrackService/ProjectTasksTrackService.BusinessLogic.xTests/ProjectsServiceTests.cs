using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;

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
        public async Task Create_ProjectIsNull_ShouldThrowArgumentNullException()
        {
            Core.Project project = null;
            CreateResult createResult = null;
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => createResult = await _projectsService.Create(project));

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            Assert.NotNull(exception);
            Assert.Null(createResult);
            Assert.Equal(ErrorStrings.PROJECT_PARAM_NAME, exception.ParamName);
        }

        [Fact]
        public async Task Create_ProjectIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Core.Project project = null;
            CreateResult createResult = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => createResult = await _projectsService.Create(project));

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.PROJECT_PARAM_NAME);
            createResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.PROJECT_PARAM_NAME);
        }

        [Fact]
        public async Task Create_ProjectWithoutCode_ShouldReturnCreateResult_PROJECT_CODE_SHOULD_NOT_BE_EMPTY_400()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateCode: false);

            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.PROJECT_CODE_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }

        [Fact]
        public async Task Create_ProjectWithoutCode_ShouldReturnCreateResult_PROJECT_CODE_SHOULD_NOT_BE_EMPTY_400_Fluent()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateCode: false);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.PROJECT_CODE_SHOULD_NOT_BE_EMPTY);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [Fact]
        public async Task Create_ProjectWithoutCode_ShouldReturnCreateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateName: false);

            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }


        [Fact]
        public async Task Create_ProjectWithNotNullId_ShouldReturnCreateResult_PROJECT_ID_SHOULD_BE_ZERO_400()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.PROJECT_ID_SHOULD_BE_ZERO, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }

        [Fact]
        public async Task Create_ProjectWithNotNullId_ShouldReturnCreateResult_PROJECT_ID_SHOULD_BE_ZERO_Fluent()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.PROJECT_ID_SHOULD_BE_ZERO);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [Fact]
        public async Task Create_ProjectWithoutCode_ShouldReturnCreateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY_400_Fluent()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateName: false);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [Fact]
        public async Task Create_ProjectIsValidAndFull_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _projectsService.Create(project);

            Assert.True(createResult.Id > 0);
            Assert.Equal(expectedId, createResult.Id);
            Assert.Equal(System.Net.HttpStatusCode.Created, createResult.StatusCode);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _projectsService.Create(project);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValid_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _projectsService.Create(project);

            Assert.True(createResult.Id > 0);
            Assert.Equal(expectedId, createResult.Id);
            Assert.Equal(System.Net.HttpStatusCode.Created, createResult.StatusCode);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Once);
        }

        [Fact]
        public async Task Create_ProjectIsValid_ShouldReturnOk_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _projectsService.Create(project);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Once);
        }

        [Fact]
        public async Task Import_ProjectsIsNull_ShouldThrowArgumentNullException()
        {
            IEnumerable<Core.Project> projects = null;
            ImportResult importResult = null;
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            Assert.NotNull(exception);
            Assert.Null(importResult);
            Assert.Equal(ErrorStrings.PROJECTS_PARAM_NAME, exception.ParamName);
        }

        [Fact]
        public async Task Import_ProjectsIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            IEnumerable<Core.Project> projects = null;
            ImportResult importResult = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.PROJECTS_PARAM_NAME);
            importResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.PROJECTS_PARAM_NAME);
        }

        [Fact]
        public async Task Import_ProjectsIsEmpty_ShouldReturnImportResult_PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED()
        {
            IEnumerable<Core.Project> projects = [];
            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED, importResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_ProjectsIsEmpty_ShouldReturnImportResult_PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED_FluentAssertion()
        {
            IEnumerable<Core.Project> projects = [];
            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Import_AlreadyImportedProjects_ShouldReturnImportResult_ALREADY_IMPORTED()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(3);

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(projects);

            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.ALREADY_IMPORTED, importResult.Message);
            Assert.Equal(0, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_AlreadyImportedProjects_ShouldReturnImportResult_ALREADY_IMPORTED_FluentAssertion()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(3);

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(projects);

            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.ALREADY_IMPORTED);
            importResult.ImportedCount.Should().Be(0);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        /// <summary> Тест импорта набора из 5 проектов, с конфликтами в проектах с индексами [0], [2], [4] </summary>
        [Fact]
        public async Task Import_ProjectsWithConflicts1_3_5_ShouldReturnImportResult_PROJECT_CONFLICTS()
        {
            (List<Core.Project> existingProjects, List<Core.Project> projectToImport) =
               TestFixtures.TestFixtures.Simulate10ProjectsWithConflicts1_3_5_ToImport();

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(existingProjects);
            var expectedMessage = $"{ErrorStrings.PROJECT_CONFLICTS}:{existingProjects[0].Id},{existingProjects[2].Id},{existingProjects[4].Id}";

            var importResult = await _projectsService.Import(projectToImport);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projectToImport), Times.Never);
            Assert.NotNull(importResult);
            Assert.Equal(expectedMessage, importResult.Message);
            Assert.Equal(0, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.Conflict, importResult.StatusCode);
        }

        /// <summary> Тест импорта набора из 5 проектов, с конфликтами в проектах с индексами [0], [2], [4] </summary>
        [Fact]
        public async Task Import_ProjectsWithConflicts1_3_5_ShouldReturnImportResult_PROJECT_CONFLICTS_FluentAssertion()
        {
            (List<Core.Project> existingProjects, List<Core.Project> projectToImport) =
               TestFixtures.TestFixtures.Simulate10ProjectsWithConflicts1_3_5_ToImport();

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(existingProjects);
            var expectedMessage = $"{ErrorStrings.PROJECT_CONFLICTS}:{existingProjects[0].Id},{existingProjects[2].Id},{existingProjects[4].Id}";

            var importResult = await _projectsService.Import(projectToImport);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projectToImport), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(expectedMessage);
            importResult.ImportedCount.Should().Be(0);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }


        [Fact]
        public async Task Import_PartiallyImportedProjects_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(10);

            var existingProjects = TestFixtures.TestFixtures.ReturnSomeOfProjects(projects);
            var newProjects = projects.Except(existingProjects).ToList();
            var expectedImportedCount = newProjects.Count;

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(existingProjects);
            _projectsRepositoryMock.Setup(pr => pr.Import(newProjects))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = newProjects.Count });

            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(newProjects), Times.Once);

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_PartiallyImportedProjects_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(10);

            var existingProjects = TestFixtures.TestFixtures.ReturnSomeOfProjects(projects);
            var newProjects = projects.Except(existingProjects).ToList();
            var expectedImportedCount = newProjects.Count;

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(existingProjects);
            _projectsRepositoryMock.Setup(pr => pr.Import(newProjects))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = newProjects.Count });

            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(newProjects), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(3);
            List<Project> emptyProjectList = [];
            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(projects);
            var importResultExpectedMessage = Core.ErrorStrings.PROJECTS_SHOULD_CONTAIN_AT_LEAST_1_PROJECT;
            _projectsRepositoryMock.Setup(pr => pr.Import(emptyProjectList))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0 });

            ImportResult importResult = null;
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(emptyProjectList), Times.Once);

            Assert.NotNull(exception);
            Assert.Null(importResult);
            Assert.Equal($"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK} ({importResult.StatusCode}). Message: ({importResultExpectedMessage})", exception.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException_FluentAssertion()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(3);
            List<Project> emptyProjectList = [];

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(projects);
            var importResultExpectedMessage = Core.ErrorStrings.PROJECTS_SHOULD_CONTAIN_AT_LEAST_1_PROJECT;
            _projectsRepositoryMock.Setup(pr => pr.Import(emptyProjectList))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0 });

            ImportResult importResult = null;
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(emptyProjectList), Times.Once);
            importResult.Should().BeNull();
            exception.Should().NotBeNull().And.Match<InvalidOperationException>(e => string.Equals(e.Message, $"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK} ({importResult.StatusCode}). Message: ({importResultExpectedMessage})"));
        }

        [Fact]
        public async Task Import_NewImportedProjects_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            (List<Core.Project> existingProjects, List<Core.Project> projectToImport) =
               TestFixtures.TestFixtures.Simulate5And3ProjectsWithoutConflicts();

            var expectedImportedCount = projectToImport.Count;

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(existingProjects);
            _projectsRepositoryMock.Setup(pr => pr.Import(projectToImport))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _projectsService.Import(projectToImport);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projectToImport), Times.Once);

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_NewImportedProjects_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            (List<Core.Project> existingProjects, List<Core.Project> projectToImport) =
               TestFixtures.TestFixtures.Simulate5And3ProjectsWithoutConflicts();

            var expectedImportedCount = projectToImport.Count;

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(existingProjects);
            _projectsRepositoryMock.Setup(pr => pr.Import(projectToImport))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _projectsService.Import(projectToImport);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projectToImport), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Import_NewImportedProjectsWhenNoExistingProjects_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(7);

            var expectedImportedCount = projects.Count;
            var existingProjects = new List<Project>();

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(existingProjects);
            _projectsRepositoryMock.Setup(pr => pr.Import(projects))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Once);

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_NewImportedProjectsWhenNoExistingProjects_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(7);

            var expectedImportedCount = projects.Count;
            var existingProjects = new List<Project>();

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(existingProjects);
            _projectsRepositoryMock.Setup(pr => pr.Import(projects))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task DeleteProject_ProjectNotFound_ShouldReturnDeleteResult_PROJECT_NOT_FOUND()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.PROJECT_NOT_FOUND });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.PROJECT_NOT_FOUND, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProject_ProjectNotFound_ShouldReturnDeleteResult_PROJECT_NOT_FOUND_FluentAssertion()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.PROJECT_NOT_FOUND });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.PROJECT_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task DeleteProject_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING(string projectSecretString)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task DeleteProject_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING_FluentAssertion(string projectSecretString)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteProject_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.INVALID_SECRET_STRING, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProject_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING_FluentAssertion()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.INVALID_SECRET_STRING);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }


        [Fact]
        public async Task DeleteProject_OK_ShouldReturnDeleteResult_OK()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProject_OK_ShouldReturnDeleteResult_OK_FluentAssertion()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.INVALID_SECRET_STRING);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task GetProject_NULL_EMPTY_PRMS_ShouldThrowInvalidOperationException()
        {
            Project project = null;
            int? id = null;
            string codeSubStr = null;
            string nameSubStr = null;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => project = await _projectsService.GetProject(id, codeSubStr, nameSubStr));

            Assert.NotNull(exception);
            Assert.Null(project);
            Assert.Equal($"{ErrorStrings.GET_PROJECT_CALLED_WITH_NULL_EMPTY_PRMS}", exception.Message);

        }

        [Fact]
        public async Task GetProject_NULL_EMPTY_PRMS_ShouldThrowInvalidOperationException_FluentAssertion()
        {
            Project project = null;
            int? id = null;
            string codeSubStr = null;
            string nameSubStr = null;
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => project = await _projectsService.GetProject(id, codeSubStr, nameSubStr));

            project.Should().BeNull();
            exception.Should().NotBeNull().And.Match<InvalidOperationException>(e => string.Equals(e.Message, $"{ErrorStrings.GET_PROJECT_CALLED_WITH_NULL_EMPTY_PRMS}"));
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetProject_ExistingProjectById_ShouldReturnProject(int id)
        {
            Project project = null;
            string codeSubStr = null;
            string nameSubStr = null;
            var existingProject = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(setId: id);
            _projectsRepositoryMock.Setup(pr => pr.GetProjectById(id))
                .ReturnsAsync(existingProject);

            project = await _projectsService.GetProject(id, codeSubStr, nameSubStr);

            _projectsRepositoryMock.Verify(pr => pr.GetProjectById(id), Times.Once);

            Assert.NotNull(project);
            Assert.Equal(id, project.Id);
            Assert.Equal(existingProject, project);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetProject_ExistingProjectById_ShouldReturnProject_FluentAssertion(int id)
        {
            Project project = null;
            string codeSubStr = null;
            string nameSubStr = null;
            var existingProject = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(setId: id);
            _projectsRepositoryMock.Setup(pr => pr.GetProjectById(id))
                .ReturnsAsync(existingProject);

            project = await _projectsService.GetProject(id, codeSubStr, nameSubStr);

            _projectsRepositoryMock.Verify(pr => pr.GetProjectById(id), Times.Once);

            project.Should().NotBeNull();
            project.Id.Should().Be(id);
            project.Should().Be(existingProject);
        }


        [Fact]
        public async Task GetProject_NotExistingProjectById_ShouldReturnNull()
        {
            Project project = null;
            int id = int.MinValue;
            string codeSubStr = null;
            string nameSubStr = null;

            _projectsRepositoryMock.Setup(pr => pr.GetProjectById(id))
                .ReturnsAsync(project);

            project = await _projectsService.GetProject(id, codeSubStr, nameSubStr);

            _projectsRepositoryMock.Verify(pr => pr.GetProjectById(id), Times.Once);

            Assert.Null(project);
        }

        [Fact]
        public async Task GetProject_NotExistingProjectById_ShouldReturnNull_FluentAssertion()
        {
            Project project = null;
            int id = int.MinValue;
            string codeSubStr = null;
            string nameSubStr = null;

            _projectsRepositoryMock.Setup(pr => pr.GetProjectById(id))
                .ReturnsAsync(project);

            project = await _projectsService.GetProject(id, codeSubStr, nameSubStr);

            _projectsRepositoryMock.Verify(pr => pr.GetProjectById(id), Times.Once);

            project.Should().BeNull();
        }
    }
}
