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
    public sealed class SubProjectsServiceTests
    {
        private readonly Mock<ISubProjectsRepository> _subProjectsRepositoryMock;
        private readonly SubProjectsService _subProjectsService;

        public SubProjectsServiceTests()
        {
            _subProjectsRepositoryMock = new Mock<ISubProjectsRepository>();
            _subProjectsService = new SubProjectsService(_subProjectsRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Create_SubDivisionIsNull_ShouldThrowArgumentNullException()
        {
            ProjectSubDivision sub = null;
            CreateResult createResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => createResult = await _subProjectsService.Create(sub));

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(createResult);
            Assert.AreEqual(ErrorStrings.SUBPROJECT_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Create_SubDivisionIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            ProjectSubDivision sub = null;
            CreateResult createResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => createResult = await _subProjectsService.Create(sub));

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);

            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.SUBPROJECT_PARAM_NAME);
            createResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.SUBPROJECT_PARAM_NAME);
        }

        [TestMethod]
        public async Task Create_SubWithoutCode_ShouldReturnCreateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateCode: false);
            
            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.IsNull(createResult.Code);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Create_SubWithoutName_ShouldReturnCreateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateName: false);
            
            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);
            Assert.IsNotNull(createResult);
            Assert.AreEqual(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY, createResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.IsNull(createResult.Code);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Create_SubWithNotNullId_ShouldReturnCreateResult_SUBPROJECT_ID_SHOULD_BE_ZERO_400()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);
            
            var createResult = await _subProjectsService.Create(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Never);

            Assert.IsNotNull(createResult);
            Assert.AreEqual(ErrorStrings.SUBPROJECT_ID_SHOULD_BE_ZERO, createResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, createResult.StatusCode);
            Assert.IsNull(createResult.Code);
        }

        [TestMethod]
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


        [TestMethod]
        public async Task Create_SubIsValidAndFull_ShouldReturnOk()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.Add(sub, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _subProjectsService.Create(sub);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Once);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Create_SubIsValid_ShouldReturnOk()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.Add(sub, false))
                .ReturnsAsync(new CreateResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _subProjectsService.Create(sub);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _subProjectsRepositoryMock.Verify(sr => sr.Add(sub, false), Times.Once);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Import_SubsIsNull_ShouldThrowArgumentNullException()
        {
            IEnumerable<ProjectSubDivision> subs = null;
            ImportResult importResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(importResult);
            Assert.AreEqual(ErrorStrings.SUBS_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Import_SubsIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            IEnumerable<ProjectSubDivision> subs = null;
            ImportResult importResult = null;
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.SUBS_PARAM_NAME);
            importResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.SUBS_PARAM_NAME);
        }

        [TestMethod]
        public async Task Import_SubsIsEmpty_ShouldReturnImportResult_SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED()
        {
            IEnumerable<ProjectSubDivision> subs = [];
            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED, importResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [TestMethod]
        public async Task Import_SubsIsEmpty_ShouldReturnImportResult_SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED_FluentAssertion()
        {
            IEnumerable<ProjectSubDivision> subs = [];
            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            importResult.Should().NotBeNull();
            importResult.Message.Should().Be(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED);
            importResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Import_AlreadyImportedSubs_ShouldReturnImportResult_ALREADY_IMPORTED()
        {
            var subs = TestFixtures.TestFixtures.GenerateSubProjectsList(3);

            _subProjectsRepositoryMock.Setup(sr => sr.GetAllProjectSubDivisions())
                .ReturnsAsync(subs);

            var importResult = await _subProjectsService.Import(subs);

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Never);
            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.ALREADY_IMPORTED, importResult.Message);
            Assert.AreEqual(0, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
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
        [TestMethod]
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

            Assert.IsNotNull(importResult);
            Assert.AreEqual(expectedMessage, importResult.Message);
            Assert.AreEqual(0, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, importResult.StatusCode);
        }

        /// <summary> Тест импорта набора из 5 подпроектов, с конфликтами в подпроектах с индексами [0], [2], [4] </summary>
        [TestMethod]
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


        [TestMethod]
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
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Once);
            
            Assert.IsNotNull(exception);
            Assert.IsNull(importResult);
            Assert.AreEqual(importResultExpectedMessage, exception.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, importResult.StatusCode);
        }

        [TestMethod]
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

            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => importResult = await _subProjectsService.Import(subs));

            _subProjectsRepositoryMock.Verify(sr => sr.GetAllProjectSubDivisions(), Times.Once);
            _subProjectsRepositoryMock.Verify(sr => sr.Import(subs), Times.Once);
            importResult.Should().BeNull();
            exception.Should().NotBeNull().And.Match<InvalidOperationException>(
                e => string.Equals(e.Message, importResultExpectedMessage));
        }


        [TestMethod]
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

            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.IMPORTED, importResult.Message);
            Assert.AreEqual(expectedImportedCount, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
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


        [TestMethod]
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

            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.IMPORTED, importResult.Message);
            Assert.AreEqual(expectedImportedCount, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
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

        [TestMethod]
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

            Assert.IsNotNull(importResult);
            Assert.AreEqual(ErrorStrings.IMPORTED, importResult.Message);
            Assert.AreEqual(expectedImportedCount, importResult.ImportedCount);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, importResult.StatusCode);
        }

        [TestMethod]
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

        [DataTestMethod]
        [DataRow(null)]
        [DataRow(100500)]
        public async Task DeleteSubDivision_SubNotFound_ShouldReturnDeleteResult_SUBDIVISION_NOT_FOUND(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.NotFound, Message = Core.ErrorStrings.SUBDIVISION_NOT_FOUND });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.SUBDIVISION_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow(100500)]
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

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow(" ", null)]
        [DataRow(null, 100500)]
        [DataRow("", 100500)]
        [DataRow(" ", 100500)]
        public async Task DeleteSubDivision_EmptyOrNullOrSpaceSecretString_ShouldReturnDeleteResult_EMPTY_OR_NULL_SECRET_STRING(string projectSecretString, int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.EMPTY_OR_NULL_SECRET_STRING, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow(" ", null)]
        [DataRow(null, 100500)]
        [DataRow("", 100500)]
        [DataRow(" ", 100500)]
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

        [DataTestMethod]
        [DataRow(null)]
        [DataRow(100500)]
        public async Task DeleteSubDivision_InvalidSecretString_ShouldReturnDeleteResult_INVALID_SECRET_STRING(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.Forbidden, Message = Core.ErrorStrings.INVALID_SECRET_STRING });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.INVALID_SECRET_STRING, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow(100500)]
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

        [DataTestMethod]
        [DataRow(null)]
        [DataRow(100500)]
        public async Task DeleteSubDivision_OK_ShouldReturnDeleteResult_OK(int? projectId)
        {
            var id = TestFixtures.TestFixtures.GenerateId();
            var projectSecretString = TestFixtures.TestFixtures.GenerateString();

            _subProjectsRepositoryMock.Setup(sr => sr.DeleteSubDivision(id, projectSecretString, projectId))
                .ReturnsAsync(new DeleteResult { StatusCode = System.Net.HttpStatusCode.OK, Message = Core.ErrorStrings.OK });

            var deleteResult = await _subProjectsService.DeleteSubDivision(id, projectSecretString, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.DeleteSubDivision(id, projectSecretString, projectId), Times.Once);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow(100500)]
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

        [DataTestMethod]
        [DataRow(0, null)]
        [DataRow(1, null)]
        [DataRow(2, null)]
        [DataRow(3, null)]
        [DataRow(0, 100500)]
        [DataRow(1, 100500)]
        [DataRow(2, 100500)]
        [DataRow(3, 100500)]
        public async Task GetSub_ExistingSubById_ShouldReturnSub(int id, int? projectId)
        {
            ProjectSubDivision sub = null;
            var setProjectId = projectId != null ? projectId.Value : 0;
            var existingSub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(setId: id, setProjectId: setProjectId);
            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(existingSub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            Assert.IsNotNull(sub);
            Assert.AreEqual(id, sub.Id);
            Assert.AreEqual(existingSub, sub);
        }

        [DataTestMethod]
        [DataRow(0, null)]
        [DataRow(1, null)]
        [DataRow(2, null)]
        [DataRow(3, null)]
        [DataRow(0, 100500)]
        [DataRow(1, 100500)]
        [DataRow(2, 100500)]
        [DataRow(3, 100500)]
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


        [DataTestMethod]
        [DataRow(0, null)]
        [DataRow(1, null)]
        [DataRow(2, null)]
        [DataRow(3, null)]
        [DataRow(0, 404)]
        [DataRow(1, 404)]
        [DataRow(2, 404)]
        [DataRow(3, 404)]
        public async Task GetSub_NotExistingSubById_ShouldReturnNull(int id, int? projectId)
        {
            ProjectSubDivision sub = null;

            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(sub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            Assert.IsNull(sub);
        }

        [DataTestMethod]
        [DataRow(0, null)]
        [DataRow(1, null)]
        [DataRow(2, null)]
        [DataRow(3, null)]
        [DataRow(0, 404)]
        [DataRow(1, 404)]
        [DataRow(2, 404)]
        [DataRow(3, 404)]
        public async Task GetSub_NotExistingSubById_ShouldReturnNull_FluentAssertion(int id, int? projectId)
        {
            ProjectSubDivision sub = null;

            _subProjectsRepositoryMock.Setup(sr => sr.GetProjectSubDivision(id, projectId))
                .ReturnsAsync(sub);

            sub = await _subProjectsService.GetSubDivision(id, projectId);

            _subProjectsRepositoryMock.Verify(sr => sr.GetProjectSubDivision(id, projectId), Times.Once);

            sub.Should().BeNull();
        }


        [TestMethod]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.AreEqual(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_CodeIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateCode: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY);
        }


        [TestMethod]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.AreEqual(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_NameIsNullOrWhiteSpace_ShouldReturnUpdateResult_SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true, generateName: false);

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Never);

            updateResult.Should().NotBeNull();
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            updateResult.Message.Should().Be(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY);
        }


        [TestMethod]
        public async Task Update_SubIsValid_ShouldReturnUpdateResult_SUBDIVISION_UPDATED()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.AreEqual(Core.ErrorStrings.SUBDIVISION_UPDATED, updateResult.Message);
        }

        [TestMethod]
        public async Task Update_SubIsValid_ShouldReturnUpdateResult_Update_SubIsValid_ShouldReturnUpdateResult_SUBDIVISION_UPDATED_FluentAssertion()
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


        [TestMethod]
        public async Task Update_SubIsActual_ShouldReturnUpdateResult_SUBDIVISION_IS_ACTUAL()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
            Assert.AreEqual(Core.ErrorStrings.SUBDIVISION_IS_ACTUAL, updateResult.Message);
        }

        [TestMethod]
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


        [TestMethod]
        public async Task Update_SubNotFound_ShouldReturnUpdateResult_SUBDIVISION_NOT_FOUND()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_NOT_FOUND, StatusCode = System.Net.HttpStatusCode.NotFound });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);
            Assert.AreEqual(Core.ErrorStrings.SUBDIVISION_NOT_FOUND, updateResult.Message);
        }

        [TestMethod]
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


        [TestMethod]
        public async Task Update_CodeChanging_ShouldReturnUpdateResult_SUBDIVISION_CODE_SHOULD_BE_THE_SAME()
        {
            var sub = TestFixtures.TestFixtures.GetSubProjectFixtureWithAllFields(generateId: true);

            _subProjectsRepositoryMock.Setup(sr => sr.UpdateSubDivision(sub))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME, StatusCode = System.Net.HttpStatusCode.Conflict });

            var updateResult = await _subProjectsService.UpdateSubDivision(sub);

            _subProjectsRepositoryMock.Verify(sr => sr.UpdateSubDivision(sub), Times.Once);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, updateResult.StatusCode);
            Assert.AreEqual(Core.ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME, updateResult.Message);
        }

        [TestMethod]
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
