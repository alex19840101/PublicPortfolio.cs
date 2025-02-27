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
    public sealed class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _authRepositoryMock;
        private readonly AuthService _authService;
        public AuthServiceTests()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();
            _authService = new AuthService(_authRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Register_AuthUserIsNull_ShouldThrowArgumentNullException()
        {
            Core.Auth.AuthUser authUser = null;
            AuthResult authResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _authService.Register(authUser));

            _authRepositoryMock.Verify(ar => ar.AddUser(authUser), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(authResult);
            Assert.AreEqual(ErrorStrings.AUTHUSER_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Register_AuthUserIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Core.Auth.AuthUser authUser = null;
            AuthResult authResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _authService.Register(authUser));

            _authRepositoryMock.Verify(ar => ar.AddUser(authUser), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.AUTHUSER_PARAM_NAME);
            authResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.AUTHUSER_PARAM_NAME);
        }

        [TestMethod]
        public async Task Register_AuthUserWithNotZeroUserId_ShouldReturnAuthResult_USER_ID_SHOULD_BE_ZERO_400()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generateId: true);
            
            var authResult = await _authService.Register(authUser);
            
            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.USER_ID_SHOULD_BE_ZERO, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserWithNotZeroUserId_ShouldReturnAuthResult_USER_ID_SHOULD_BE_ZERO_400_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generateId: true);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.USER_ID_SHOULD_BE_ZERO, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generateLogin: false);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generateLogin: false);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserWithoutUserName_ShouldReturnAuthResult_USERNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generateName: false);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserWithoutUserName_ShouldReturnAuthResult_USERNAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generateName: false);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserWithoutEmail_ShouldReturnAuthResult_EMAIL_SHOULD_NOT_BE_EMPTY_400()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generateEmail: false);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.EMAIL_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserWithoutEmail_ShouldReturnAuthResult_EMAIL_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generateEmail: false);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.EMAIL_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        //TODO: Register tests

        [TestMethod]
        public async Task Register_AuthUserWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generatePasswordHash: false);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields(generatePasswordHash: false);

            var authResult = await _authService.Register(authUser);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_AuthUserIsValidAndFull_ShouldReturnOk()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Setup(pr => pr.AddUser(authUser))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _authService.Register(authUser);

            Assert.IsTrue(registerResult.Id > 0);
            Assert.AreEqual(expectedId, registerResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, registerResult.StatusCode);
            _authRepositoryMock.Verify(pr => pr.AddUser(authUser), Times.Once);
        }

        [TestMethod]
        public async Task Register_AuthUserIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Setup(pr => pr.AddUser(authUser))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _authService.Register(authUser);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _authRepositoryMock.Verify(pr => pr.AddUser(authUser), Times.Once);
        }

        [TestMethod]
        public async Task Register_AuthUserIsValid_ShouldReturnOk()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Setup(pr => pr.AddUser(authUser))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _authService.Register(authUser);

            Assert.IsTrue(createResult.Id > 0);
            Assert.AreEqual(expectedId, createResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResult.StatusCode);
            _authRepositoryMock.Verify(pr => pr.AddUser(authUser), Times.Once);
        }

        [TestMethod]
        public async Task Register_AuthUserIsValid_ShouldReturnOk_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Setup(pr => pr.AddUser(authUser))
                            .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _authService.Register(authUser);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _authRepositoryMock.Verify(pr => pr.AddUser(authUser), Times.Once);
        }

        //TODO: Login tests
        //TODO: GrantRole tests
        //TODO: UpdateAccount tests
        //TODO: DeleteAccount tests
    }
}
