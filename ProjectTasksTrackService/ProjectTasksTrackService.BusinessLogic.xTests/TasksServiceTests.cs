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
    public class TasksServiceTests
    {
        private readonly Mock<ITasksRepository> _tasksRepositoryMock;
        private readonly TasksService _tasksService;

        public TasksServiceTests()
        {
            _tasksRepositoryMock = new Mock<ITasksRepository>();
            _tasksService = new TasksService(_tasksRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_TaskIsNull_ShouldThrowArgumentNullException()
        {
            ProjectTask task = null;
            CreateResult createResult = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => createResult = await _tasksService.Create(task));

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Never);
            Assert.NotNull(exception);
            Assert.Null(createResult);
            Assert.Equal(ErrorStrings.TASK_PARAM_NAME, exception.ParamName);
        }

        [Fact]
        public async Task Create_TaskIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            ProjectTask task = null;
            CreateResult createResult = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => createResult = await _tasksService.Create(task));

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.TASK_PARAM_NAME);
            createResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.TASK_PARAM_NAME);
        }

        /// <summary>
        /// Тест возврата ошибки при заданном Code. Code у задачи формируется автоматически, а не задается, поэтому в Create должен отсутствовать
        /// </summary>
        [Fact]
        public async Task Create_TaskWithCode_ShouldReturnCreateResult_TASK_CODE_SHOULD_BE_EMPTY_400()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateCode: true);

            var createResult = await _tasksService.Create(task);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.TASK_CODE_SHOULD_BE_EMPTY_IN_CREATE, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }


        /// <summary>
        /// Тест возврата ошибки при заданном Code. Code у задачи формируется автоматически, а не задается, поэтому в Create должен отсутствовать
        /// </summary>
        [Fact]
        public async Task Create_TaskWithCode_ShouldReturnCreateResult_TASK_CODE_SHOULD_BE_EMPTY_400_Fluent()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateCode: true);
            var createResult = await _tasksService.Create(task);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.TASK_CODE_SHOULD_BE_EMPTY_IN_CREATE);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [Fact]
        public async Task Create_TaskWithoutName_ShouldReturnCreateResult_TASK_NAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateName: false, generateCode: false);

            var createResult = await _tasksService.Create(task);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.TASK_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }

        [Fact]
        public async Task Create_TaskWithoutName_ShouldReturnCreateResult_TASK_NAME_SHOULD_NOT_BE_EMPTY_400_Fluent()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateName: false, generateCode: false);
            var createResult = await _tasksService.Create(task);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.TASK_NAME_SHOULD_NOT_BE_EMPTY);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }


        [Fact]
        public async Task Create_TaskWithNotZeroId_ShouldReturnCreateResult_TASK_ID_SHOULD_BE_ZERO_400()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true, generateCode: false);
            var createResult = await _tasksService.Create(task);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.TASK_ID_SHOULD_BE_ZERO, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }

        [Fact]
        public async Task Create_TaskWithNotZeroId_ShouldReturnCreateResult_TASK_ID_SHOULD_BE_ZERO_Fluent()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true, generateCode: false);
            var createResult = await _tasksService.Create(task);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.TASK_ID_SHOULD_BE_ZERO);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }



        [Fact]
        public async Task Create_TaskIsValidAndFull_ShouldReturnOk()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateCode: false);

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _tasksRepositoryMock.Setup(sr => sr.Add(task, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _tasksService.Create(task);

            Assert.True(createResult.Id > 0);
            Assert.Equal(expectedId, createResult.Id);
            Assert.Equal(System.Net.HttpStatusCode.Created, createResult.StatusCode);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Once);
        }

        [Fact]
        public async Task Create_TaskIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateCode: false);

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _tasksRepositoryMock.Setup(sr => sr.Add(task, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _tasksService.Create(task);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Once);
        }

        [Fact]
        public async Task Create_TaskIsValid_ShouldReturnOk()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithRequiredFields(generateCode: false);

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _tasksRepositoryMock.Setup(sr => sr.Add(task, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _tasksService.Create(task);

            Assert.True(createResult.Id > 0);
            Assert.Equal(expectedId, createResult.Id);
            Assert.Equal(System.Net.HttpStatusCode.Created, createResult.StatusCode);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Once);
        }

        [Fact]
        public async Task Create_TaskIsValid_ShouldReturnOk_FluentAssertion()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithRequiredFields(generateCode: false);

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _tasksRepositoryMock.Setup(sr => sr.Add(task, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _tasksService.Create(task);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _tasksRepositoryMock.Verify(sr => sr.Add(task, false), Times.Once);
        }

        [Fact]
        public async Task Import_TasksIsNull_ShouldThrowArgumentNullException()
        {
            IEnumerable<ProjectTask> tasks = null;
            ImportResult importResult = null;
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => importResult = await _tasksService.Import(tasks));

            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Never);
            Assert.NotNull(exception);
            Assert.Null(importResult);
            Assert.Equal(ErrorStrings.TASKS_PARAM_NAME, exception.ParamName);
        }

        [Fact]
        public async Task Import_TasksIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            IEnumerable<ProjectTask> tasks = null;
            ImportResult importResult = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => importResult = await _tasksService.Import(tasks));

            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.TASKS_PARAM_NAME);
            importResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.TASKS_PARAM_NAME);
        }

        [Fact]
        public async Task Import_TasksIsEmpty_ShouldReturnImportResult_TASKS_LIST_TO_IMPORT_SHOULD_BE_FILLED()
        {
            IEnumerable<ProjectTask> tasks = [];
            var importResult = await _tasksService.Import(tasks);

            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Never);
            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.TASKS_LIST_TO_IMPORT_SHOULD_BE_FILLED, importResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_TasksIsEmpty_ShouldReturnImportResult_TASKS_LIST_TO_IMPORT_SHOULD_BE_FILLED_FluentAssertion()
        {
            IEnumerable<ProjectTask> tasks = [];
            var importResult = await _tasksService.Import(tasks);

            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.TASKS_LIST_TO_IMPORT_SHOULD_BE_FILLED);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Import_AlreadyImportedTasks_ShouldReturnImportResult_ALREADY_IMPORTED()
        {
            var tasks = TestFixtures.TestFixtures.GenerateTasksList(3);

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(tasks);

            var importResult = await _tasksService.Import(tasks);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Never);

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.ALREADY_IMPORTED, importResult.Message);
            Assert.Equal(0, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_AlreadyImportedTasks_ShouldReturnImportResult_ALREADY_IMPORTED_FluentAssertion()
        {
            var tasks = TestFixtures.TestFixtures.GenerateTasksList(3);

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(tasks);

            var importResult = await _tasksService.Import(tasks);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Never);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.ALREADY_IMPORTED);
            importResult.ImportedCount.Should().Be(0);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        /// <summary> Тест импорта набора из 5 задач, с конфликтами в задачах с индексами [0], [2], [4] </summary>
        [Fact]
        public async Task Import_TasksWithConflicts1_3_5_ShouldReturnImportResult_TASKS_CONFLICTS()
        {
            (List<ProjectTask> existingTasks, List<ProjectTask> taskToImport) =
               TestFixtures.TestFixtures.Simulate10TasksWithConflicts1_3_5_ToImport();

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(existingTasks);
            var expectedMessage = $"{ErrorStrings.TASKS_CONFLICTS}:{existingTasks[0].Id},{existingTasks[2].Id},{existingTasks[4].Id}";

            var importResult = await _tasksService.Import(taskToImport);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(taskToImport), Times.Never);

            Assert.NotNull(importResult);
            Assert.Equal(expectedMessage, importResult.Message);
            Assert.Equal(0, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.Conflict, importResult.StatusCode);
        }

        /// <summary> Тест импорта набора из 5 задач, с конфликтами в задачах с индексами [0], [2], [4] </summary>
        [Fact]
        public async Task Import_TasksWithConflicts1_3_5_ShouldReturnImportResult_TASKS_CONFLICTS_FluentAssertion()
        {
            (List<ProjectTask> existingTasks, List<ProjectTask> tasksToImport) =
               TestFixtures.TestFixtures.Simulate10TasksWithConflicts1_3_5_ToImport();

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(existingTasks);
            var expectedMessage = $"{ErrorStrings.TASKS_CONFLICTS}:{existingTasks[0].Id},{existingTasks[2].Id},{existingTasks[4].Id}";

            var importResult = await _tasksService.Import(tasksToImport);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasksToImport), Times.Never);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(expectedMessage);
            importResult.ImportedCount.Should().Be(0);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }


        [Fact]
        public async Task Import_PartiallyImportedTasks_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            var tasks = TestFixtures.TestFixtures.GenerateTasksList(10);

            var existingTasks = TestFixtures.TestFixtures.ReturnSomeOfEntities<ProjectTask>(tasks);
            var newTasks = tasks.Except(existingTasks).ToList();
            var expectedImportedCount = newTasks.Count;

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(existingTasks);
            _tasksRepositoryMock.Setup(sr => sr.Import(newTasks))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = newTasks.Count });

            var importResult = await _tasksService.Import(tasks);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(newTasks), Times.Once);

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_PartiallyImportedTasks_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            var tasks = TestFixtures.TestFixtures.GenerateTasksList(10);

            var existingTasks = TestFixtures.TestFixtures.ReturnSomeOfEntities<ProjectTask>(tasks);
            var newTasks = tasks.Except(existingTasks).ToList();
            var expectedImportedCount = newTasks.Count;

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(existingTasks);
            _tasksRepositoryMock.Setup(sr => sr.Import(newTasks))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = newTasks.Count });

            var importResult = await _tasksService.Import(tasks);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(newTasks), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException()
        {
            var tasks = TestFixtures.TestFixtures.GenerateTasksList(3);
            List<ProjectTask> emptyTasksList = [];
            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(emptyTasksList);
            var importResultExpectedMessage =
                $"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK}: {System.Net.HttpStatusCode.BadRequest}. {Core.ErrorStrings.TASKS_SHOULD_CONTAIN_AT_LEAST_1_TASK}";
            _tasksRepositoryMock.Setup(sr => sr.Import(tasks))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0 });

            ImportResult importResult = null;
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => importResult = await _tasksService.Import(tasks));

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Once);

            Assert.NotNull(exception);
            Assert.Null(importResult);
            Assert.Equal(importResultExpectedMessage, exception.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException_FluentAssertion()
        {
            var tasks = TestFixtures.TestFixtures.GenerateTasksList(3);
            List<ProjectTask> emptyTaskList = [];
            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(emptyTaskList);
            var importResultExpectedMessage =
                $"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK}: {System.Net.HttpStatusCode.BadRequest}. {Core.ErrorStrings.TASKS_SHOULD_CONTAIN_AT_LEAST_1_TASK}";
            _tasksRepositoryMock.Setup(sr => sr.Import(tasks))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0 });

            ImportResult importResult = null;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => importResult = await _tasksService.Import(tasks));

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Once);

            importResult.Should().BeNull();
            exception.Should().NotBeNull().And.Match<InvalidOperationException>(
                e => string.Equals(e.Message, importResultExpectedMessage));
        }

        [Fact]
        public async Task Import_NewImportedTasks_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            (List<ProjectTask> existingTasks, List<ProjectTask> tasksToImport) =
                TestFixtures.TestFixtures.Simulate5And3TasksWithoutConflicts();

            var expectedImportedCount = tasksToImport.Count;

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(existingTasks);
            _tasksRepositoryMock.Setup(sr => sr.Import(tasksToImport))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _tasksService.Import(tasksToImport);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasksToImport), Times.Once);

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_NewImportedTasks_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            (List<ProjectTask> existingTasks, List<ProjectTask> tasksToImport) =
                TestFixtures.TestFixtures.Simulate5And3TasksWithoutConflicts();

            var expectedImportedCount = tasksToImport.Count;

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(existingTasks);
            _tasksRepositoryMock.Setup(sr => sr.Import(tasksToImport))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _tasksService.Import(tasksToImport);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasksToImport), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Import_NewImportedTasksWhenNoExistingTasks_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            var tasks = TestFixtures.TestFixtures.GenerateTasksList(7);

            var expectedImportedCount = tasks.Count;
            var existingTasks = new List<ProjectTask>();

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(existingTasks);
            _tasksRepositoryMock.Setup(sr => sr.Import(tasks))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _tasksService.Import(tasks);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Once);

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_NewImportedTasksWhenNoExistingTasks_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            var tasks = TestFixtures.TestFixtures.GenerateTasksList(7);

            var expectedImportedCount = tasks.Count;
            var existingTasks = new List<ProjectTask>();

            _tasksRepositoryMock.Setup(sr => sr.GetAllTasks())
                .ReturnsAsync(existingTasks);
            _tasksRepositoryMock.Setup(sr => sr.Import(tasks))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _tasksService.Import(tasks);

            _tasksRepositoryMock.Verify(sr => sr.GetAllTasks(), Times.Once);
            _tasksRepositoryMock.Verify(sr => sr.Import(tasks), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(100500, null)]
        [InlineData(100500, 100)]

        public async Task DeleteTask_TaskNotFound_ShouldReturnDeleteResult_TASK_NOT_FOUND(int? projectId, int? subdivisionId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _tasksRepositoryMock.Setup(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.TASK_NOT_FOUND });

            var deleteResult = await _tasksService.DeleteTask(id, projectSecretString, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.TASK_NOT_FOUND, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(100500, null)]
        [InlineData(100500, 100)]
        public async Task DeleteTask_TaskNotFound_ShouldReturnDeleteResult_TASK_NOT_FOUND_FluentAssertion(int? projectId, int? subdivisionId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _tasksRepositoryMock.Setup(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.TASK_NOT_FOUND });

            var deleteResult = await _tasksService.DeleteTask(id, projectSecretString, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.TASK_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(null, 100500, null)]
        [InlineData("", 100, null)]
        [InlineData(" ", 200, null)]
        [InlineData(null, 100500, 1)]
        [InlineData("", 100500, null)]
        [InlineData(" ", null, 100500)]
        public async Task DeleteTask_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING(string projectSecretString, int? projectId, int? subdivisionId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _tasksRepositoryMock.Setup(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _tasksService.DeleteTask(id, projectSecretString, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null, 100500, null)]
        [InlineData("", 100, null)]
        [InlineData(" ", 200, null)]
        [InlineData(null, 100500, 1)]
        [InlineData("", 100500, null)]
        [InlineData(" ", null, 100500)]
        public async Task DeleteTask_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING_FluentAssertion(string projectSecretString, int? projectId, int? subdivisionId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _tasksRepositoryMock.Setup(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _tasksService.DeleteTask(id, projectSecretString, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(100500, null)]
        [InlineData(100500, 100)]
        public async Task DeleteTask_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING(int? projectId, int? subdivisionId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _tasksRepositoryMock.Setup(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _tasksService.DeleteTask(id, projectSecretString, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.INVALID_SECRET_STRING, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(100500, null)]
        [InlineData(100500, 100)]
        public async Task DeleteTask_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING_FluentAssertion(int? projectId, int? subdivisionId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _tasksRepositoryMock.Setup(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _tasksService.DeleteTask(id, projectSecretString, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.DeleteTask(id, projectSecretString, projectId, subdivisionId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.INVALID_SECRET_STRING);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }


        [Theory]
        [InlineData(null, null)]
        [InlineData(100500, null)]
        [InlineData(100500, 100)]
        public async Task DeleteTask_OK_ShouldReturnDeleteResult_OK(int? projectId, int? subdivisionId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var taskSecretString = TestFixtures.TestFixtures.GenerateString();

            _tasksRepositoryMock.Setup(sr => sr.DeleteTask(id, taskSecretString, projectId, subdivisionId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _tasksService.DeleteTask(id, taskSecretString, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.DeleteTask(id, taskSecretString, projectId, subdivisionId), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(100500, null)]
        [InlineData(100500, 100)]
        public async Task DeleteTask_OK_ShouldReturnDeleteResult_OK_FluentAssertion(int? projectId, int? subdivisionId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var taskSecretString = TestFixtures.TestFixtures.GenerateString();

            _tasksRepositoryMock.Setup(sr => sr.DeleteTask(id, taskSecretString, projectId, subdivisionId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _tasksService.DeleteTask(id, taskSecretString, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.DeleteTask(id, taskSecretString, projectId, subdivisionId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(10, null, 3)]
        [InlineData(1, 3, 4)]
        [InlineData(2, 2, null)]
        [InlineData(3, null, null)]
        [InlineData(0, 100500, null)]
        [InlineData(1, 100500, 100500)]
        [InlineData(2, 100500, null)]
        [InlineData(3, 100500, 10)]
        public async Task GetTask_ExistingTaskById_ShouldReturnTask(int id, int? projectId, int? subdivisionId)
        {
            ProjectTask task = null;
            var setProjectId = projectId != null ? projectId.Value : 0;
            var existingSub = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(setId: id, setProjectId: setProjectId);
            _tasksRepositoryMock.Setup(sr => sr.GetTask(id, projectId, subdivisionId))
                .ReturnsAsync(existingSub);

            task = await _tasksService.GetTask(id, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.GetTask(id, projectId, subdivisionId), Times.Once);

            Assert.NotNull(task);
            Assert.Equal(id, task.Id);
            Assert.Equal(existingSub, task);
        }

        [Theory]
        [InlineData(0, null, 2)]
        [InlineData(1, null, 2)]
        [InlineData(2, 3, 4)]
        [InlineData(3, 5, 6)]
        [InlineData(0, 100500, null)]
        [InlineData(1, 100500, 100500)]
        [InlineData(2, 100500, 100500)]
        [InlineData(3, 100500, 100500)]
        public async Task GetTask_ExistingTaskById_ShouldReturnTask_FluentAssertion(int id, int? projectId, int? subdivisionId)
        {
            ProjectTask task = null;
            var setProjectId = projectId != null ? projectId.Value : 0;
            var existingSub = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(setId: id, setProjectId: setProjectId);
            _tasksRepositoryMock.Setup(sr => sr.GetTask(id, projectId, subdivisionId))
                .ReturnsAsync(existingSub);

            task = await _tasksService.GetTask(id, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.GetTask(id, projectId, subdivisionId), Times.Once);

            task.Should().NotBeNull();
            task.Id.Should().Be(id);
            task.Should().Be(existingSub);
        }


        [Theory]
        [InlineData(0, 1, null)]
        [InlineData(1, null, 2)]
        [InlineData(2, null, 3)]
        [InlineData(3, null, null)]
        [InlineData(0, 404, null)]
        [InlineData(1, 404, 5)]
        [InlineData(2, 404, 6)]
        [InlineData(3, 404, null)]
        public async Task GetTask_NotExistingTaskById_ShouldReturnNull(int id, int? projectId, int? subdivisionId)
        {
            ProjectTask task = null;

            _tasksRepositoryMock.Setup(sr => sr.GetTask(id, projectId, subdivisionId))
                .ReturnsAsync(task);

            task = await _tasksService.GetTask(id, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.GetTask(id, projectId, subdivisionId), Times.Once);

            Assert.Null(task);
        }

        [Theory]
        [InlineData(0, 1, null)]
        [InlineData(1, null, 2)]
        [InlineData(2, null, 3)]
        [InlineData(3, null, null)]
        [InlineData(0, 404, null)]
        [InlineData(1, 404, 5)]
        [InlineData(2, 404, 6)]
        [InlineData(3, 404, null)]
        public async Task GetTask_NotExistingTaskById_ShouldReturnNull_FluentAssertion(int id, int? projectId, int? subdivisionId)
        {
            ProjectTask task = null;

            _tasksRepositoryMock.Setup(sr => sr.GetTask(id, projectId, subdivisionId))
                .ReturnsAsync(task);

            task = await _tasksService.GetTask(id, projectId, subdivisionId);

            _tasksRepositoryMock.Verify(sr => sr.GetTask(id, projectId, subdivisionId), Times.Once);

            task.Should().BeNull();
        }


        [Fact]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_TASK_CODE_SHOULD_NOT_BE_EMPTY()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Never);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.Equal(ErrorStrings.TASK_CODE_SHOULD_NOT_BE_EMPTY, updateResult.Message);
        }

        [Fact]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_TASK_CODE_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.TASK_CODE_SHOULD_NOT_BE_EMPTY);
        }


        [Fact]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_TASK_NAME_SHOULD_NOT_BE_EMPTY()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Never);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.Equal(ErrorStrings.TASK_NAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
        }

        [Fact]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_TASK_NAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.TASK_NAME_SHOULD_NOT_BE_EMPTY);
        }


        [Fact]
        public async Task Update_TaskIsValid_ShouldReturnUpdateResult_TASK_UPDATED()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true);

            _tasksRepositoryMock.Setup(sr => sr.UpdateTask(task))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.TASK_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Once);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.Equal(Core.ErrorStrings.TASK_UPDATED, updateResult.Message);
        }

        [Fact]
        public async Task Update_TaskIsValid_ShouldReturnUpdateResult_TASK_UPDATED_FluentAssertion()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true);

            _tasksRepositoryMock.Setup(sr => sr.UpdateTask(task))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.TASK_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            updateResult.Message.Should().Be(Core.ErrorStrings.TASK_UPDATED);
        }


        [Fact]
        public async Task Update_TaskIsActual_ShouldReturnUpdateResult_TASK_IS_ACTUAL()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true);

            _tasksRepositoryMock.Setup(sr => sr.UpdateTask(task))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.TASK_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Once);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.Equal(Core.ErrorStrings.TASK_IS_ACTUAL, updateResult.Message);
        }

        [Fact]
        public async Task Update_TaskIsActual_ShouldReturnUpdateResult_TASK_IS_ACTUAL_FluentAssertion()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true);

            _tasksRepositoryMock.Setup(sr => sr.UpdateTask(task))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.TASK_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            updateResult.Message.Should().Be(Core.ErrorStrings.TASK_IS_ACTUAL);
        }

        [Fact]
        public async Task Update_TaskNotFound_ShouldReturnUpdateResult_TASK_NOT_FOUND()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true);

            _tasksRepositoryMock.Setup(sr => sr.UpdateTask(task))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.TASK_NOT_FOUND, StatusCode = System.Net.HttpStatusCode.NotFound });

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Once);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);
            Assert.Equal(Core.ErrorStrings.TASK_NOT_FOUND, updateResult.Message);
        }

        [Fact]
        public async Task Update_TaskNotFound_ShouldReturnUpdateResult_TASK_NOT_FOUND_FluentAssertion()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true);

            _tasksRepositoryMock.Setup(sr => sr.UpdateTask(task))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.TASK_NOT_FOUND, StatusCode = System.Net.HttpStatusCode.NotFound });

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            updateResult.Message.Should().Be(Core.ErrorStrings.TASK_NOT_FOUND);
        }


        [Fact]
        public async Task Update_CodeChanging_ShouldReturnUpdateResult_TASK_CODE_SHOULD_BE_THE_SAME()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true);

            _tasksRepositoryMock.Setup(sr => sr.UpdateTask(task))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.TASK_CODE_SHOULD_BE_THE_SAME, StatusCode = System.Net.HttpStatusCode.Conflict });

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Once);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.Conflict, updateResult.StatusCode);
            Assert.Equal(Core.ErrorStrings.TASK_CODE_SHOULD_BE_THE_SAME, updateResult.Message);
        }

        [Fact]
        public async Task Update_CodeChanging_ShouldReturnUpdateResult_TASK_CODE_SHOULD_BE_THE_SAME_FluentAssertion()
        {
            var task = TestFixtures.TestFixtures.GetTaskFixtureWithAllFields(generateId: true);

            _tasksRepositoryMock.Setup(sr => sr.UpdateTask(task))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.TASK_CODE_SHOULD_BE_THE_SAME, StatusCode = System.Net.HttpStatusCode.Conflict });

            var updateResult = await _tasksService.UpdateTask(task);

            _tasksRepositoryMock.Verify(sr => sr.UpdateTask(task), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
            updateResult.Message.Should().Be(Core.ErrorStrings.TASK_CODE_SHOULD_BE_THE_SAME);
        }
    }
}
