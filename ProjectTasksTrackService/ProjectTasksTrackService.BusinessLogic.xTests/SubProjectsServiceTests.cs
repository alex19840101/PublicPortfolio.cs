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
    public class SubProjectsServiceTests
    {
        private readonly Mock<ISubProjectsRepository> _subProjectsRepositoryMock;
        private readonly SubProjectsService _subProjectsService;

        public SubProjectsServiceTests()
        {
            _subProjectsRepositoryMock = new Mock<ISubProjectsRepository>();
            _subProjectsService = new SubProjectsService(_subProjectsRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_SubDivisionIsNull_ShouldThrowArgumentNullException()
        {
            ProjectSubDivision sub = null;
            CreateResult createResult = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => createResult = await _subProjectsService.Create(sub));

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.NotNull(exception);
            Assert.Null(createResult);
            Assert.Equal(ErrorStrings.SUBPROJECT_PARAM_NAME, exception.ParamName);
        }

        [Fact]
        public async Task Create_SubDivisionIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            ProjectSubDivision sub = null;
            CreateResult createResult = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => createResult = await _subProjectsService.Create(sub));

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.SUBPROJECT_PARAM_NAME);
            createResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.SUBPROJECT_PARAM_NAME);
        }

        [Fact]
        public async Task Create_SubWithoutCode_ShouldReturnCreateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateCode: false);

            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }

        [Fact]
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

        [Fact]
        public async Task Create_SubWithoutName_ShouldReturnCreateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateName: false);

            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }

        [Fact]
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


        [Fact]
        public async Task Create_SubWithNotNullId_ShouldReturnCreateResult_SUBPROJECT_ID_SHOULD_BE_ZERO_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);
            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.NotNull(createResult);
            Assert.Equal(ErrorStrings.SUBPROJECT_ID_SHOULD_BE_ZERO, createResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.Null(createResult.Code);
        }

        [Fact]
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



        [Fact]
        public async Task Create_SubIsValidAndFull_ShouldReturnOk()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.Add(sub, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _subProjectsService.Create(sub);

            Assert.True(createResult.Id > 0);
            Assert.Equal(expectedId, createResult.Id);
            Assert.Equal(System.Net.HttpStatusCode.Created, createResult.StatusCode);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Once);
        }

        [Fact]
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

        [Fact]
        public async Task Create_SubIsValid_ShouldReturnOk()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.Add(sub, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _subProjectsService.Create(sub);

            Assert.True(createResult.Id > 0);
            Assert.Equal(expectedId, createResult.Id);
            Assert.Equal(System.Net.HttpStatusCode.Created, createResult.StatusCode);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Once);
        }

        [Fact]
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

        [Fact]
        public async Task Import_SubsIsNull_ShouldThrowArgumentNullException()
        {
            IEnumerable<ProjectSubDivision> subs = null;
            ImportResult importResult = null;
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            Assert.NotNull(exception);
            Assert.Null(importResult);
            Assert.Equal(ErrorStrings.SUBS_PARAM_NAME, exception.ParamName);
        }

        [Fact]
        public async Task Import_SubsIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            IEnumerable<ProjectSubDivision> subs = null;
            ImportResult importResult = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.SUBS_PARAM_NAME);
            importResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.SUBS_PARAM_NAME);
        }

        [Fact]
        public async Task Import_SubsIsEmpty_ShouldReturnImportResult_SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED()
        {
            IEnumerable<ProjectSubDivision> subs = [];
            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED, importResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_SubsIsEmpty_ShouldReturnImportResult_SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED_FluentAssertion()
        {
            IEnumerable<ProjectSubDivision> subs = [];
            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Import_AlreadyImportedSubs_ShouldReturnImportResult_ALREADY_IMPORTED()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(3);

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(subs);

            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.ALREADY_IMPORTED, importResult.Message);
            Assert.Equal(0, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
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


        /// <summary> Тест импорта набора из 5 подпроектов, с конфликтами в подпроектах с индексами [0], [2], [4] </summary>
        [Fact]
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

            Assert.NotNull(importResult);
            Assert.Equal(expectedMessage, importResult.Message);
            Assert.Equal(0, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.Conflict, importResult.StatusCode);
        }

        /// <summary> Тест импорта набора из 5 подпроектов, с конфликтами в подпроектах с индексами [0], [2], [4] </summary>
        [Fact]
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


        [Fact]
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

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_PartiallyImportedSubss_ShouldReturnImportResult_OK_with_new_importedCount_FluentAssertion()
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


        [Fact]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(3);
            List<ProjectSubDivision> emptySubsList = [];
            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(emptySubsList);
            var importResultExpectedMessage =
                $"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK}: {System.Net.HttpStatusCode.BadRequest}. {Core.ErrorStrings.SUBDIVISIONS_SHOULD_CONTAIN_AT_LEAST_1_SUBDIVISION}";
            _subProjectsRepositoryMock.Setup(sr => sr.Import(subs))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0 });

            ImportResult importResult = null;
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Once);

            Assert.NotNull(exception);
            Assert.Null(importResult);
            Assert.Equal(importResultExpectedMessage, exception.Message);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [Fact]
        public async Task Import_IMPORT_RESULT_STATUS_CODE_IS_NOT_OK_ShouldThrowInvalidOperationException_FluentAssertion()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(3);
            List<ProjectSubDivision> emptySubsList = [];
            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(emptySubsList);
            var importResultExpectedMessage =
                $"{ErrorStrings.IMPORT_RESULT_STATUS_CODE_IS_NOT_OK}: {System.Net.HttpStatusCode.BadRequest}. {Core.ErrorStrings.SUBDIVISIONS_SHOULD_CONTAIN_AT_LEAST_1_SUBDIVISION}";
            _subProjectsRepositoryMock.Setup(sr => sr.Import(subs))
                .ReturnsAsync(new ImportResult { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = importResultExpectedMessage, ImportedCount = 0 });

            ImportResult importResult = null;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Once);

            importResult.Should().BeNull();
            exception.Should().NotBeNull().And.Match<InvalidOperationException>(
                e => string.Equals(e.Message, importResultExpectedMessage));
        }

        [Fact]
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

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
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

        [Fact]
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

            Assert.NotNull(importResult);
            Assert.Equal(ErrorStrings.IMPORTED, importResult.Message);
            Assert.Equal(expectedImportedCount, importResult.ImportedCount);
            Assert.Equal(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [Fact]
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

        [Theory]
        [InlineData(null)]
        [InlineData(100500)]
        public async Task DeleteSubDivision_SubNotFound_ShouldReturnDeleteResult_SUBDIVISION_NOT_FOUND(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.SUBDIVISION_NOT_FOUND });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.SUBDIVISION_NOT_FOUND, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(100500)]
        public async Task DeleteSubDivision_SubNotFound_ShouldReturnDeleteResult_SUBDIVISION_NOT_FOUND_FluentAssertion(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.SUBDIVISION_NOT_FOUND });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.SUBDIVISION_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        [InlineData(null, 100500)]
        [InlineData("", 100500)]
        [InlineData(" ", 100500)]
        public async Task DeleteSubDivision_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING(string projectSecretString, int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        [InlineData(null, 100500)]
        [InlineData("", 100500)]
        [InlineData(" ", 100500)]
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

        [Theory]
        [InlineData(null)]
        [InlineData(100500)]
        public async Task DeleteSubDivision_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.INVALID_SECRET_STRING, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(100500)]
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


        [Theory]
        [InlineData(null)]
        [InlineData(100500)]
        public async Task DeleteSubDivision_OK_ShouldReturnDeleteResult_OK(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.NotNull(deleteResult);
            Assert.Equal(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.Equal(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(100500)]
        public async Task DeleteSubDivision_OK_ShouldReturnDeleteResult_OK_FluentAssertion(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.INVALID_SECRET_STRING);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, null)]
        [InlineData(2, null)]
        [InlineData(3, null)]
        [InlineData(0, 100500)]
        [InlineData(1, 100500)]
        [InlineData(2, 100500)]
        [InlineData(3, 100500)]
        public async Task GetSub_ExistingSubById_ShouldReturnSub(int id, int? projectId)
        {
            ProjectSubDivision sub = null;
            var setProjectId = projectId != null ? projectId.Value : 0;
            var existingSub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(setId: id, setProjectId: setProjectId);
            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(existingSub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            Assert.NotNull(sub);
            Assert.Equal(id, sub.Id);
            Assert.Equal(existingSub, sub);
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, null)]
        [InlineData(2, null)]
        [InlineData(3, null)]
        [InlineData(0, 100500)]
        [InlineData(1, 100500)]
        [InlineData(2, 100500)]
        [InlineData(3, 100500)]
        public async Task GetSub_ExistingSubById_ShouldReturnSub_FluentAssertion(int id, int? projectId)
        {
            ProjectSubDivision sub = null;
            var setProjectId = projectId != null ? projectId.Value : 0;
            var existingSub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(setId: id, setProjectId: setProjectId);
            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(existingSub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            sub.Should().NotBeNull();
            sub.Id.Should().Be(id);
            sub.Should().Be(existingSub);
        }


        [Theory]
        [InlineData(0, null)]
        [InlineData(1, null)]
        [InlineData(2, null)]
        [InlineData(3, null)]
        [InlineData(0, 404)]
        [InlineData(1, 404)]
        [InlineData(2, 404)]
        [InlineData(3, 404)]
        public async Task GetSub_NotExistingSubById_ShouldReturnNull(int id, int? projectId)
        {
            ProjectSubDivision sub = null;

            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(sub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            Assert.Null(sub);
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, null)]
        [InlineData(2, null)]
        [InlineData(3, null)]
        [InlineData(0, 404)]
        [InlineData(1, 404)]
        [InlineData(2, 404)]
        [InlineData(3, 404)]
        public async Task GetSub_NotExistingSubById_ShouldReturnNull_FluentAssertion(int id, int? projectId)
        {
            ProjectSubDivision sub = null;

            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(sub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            sub.Should().BeNull();
        }


        [Fact]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.Equal(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY, updateResult.Message);
        }

        [Fact]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY);
        }


        [Fact]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.Equal(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
        }

        [Fact]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_PROJECT_NAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY);
        }


        [Fact]
        public async Task Update_SubIsValid_ShouldReturnUpdateResult_SUBDIVISION_UPDATED()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.Equal(Core.ErrorStrings.SUBDIVISION_UPDATED, updateResult.Message);
        }

        [Fact]
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


        [Fact]
        public async Task Update_SubIsActual_ShouldReturnUpdateResult_SUBDIVISION_IS_ACTUAL()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.Equal(Core.ErrorStrings.SUBDIVISION_IS_ACTUAL, updateResult.Message);
        }

        [Fact]
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

        [Fact]
        public async Task Update_SubNotFound_ShouldReturnUpdateResult_SUBDIVISION_NOT_FOUND()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_NOT_FOUND, StatusCode = System.Net.HttpStatusCode.NotFound });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);
            Assert.Equal(Core.ErrorStrings.SUBDIVISION_NOT_FOUND, updateResult.Message);
        }

        [Fact]
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


        [Fact]
        public async Task Update_CodeChanging_ShouldReturnUpdateResult_SUBDIVISION_CODE_SHOULD_BE_THE_SAME()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME, StatusCode = System.Net.HttpStatusCode.Conflict });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.NotNull(updateResult);
            Assert.Equal(System.Net.HttpStatusCode.Conflict, updateResult.StatusCode);
            Assert.Equal(Core.ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME, updateResult.Message);
        }

        [Fact]
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
