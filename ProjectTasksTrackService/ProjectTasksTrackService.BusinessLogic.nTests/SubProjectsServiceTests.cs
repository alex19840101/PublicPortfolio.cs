using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.BusinessLogic.nTests
{
    public class SubProjectsServiceTests
    {
        private readonly Mock<ISubProjectsRepository> _subProjectsRepositoryMock;
        private readonly SubProjectsService _subProjectsService;

        public SubProjectsServiceTests()
        {
            _subProjectsRepositoryMock = new Mock<ISubProjectsRepository>();
            _subProjectsService = new SubProjectsService(_subProjectsRepositoryMock.Object);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Create_SubDivisionIsNull_ShouldThrowArgumentNullException()
        {
            ProjectSubDivision sub = null;
            CreateResult createResult = null;
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => createResult = await _subProjectsService.Create(sub));

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.That(exception != null);
            Assert.That(createResult, Is.EqualTo(null));
            Assert.That(exception.ParamName, Is.EqualTo(ErrorStrings.SUBPROJECT_PARAM_NAME));
        }

        [Test]
        public async Task Create_SubDivisionIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            ProjectSubDivision sub = null;
            CreateResult createResult = null;
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => createResult = await _subProjectsService.Create(sub));

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.SUBPROJECT_PARAM_NAME);
            createResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.SUBPROJECT_PARAM_NAME);
        }

        [Test]
        public async Task Create_SubWithoutCode_ShouldReturnCreateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateCode: false);
            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.That(createResult != null);
            Assert.That(createResult.Message, Is.EqualTo(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY));
            Assert.That(createResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            Assert.That(createResult.Code, Is.EqualTo(null));
        }

        [Test]
        public async Task Create_SubWithoutCode_ShouldReturnCreateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY_400_Fluent()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateCode: false);
            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [Test]
        public async Task Create_SubWithoutName_ShouldReturnCreateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateName: false);
            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.That(createResult != null);
            Assert.That(createResult.Message, Is.EqualTo(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY));
            Assert.That(createResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            Assert.That(createResult.Code, Is.EqualTo(null));
        }

        [Test]
        public async Task Create_SubWithoutName_ShouldReturnCreateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY_400_Fluent()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateName: false);
            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [Test]
        public async Task Create_SubWithNotNullId_ShouldReturnCreateResult_SUBPROJECT_ID_SHOULD_BE_ZERO_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);

            Assert.That(createResult != null);
            Assert.That(createResult.Message, Is.EqualTo(ErrorStrings.SUBPROJECT_ID_SHOULD_BE_ZERO));
            Assert.That(createResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            Assert.That(createResult.Code, Is.EqualTo(null));
        }

        [Test]
        public async Task Create_SubWithNotNullId_ShouldReturnCreateResult_SUBPROJECT_ID_SHOULD_BE_ZERO_Fluent()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);

            createResult.Should().NotBeNull();
            createResult.Message.Should().Be(ErrorStrings.SUBPROJECT_ID_SHOULD_BE_ZERO);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            createResult.Code.Should().BeNull();
        }

        [Test]
        public async Task Create_SubIsValidAndFull_ShouldReturnOk()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.Add(sub, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _subProjectsService.Create(sub);

            Assert.That(createResult.Id, Is.GreaterThan(0));
            Assert.That(createResult.Id, Is.EqualTo(expectedId));
            Assert.That(createResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Once);
        }

        [Test]
        public async Task Create_SubIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.Add(sub, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _subProjectsService.Create(sub);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Once);
        }

        [Test]
        public async Task Create_SubIsValid_ShouldReturnOk()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.Add(sub, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _subProjectsService.Create(sub);

            Assert.That(createResult.Id, Is.GreaterThan(0));
            Assert.That(createResult.Id, Is.EqualTo(expectedId));
            Assert.That(createResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Once);
        }

        [Test]
        public async Task Create_SubIsValid_ShouldReturnOk_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.Add(sub, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _subProjectsService.Create(sub);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Once);
        }

        [Test]
        public async Task Import_SubsIsNull_ShouldThrowArgumentNullException()
        {
            IEnumerable<ProjectSubDivision> subs = null;
            ImportResult importResult = null;
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            Assert.That(exception != null);
            Assert.That(importResult, Is.EqualTo(null));
            Assert.That(exception.ParamName, Is.EqualTo(ErrorStrings.SUBS_PARAM_NAME));
        }

        [Test]
        public async Task Import_SubsIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            IEnumerable<ProjectSubDivision> subs = null;
            ImportResult importResult = null;
            var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.PROJECTS_PARAM_NAME);
            importResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.SUBS_PARAM_NAME);
        }

        [Test]
        public async Task Import_SubsIsEmpty_ShouldReturnImportResult_SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED()
        {
            IEnumerable<ProjectSubDivision> subs = [];
            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);

            Assert.That(importResult != null);
            Assert.That(importResult.Message, Is.EqualTo(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED));
            Assert.That(importResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Import_SubsIsEmpty_ShouldReturnImportResult_SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED_FluentAssertion()
        {
            IEnumerable<ProjectSubDivision> subs = [];
            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Import_AlreadyImportedSubs_ShouldReturnImportResult_ALREADY_IMPORTED()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(3);

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(subs);

            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);

            Assert.That(importResult != null);
            Assert.That(importResult.Message, Is.EqualTo(ErrorStrings.ALREADY_IMPORTED));
            Assert.That(importResult.ImportedCount, Is.EqualTo(0));
            Assert.That(importResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        /// <summary> Здесь бывает ошибка-глюк тестирования ((из-за nUnit+FluentAssertion)): Moq.MockException : Expected invocation on the mock once, but was 2 times: sr => sr.GetAllProjects()</summary>
        [Test]
        public async Task Import_AlreadyImportedSubs_ShouldReturnImportResult_ALREADY_IMPORTED_FluentAssertion()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(3);

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(subs);

            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.ALREADY_IMPORTED);
            importResult.ImportedCount.Should().Be(0);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        /// <summary> Тест импорта набора из 5 подпроектов, с конфликтами в подпроектах с индексами [0], [2], [4]
        /// Здесь бывает ошибка-глюк тестирования ((из-за nUnit)): Moq.MockException : Expected invocation on the mock once, but was 2 times: sr => sr.GetAllProjects()
        /// </summary>
        [Test]
        public async Task Import_SubsWithConflicts1_3_5_ShouldReturnImportResult_SUBPROJECTS_CONFLICTS()
        {
            (List<ProjectSubDivision> existingSubs, List<ProjectSubDivision> subsToImport) =
               TestFixtures.TestFixtures.Simulate10SubProjectsWithConflicts1_3_5_ToImport();

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(existingSubs);
            var expectedMessage = $"{ErrorStrings.SUBPROJECTS_CONFLICTS}:{existingSubs[0].Id},{existingSubs[2].Id},{existingSubs[4].Id}";

            var importResult = await _subProjectsService.Import(subsToImport);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subsToImport), Times.Never);

            Assert.That(importResult != null);
            Assert.That(importResult.Message, Is.EqualTo(expectedMessage));
            Assert.That(importResult.ImportedCount, Is.EqualTo(0));
            Assert.That(importResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Conflict));
        }

        /// <summary> Тест импорта набора из 5 подпроектов, с конфликтами в подпроектах с индексами [0], [2], [4].
        /// Здесь бывает ошибка-глюк тестирования ((из-за nUnit+FluentAssertion)): Moq.MockException : Expected invocation on the mock once, but was 2 times: sr => sr.GetAllProjects().</summary>
        [Test]
        public async Task Import_SubsWithConflicts1_3_5_ShouldReturnImportResult_SUBPROJECTS_CONFLICTS_FluentAssertion()
        {
            (List<ProjectSubDivision> existingSubs, List<ProjectSubDivision> subsToImport) =
               TestFixtures.TestFixtures.Simulate10SubProjectsWithConflicts1_3_5_ToImport();

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(existingSubs);
            var expectedMessage = $"{ErrorStrings.SUBPROJECTS_CONFLICTS}:{existingSubs[0].Id},{existingSubs[2].Id},{existingSubs[4].Id}";

            var importResult = await _subProjectsService.Import(subsToImport);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subsToImport), Times.Never);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(expectedMessage);
            importResult.ImportedCount.Should().Be(0);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }


        [Test]
        public async Task Import_PartiallyImportedSubs_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(10);

            var existingSubs = TestFixtures.TestFixtures.ReturnSomeOfEntities<ProjectSubDivision>(subs);
            var newSubs = subs.Except(existingSubs).ToList();
            var expectedImportedCount = newSubs.Count;

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(existingSubs);
            _subProjectsRepositoryMock.Setup(sr => sr.Import(newSubs))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = newSubs.Count });

            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(newSubs), Times.Once);

            Assert.That(importResult != null);
            Assert.That(importResult.Message, Is.EqualTo(ErrorStrings.IMPORTED));
            Assert.That(importResult.ImportedCount, Is.EqualTo(expectedImportedCount));
            Assert.That(importResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task Import_PartiallyImportedSubs_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(10);

            var existingSubs = TestFixtures.TestFixtures.ReturnSomeOfEntities<ProjectSubDivision>(subs);
            var newSubs = subs.Except(existingSubs).ToList();
            var expectedImportedCount = newSubs.Count;

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(existingSubs);
            _subProjectsRepositoryMock.Setup(sr => sr.Import(newSubs))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = newSubs.Count });

            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(newSubs), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Test]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(3);
            List<ProjectSubDivision> emptySubsList = [];
            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(subs);
            var importResultExpectedMessage = Core.ErrorStrings.PROJECTS_SHOULD_CONTAIN_AT_LEAST_1_PROJECT;
            _subProjectsRepositoryMock.Setup(sr => sr.Import(emptySubsList))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0 });

            ImportResult importResult = null;
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(emptySubsList), Times.Once);

            Assert.That(exception != null);
            Assert.That(importResult, Is.EqualTo(null));
            Assert.That(exception.Message, Is.EqualTo($"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK} ({importResult.StatusCode}). Message: ({importResultExpectedMessage})"));
            Assert.That(importResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException_FluentAssertion()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(3);
            List<ProjectSubDivision> emptySubsList = [];
            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(subs);
            var importResultExpectedMessage = Core.ErrorStrings.PROJECTS_SHOULD_CONTAIN_AT_LEAST_1_PROJECT;
            _subProjectsRepositoryMock.Setup(sr => sr.Import(emptySubsList))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0 });

            ImportResult importResult = null;
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(emptySubsList), Times.Once);
            importResult.Should().BeNull();
            exception.Should().NotBeNull().And.Match<InvalidOperationException>(
                e => string.Equals(e.Message, $"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK} ({importResult.StatusCode}). Message: ({importResultExpectedMessage})"));
        }

        [Test]
        public async Task Import_NewImportedSubs_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            (List<ProjectSubDivision> existingSubs, List<ProjectSubDivision> subsToImport) =
                TestFixtures.TestFixtures.Simulate5And3SubProjectsWithoutConflicts();

            var expectedImportedCount = subsToImport.Count;

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(existingSubs);
            _subProjectsRepositoryMock.Setup(sr => sr.Import(subsToImport))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _subProjectsService.Import(subsToImport);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subsToImport), Times.Once);

            Assert.That(importResult != null);
            Assert.That(importResult.Message, Is.EqualTo(ErrorStrings.IMPORTED));
            Assert.That(importResult.ImportedCount, Is.EqualTo(expectedImportedCount));
            Assert.That(importResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task Import_NewImportedSubs_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            (List<ProjectSubDivision> existingSubs, List<ProjectSubDivision> subsToImport) =
                TestFixtures.TestFixtures.Simulate5And3SubProjectsWithoutConflicts();

            var expectedImportedCount = subsToImport.Count;

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(existingSubs);
            _subProjectsRepositoryMock.Setup(sr => sr.Import(subsToImport))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _subProjectsService.Import(subsToImport);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subsToImport), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task Import_NewImportedSubsWhenNoExistingSubs_ShouldReturnImportResult_OK_with_new_importedCount()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(7);

            var expectedImportedCount = subs.Count;
            var existingSubs = new List<ProjectSubDivision>();

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(existingSubs);
            _subProjectsRepositoryMock.Setup(sr => sr.Import(subs))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Once);

            Assert.That(importResult != null);
            Assert.That(importResult.Message, Is.EqualTo(ErrorStrings.IMPORTED));
            Assert.That(importResult.ImportedCount, Is.EqualTo(expectedImportedCount));
            Assert.That(importResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task Import_NewImportedSubsWhenNoExistingSubs_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(7);

            var expectedImportedCount = subs.Count;
            var existingSubs = new List<ProjectSubDivision>();

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(existingSubs);
            _subProjectsRepositoryMock.Setup(sr => sr.Import(subs))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.OK, Message = ErrorStrings.IMPORTED, ImportedCount = expectedImportedCount });

            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Once);

            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.IMPORTED);
            importResult.ImportedCount.Should().Be(expectedImportedCount);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Test]
        [TestCase(null)]
        [TestCase(100500)]
        public async Task DeleteSubDivision_SubNotFound_ShouldReturnDeleteResult_SUBDIVISION_NOT_FOUND(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.PROJECT_NOT_FOUND });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.That(deleteResult != null);
            Assert.That(deleteResult.Message, Is.EqualTo(Core.ErrorStrings.PROJECT_NOT_FOUND));
            Assert.That(deleteResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        [TestCase(null)]
        [TestCase(100500)]
        public async Task DeleteSubDivision_SubNotFound_ShouldReturnDeleteResult_SUBDIVISION_NOT_FOUND_FluentAssertion(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.PROJECT_NOT_FOUND });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.SUBDIVISION_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase(" ", null)]
        [TestCase(null, 100500)]
        [TestCase("", 100500)]
        [TestCase(" ", 100500)]
        public async Task DeleteSubDivision_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING(string projectSecretString, int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.That(deleteResult != null);
            Assert.That(deleteResult.Message, Is.EqualTo(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING));
            Assert.That(deleteResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Forbidden));
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase(" ", null)]
        [TestCase(null, 100500)]
        [TestCase("", 100500)]
        [TestCase(" ", 100500)]
        public async Task DeleteSubDivision_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING_FluentAssertion(string projectSecretString, int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }


        [Test]
        [TestCase(null)]
        [TestCase(100500)]
        public async Task DeleteSubDivision_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.That(deleteResult != null);
            Assert.That(deleteResult.Message, Is.EqualTo(Core.ErrorStrings.INVALID_SECRET_STRING));
            Assert.That(deleteResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Forbidden));
        }

        [Test]
        [TestCase(null)]
        [TestCase(100500)]
        public async Task DeleteSubDivision_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING_FluentAssertion(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.INVALID_SECRET_STRING);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }


        [Test]
        [TestCase(null)]
        [TestCase(100500)]
        public async Task DeleteSubDivision_OK_ShouldReturnDeleteResult_OK(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.That(deleteResult != null);
            Assert.That(deleteResult.Message, Is.EqualTo(Core.ErrorStrings.OK));
            Assert.That(deleteResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        [TestCase(null)]
        [TestCase(100500)]
        public async Task DeleteSubDivision_OK_ShouldReturnDeleteResult_OK_FluentAssertion(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        
        
        [Test]
        [TestCase(0, null)]
        [TestCase(1, null)]
        [TestCase(2, null)]
        [TestCase(3, null)]
        [TestCase(0, 100500)]
        [TestCase(1, 100500)]
        [TestCase(2, 100500)]
        [TestCase(3, 100500)]
        public async Task GetSub_ExistingSubById_ShouldReturnSub(int id, int? projectId)
        {
            ProjectSubDivision sub = null;
            var existingSub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(setId: id, setProjectId: projectId.Value);
            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(existingSub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            Assert.That(sub != null);
            Assert.That(sub.Id, Is.EqualTo(id));
            Assert.That(sub, Is.EqualTo(existingSub));
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, null)]
        [TestCase(2, null)]
        [TestCase(3, null)]
        [TestCase(0, 100500)]
        [TestCase(1, 100500)]
        [TestCase(2, 100500)]
        [TestCase(3, 100500)]
        public async Task GetSub_ExistingSubById_ShouldReturnSub_FluentAssertion(int id, int? projectId)
        {
            ProjectSubDivision sub = null;
            var existingSub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(setId: id, setProjectId: projectId.Value);
            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(existingSub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            sub.Should().NotBeNull();
            sub.Id.Should().Be(id);
            sub.Should().Be(existingSub);
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, null)]
        [TestCase(2, null)]
        [TestCase(3, null)]
        [TestCase(0, 404)]
        [TestCase(1, 404)]
        [TestCase(2, 404)]
        [TestCase(3, 404)]
        public async Task GetSub_NotExistingSubById_ShouldReturnNull(int id, int? projectId)
        {
            ProjectSubDivision sub = null;

            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(sub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            Assert.That(sub == null);
        }

        [Test]
        [TestCase(0, null)]
        [TestCase(1, null)]
        [TestCase(2, null)]
        [TestCase(3, null)]
        [TestCase(0, 404)]
        [TestCase(1, 404)]
        [TestCase(2, 404)]
        [TestCase(3, 404)]
        public async Task GetSub_NotExistingSubById_ShouldReturnNull_FluentAssertion(int id, int? projectId)
        {
            ProjectSubDivision sub = null;

            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(sub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            sub.Should().BeNull();
        }


        [Test]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            Assert.That(updateResult != null);
            Assert.That(updateResult.Message, Is.EqualTo(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY));
            Assert.That(updateResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY);
        }

        [Test]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            Assert.That(updateResult != null);
            Assert.That(updateResult.Message, Is.EqualTo(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY));
            Assert.That(updateResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY);
        }


        [Test]
        public async Task Update_SubIsValid_ShouldReturnUpdateResult_SUBDIVISION_UPDATED()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.That(updateResult != null);
            Assert.That(updateResult.Message, Is.EqualTo(Core.ErrorStrings.SUBDIVISION_UPDATED));
            Assert.That(updateResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task Update_SubIsValid_ShouldReturnUpdateResult_SUBDIVISION_UPDATED_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            updateResult.Message.Should().Be(Core.ErrorStrings.SUBDIVISION_UPDATED);
        }

        [Test]
        public async Task Update_SubIsActual_ShouldReturnUpdateResult_SUBDIVISION_IS_ACTUAL()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.That(updateResult != null);
            Assert.That(updateResult.Message, Is.EqualTo(Core.ErrorStrings.SUBDIVISION_IS_ACTUAL));
            Assert.That(updateResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public async Task Update_SubIsActual_ShouldReturnUpdateResult_SUBDIVISION_IS_ACTUAL_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            updateResult.Message.Should().Be(Core.ErrorStrings.SUBDIVISION_IS_ACTUAL);
        }


        [Test]
        public async Task Update_SubNotFound_ShouldReturnUpdateResult_SUBDIVISION_NOT_FOUND()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_NOT_FOUND, StatusCode = System.Net.HttpStatusCode.NotFound });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.That(updateResult != null);
            Assert.That(updateResult.Message, Is.EqualTo(Core.ErrorStrings.SUBDIVISION_NOT_FOUND));
            Assert.That(updateResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Update_SubNotFound_ShouldReturnUpdateResult_SUBDIVISION_NOT_FOUND_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_NOT_FOUND, StatusCode = System.Net.HttpStatusCode.NotFound });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            updateResult.Message.Should().Be(Core.ErrorStrings.SUBDIVISION_NOT_FOUND);
        }


        [Test]
        public async Task Update_CodeChanging_ShouldReturnUpdateResult_SUBDIVISION_CODE_SHOULD_BE_THE_SAME()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME, StatusCode = System.Net.HttpStatusCode.Conflict });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.That(updateResult != null);
            Assert.That(updateResult.Message, Is.EqualTo(Core.ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME));
            Assert.That(updateResult.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Conflict));
        }

        [Test]
        public async Task Update_CodeChanging_ShouldReturnUpdateResult_SUBDIVISION_CODE_SHOULD_BE_THE_SAME_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME, StatusCode = System.Net.HttpStatusCode.Conflict });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
            updateResult.Message.Should().Be(Core.ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME);
        }
    }
}
