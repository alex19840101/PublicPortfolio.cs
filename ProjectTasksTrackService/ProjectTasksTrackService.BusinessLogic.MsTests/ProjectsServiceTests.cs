﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Never);
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

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.PROJECT_PARAM_NAME);
            createResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.PROJECT_PARAM_NAME);
        }

        [TestMethod]
        public async Task Create_ProjectWithoutCode_ShouldReturnCreateResult_PROJECT_CODE_SHOULD_NOT_BE_EMPTY_400()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateCode: false);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Never);
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

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.PROJECT_CODE_SHOULD_NOT_BE_EMPTY);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [TestMethod]
        public async Task Create_ProjectWithoutCode_ShouldReturnCreateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateName: false);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.IsNull(createResult.Code);
        }

        [TestMethod]
        public async Task Create_ProjectWithoutCode_ShouldReturnCreateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY_400_Fluent()
        {
            var project = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateName: false);
            var createResult = await _projectsService.Create(project);

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Never);
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

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Never);
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

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Never);
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
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
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

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
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
            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
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

            _projectsRepositoryMock.Verify(repo => repo.Add(project, false), Times.Once);
        }

        [TestMethod]
        public async Task Import_ProjectsIsNull_ShouldThrowArgumentNullException()
        {
            IEnumerable<Core.Project> projects = null;
            ImportResult importResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => importResult = await _projectsService.Import(projects));

            _projectsRepositoryMock.Verify(repo => repo.Import(projects), Times.Never);
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

            _projectsRepositoryMock.Verify(repo => repo.Import(projects), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.PROJECTS_PARAM_NAME);
            importResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.PROJECTS_PARAM_NAME);
        }

        [TestMethod]
        public async Task Import_ProjectsIsEmpty_ShouldReturnImportResult_PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED()
        {
            IEnumerable<Core.Project> projects = [];
            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(repo => repo.Import(projects), Times.Never);
            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED, importResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [TestMethod]
        public async Task Import_ProjectsIsEmpty_ShouldReturnImportResult_PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED_FluentAssertion()
        {
            IEnumerable<Core.Project> projects = [];
            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(repo => repo.Import(projects), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Import_AlreadyImportedProjects_ShouldReturnImportResult_ALREADY_IMPORTED()
        {
            List<int> excludeIds = [];
            var project1 = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project2 = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project3 = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);

            IEnumerable<Core.Project> projects = new List<Core.Project>
            {
                project1,
                project2,
                project3,
            };

            _projectsRepositoryMock.Setup(pr => pr.GetAllProjects())
                .ReturnsAsync(projects);

            var importResult = await _projectsService.Import(projects);

            _projectsRepositoryMock.Verify(pr => pr.GetAllProjects(), Times.Once);
            _projectsRepositoryMock.Verify(repo => repo.Import(projects), Times.Never);
            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.ALREADY_IMPORTED, importResult.Message);
            Assert.AreEqual(0, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
        public async Task Import_AlreadyImportedProjects_ShouldReturnImportResult_ALREADY_IMPORTED_FluentAssertion()
        {
            List<int> excludeIds = [];
            var project1 = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project2 = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project3 = TestFixtures.TestFixtures.GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);

            IEnumerable<Core.Project> projects = new List<Core.Project>
            {
                project1,
                project2,
                project3,
            };

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
    }
}
