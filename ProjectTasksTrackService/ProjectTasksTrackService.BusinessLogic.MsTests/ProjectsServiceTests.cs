using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;

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
        public async Task Create_ProjectIsNull_ShouldThrowArgumentNullException()
        {
            Core.Project project = null;
            CreateResult createResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => createResult = await _projectsService.Create(project));

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(createResult);
            Assert.AreEqual(ErrorStrings.PROJECT_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Create_ProjectIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Core.Project project = null;
            CreateResult createResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => createResult = await _projectsService.Create(project));

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.PROJECT_PARAM_NAME);
            createResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.PROJECT_PARAM_NAME);
        }

        [TestMethod]
        public async Task Create_ProjectWithoutCode_ShouldReturnCreateResult_PROJECT_CODE_SHOULD_NOT_BE_EMPTY_400()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateCode: false);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(ErrorStrings.PROJECT_CODE_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.IsNull(createResult.Code);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Create_ProjectWithoutName_ShouldReturnCreateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateName: false);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.IsNull(createResult.Code);
        }

        [TestMethod]
        public async Task Create_ProjectWithoutName_ShouldReturnCreateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY_400_Fluent()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateName: false);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [TestMethod]
        public async Task Create_ProjectWithNotNullId_ShouldReturnCreateResult_PROJECT_ID_SHOULD_BE_ZERO_400()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(ErrorStrings.PROJECT_ID_SHOULD_BE_ZERO, createResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.IsNull(createResult.Code);
        }

        [TestMethod]
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


        [TestMethod]
        public async Task Create_ProjectIsValidAndFull_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _projectsService.Create(project);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Once);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Create_ProjectIsValid_ShouldReturnOk()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.Add(project, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _projectsService.Create(project);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _projectsRepositoryMock.Verify(pr => pr.Add(project, false), Times.Once);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Import_ProjectsIsNull_ShouldThrowArgumentNullException()
        {
            IEnumerable<Core.Project> projects = null;
            ImportResult importResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(importResult);
            Assert.AreEqual(ErrorStrings.PROJECTS_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Import_ProjectsIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            IEnumerable<Core.Project> projects = null;
            ImportResult importResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.PROJECTS_PARAM_NAME);
            importResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.PROJECTS_PARAM_NAME);
        }

        [TestMethod]
        public async Task Import_ProjectsIsEmpty_ShouldReturnImportResult_PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED()
        {
            IEnumerable<Core.Project> projects = [];
            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED, importResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [TestMethod]
        public async Task Import_ProjectsIsEmpty_ShouldReturnImportResult_PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED_FluentAssertion()
        {
            IEnumerable<Core.Project> projects = [];
            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Import_AlreadyImportedProjects_ShouldReturnImportResult_ALREADY_IMPORTED()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(3);

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(projects);

            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(projects), Times.Never);
            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.ALREADY_IMPORTED, importResult.Message);
            Assert.AreEqual(0, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
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
        [TestMethod]
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
            Assert.IsNotNull(importResult);
            Assert.AreEqual(expectedMessage, importResult.Message);
            Assert.AreEqual(0, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, importResult.StatusCode);
        }

        /// <summary> Тест импорта набора из 5 проектов, с конфликтами в проектах с индексами [0], [2], [4] </summary>
        [TestMethod]
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


        [TestMethod]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException()
        {
            var projects = TestFixtures.TestFixtures.GenerateProjectsList(3);
            List<Project> emptyProjectList = [];
            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(projects);
            var importResultExpectedMessage = Core.ErrorStrings.PROJECTS_SHOULD_CONTAIN_AT_LEAST_1_PROJECT;
            _projectsRepositoryMock.Setup(pr => pr.Import(emptyProjectList))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0});

            ImportResult importResult = null;
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(emptyProjectList), Times.Once);
            
            Assert.IsNotNull(exception);
            Assert.IsNull(importResult);
            Assert.AreEqual($"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK} ({importResult.StatusCode}). Message: ({importResultExpectedMessage})", exception.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [TestMethod]
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
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(pr => pr.Import(emptyProjectList), Times.Once);
            importResult.Should().BeNull();
            exception.Should().NotBeNull().And.Match<InvalidOperationException>(e => string.Equals(e.Message, $"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK} ({importResult.StatusCode}). Message: ({importResultExpectedMessage})"));
        }


        [TestMethod]
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

            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.IMPORTED, importResult.Message);
            Assert.AreEqual(expectedImportedCount, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
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


        [TestMethod]
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

            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.IMPORTED, importResult.Message);
            Assert.AreEqual(expectedImportedCount, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
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

        [TestMethod]
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

            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.IMPORTED, importResult.Message);
            Assert.AreEqual(expectedImportedCount, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task DeleteProject_ProjectNotFound_ShouldReturnDeleteResult_PROJECT_NOT_FOUND()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.PROJECT_NOT_FOUND });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.PROJECT_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [TestMethod]
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

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task DeleteProject_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING(string projectSecretString)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
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

        [TestMethod]
        public async Task DeleteProject_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.INVALID_SECRET_STRING, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task DeleteProject_OK_ShouldReturnDeleteResult_OK()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteProject_OK_ShouldReturnDeleteResult_OK_FluentAssertion()
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _projectsRepositoryMock.Setup(pr => pr.DeleteProject(id, projectSecretString))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _projectsService.DeleteProject(id, projectSecretString);

            _projectsRepositoryMock.Verify(pr => pr.DeleteProject(id, projectSecretString), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetProject_NULL_EMPTY_PRMS_ShouldThrowInvalidOperationException()
        {
            Project project = null;
            int? id = null;
            string codeSubStr = null;
            string nameSubStr = null;

            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => project = await _projectsService.GetProject(id, codeSubStr, nameSubStr));

            Assert.IsNotNull(exception);
            Assert.IsNull(project);
            Assert.AreEqual($"{ErrorStrings.GET_PROJECT_CALLED_WITH_NULL_EMPTY_PRMS}", exception.Message);
        }

        [TestMethod]
        public async Task GetProject_NULL_EMPTY_PRMS_ShouldThrowInvalidOperationException_FluentAssertion()
        {
            Project project = null;
            int? id = null;
            string codeSubStr = null;
            string nameSubStr = null;
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => project = await _projectsService.GetProject(id, codeSubStr, nameSubStr));

            project.Should().BeNull();
            exception.Should().NotBeNull().And.Match<InvalidOperationException>(e => string.Equals(e.Message, $"{ErrorStrings.GET_PROJECT_CALLED_WITH_NULL_EMPTY_PRMS}"));
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
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

            Assert.IsNotNull(project);
            Assert.AreEqual(id, project.Id);
            Assert.AreEqual(existingProject, project);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
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


        [TestMethod]
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

            Assert.IsNull(project);
        }

        [TestMethod]
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


        [DataTestMethod]
        [DataRow(20, "cd", null)]
        [DataRow(50, "cod", "nam")]
        [DataRow(30, null, "nam")]
        [DataRow(null, "cde", null)]
        [DataRow(null, "cod", "nam")]
        [DataRow(null, null, "namE")]
        public async Task GetProject_ExistingProjectByCodeOrName_ShouldReturnProject(int? id, string codeSubStr, string nameSubStr)
        {
            Project project = null;
            var existingProject = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: id == null, setId: id ?? 0);
            if (!string.IsNullOrWhiteSpace(nameSubStr))
                existingProject.UpdateName($"{nameSubStr}{existingProject.Name}");
            
            if (!string.IsNullOrWhiteSpace(codeSubStr))
                existingProject = new Project(
                id: existingProject.Id,
                code: $"{codeSubStr}{existingProject.Code}",
                name: existingProject.Name,
                url: existingProject.Url,
                imageUrl: existingProject.ImageUrl,
                createdDt: DateTime.Now,
                lastUpdateDt: DateTime.Now);

            _projectsRepositoryMock.Setup(pr => pr.GetProject(id, codeSubStr, nameSubStr, true)).ReturnsAsync(existingProject);

            project = await _projectsService.GetProject(id, codeSubStr, nameSubStr);

            if (id != null) _projectsRepositoryMock.Verify(pr => pr.GetProjectById(id.Value), Times.Never);
            _projectsRepositoryMock.Verify(pr => pr.GetProject(id, codeSubStr, nameSubStr, true), Times.Once);

            Assert.IsNotNull(project);
            if (id != null) Assert.AreEqual(id, project.Id);
            Assert.AreEqual(existingProject, project);
        }

        [DataTestMethod]
        [DataRow(10, "cd", null)]
        [DataRow(60, "cod", "nam")]
        [DataRow(30, null, "nam")]
        [DataRow(null, "cde", null)]
        [DataRow(null, "cod", "nam")]
        [DataRow(null, null, "namE")]
        public async Task GetProject_ExistingProjectByCodeOrName_ShouldReturnProject_FluentAssertion(int? id, string codeSubStr, string nameSubStr)
        {
            Project project = null;
            var existingProject = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: id == null, setId: id ?? 0);
            if (!string.IsNullOrWhiteSpace(nameSubStr))
                existingProject.UpdateName($"{nameSubStr}{existingProject.Name}");

            if (!string.IsNullOrWhiteSpace(codeSubStr))
                existingProject = new Project(
                id: existingProject.Id,
                code: $"{codeSubStr}{existingProject.Code}",
                name: existingProject.Name,
                url: existingProject.Url,
                imageUrl: existingProject.ImageUrl,
                createdDt: DateTime.Now,
                lastUpdateDt: DateTime.Now);

            _projectsRepositoryMock.Setup(pr => pr.GetProject(id, codeSubStr, nameSubStr, true)).ReturnsAsync(existingProject);

            project = await _projectsService.GetProject(id, codeSubStr, nameSubStr);

            if (id != null) _projectsRepositoryMock.Verify(pr => pr.GetProjectById(id.Value), Times.Never);
            _projectsRepositoryMock.Verify(pr => pr.GetProject(id, codeSubStr, nameSubStr, true), Times.Once);

            project.Should().NotBeNull();
            if (id != null) project.Id.Should().Be(id);
            project.Should().Be(existingProject);
        }


        [TestMethod]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_PROJECT_CODE_SHOULD_NOT_BE_EMPTY()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Never);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.AreEqual(ErrorStrings.PROJECT_CODE_SHOULD_NOT_BE_EMPTY, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_PROJECT_CODE_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.PROJECT_CODE_SHOULD_NOT_BE_EMPTY);
        }


        [TestMethod]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Never);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.AreEqual(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY);
        }


        [TestMethod]
        public async Task Update_ProjectIsValid_ShouldReturnUpdateResult_PROJECT_UPDATED()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);

            _projectsRepositoryMock.Setup(pr => pr.UpdateProject(project))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.PROJECT_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Once);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.AreEqual(Core.ErrorStrings.PROJECT_UPDATED, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_ProjectIsValid_ShouldReturnUpdateResult_PROJECT_UPDATED_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);

            _projectsRepositoryMock.Setup(pr => pr.UpdateProject(project))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.PROJECT_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            updateResult.Message.Should().Be(Core.ErrorStrings.PROJECT_UPDATED);
        }


        [TestMethod]
        public async Task Update_ProjectIsActual_ShouldReturnUpdateResult_PROJECT_IS_ACTUAL()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);

            _projectsRepositoryMock.Setup(pr => pr.UpdateProject(project))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.PROJECT_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Once);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.AreEqual(Core.ErrorStrings.PROJECT_IS_ACTUAL, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_ProjectIsActual_ShouldReturnUpdateResult_PROJECT_IS_ACTUAL_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);

            _projectsRepositoryMock.Setup(pr => pr.UpdateProject(project))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.PROJECT_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            updateResult.Message.Should().Be(Core.ErrorStrings.PROJECT_IS_ACTUAL);
        }


        [TestMethod]
        public async Task Update_ProjectNotFound_ShouldReturnUpdateResult_PROJECT_NOT_FOUND()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);

            _projectsRepositoryMock.Setup(pr => pr.UpdateProject(project))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.PROJECT_NOT_FOUND, StatusCode = System.Net.HttpStatusCode.NotFound });

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Once);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);
            Assert.AreEqual(Core.ErrorStrings.PROJECT_NOT_FOUND, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_ProjectNotFound_ShouldReturnUpdateResult_PROJECT_NOT_FOUND_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);

            _projectsRepositoryMock.Setup(pr => pr.UpdateProject(project))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.PROJECT_NOT_FOUND, StatusCode = System.Net.HttpStatusCode.NotFound });

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            updateResult.Message.Should().Be(Core.ErrorStrings.PROJECT_NOT_FOUND);
        }


        [TestMethod]
        public async Task Update_CodeChanging_ShouldReturnUpdateResult_CODE_SHOULD_BE_THE_SAME()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);

            _projectsRepositoryMock.Setup(pr => pr.UpdateProject(project))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.CODE_SHOULD_BE_THE_SAME, StatusCode = System.Net.HttpStatusCode.Conflict });

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Once);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, updateResult.StatusCode);
            Assert.AreEqual(Core.ErrorStrings.CODE_SHOULD_BE_THE_SAME, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_CodeChanging_ShouldReturnUpdateResult_CODE_SHOULD_BE_THE_SAME_FluentAssertion()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true);

            _projectsRepositoryMock.Setup(pr => pr.UpdateProject(project))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.CODE_SHOULD_BE_THE_SAME, StatusCode = System.Net.HttpStatusCode.Conflict });

            var updateResult = await _projectsService.UpdateProject(project);

            _projectsRepositoryMock.Verify(pr => pr.UpdateProject(project), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
            updateResult.Message.Should().Be(Core.ErrorStrings.CODE_SHOULD_BE_THE_SAME);
        }
    }
}
