using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Auth;
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

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ErrorStrings.USER_ID_SHOULD_BE_ZERO);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
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

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
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

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
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

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ErrorStrings.EMAIL_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
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

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
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

            var registerResult = await _authService.Register(authUser);

            registerResult.Id.Should().BeGreaterThan(0);
            registerResult.Id.Should().Be(expectedId);
            registerResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _authRepositoryMock.Verify(pr => pr.AddUser(authUser), Times.Once);
        }

        [TestMethod]
        public async Task Register_AuthUserIsValid_ShouldReturnOk()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();

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

        [TestMethod]
        public async Task Login_LoginDataIsNull_ShouldThrowArgumentNullException()
        {
            Core.Auth.LoginData loginData = null;
            AuthResult authResult = null;
            string login = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _authService.Login(loginData));

            _authRepositoryMock.Verify(ar => ar.GetUser(login), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(authResult);
            Assert.AreEqual(ErrorStrings.LOGINDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Login_LoginDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Core.Auth.LoginData loginData = null;
            AuthResult authResult = null;
            string login = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _authService.Login(loginData));

            _authRepositoryMock.Verify(ar => ar.GetUser(login), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.LOGINDATA_PARAM_NAME);
            authResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.LOGINDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task Login_LoginDataWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutLogin();

            var authResult = await _authService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutLogin();

            var authResult = await _authService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutPasswordHash();

            var authResult = await _authService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutPasswordHash();

            var authResult = await _authService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Login_UserNotFound_ShouldReturnAuthResult_USER_NOT_FOUND_404()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = null;
            _authRepositoryMock.Setup(pr => pr.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ErrorStrings.USER_NOT_FOUND, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, authResult.StatusCode);
            Assert.IsNull(authResult.Id);

            _authRepositoryMock.Verify(pr => pr.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_UserNotFound_ShouldReturnAuthResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = null;
            _authRepositoryMock.Setup(pr => pr.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ErrorStrings.USER_NOT_FOUND);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            authResult.Id.Should().BeNull();

            _authRepositoryMock.Verify(pr => pr.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_PasswordHashMismatch_ShouldReturnAuthResult_PASSWORD_HASH_MISMATCH_401()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();
            _authRepositoryMock.Setup(pr => pr.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ErrorStrings.PASSWORD_HASH_MISMATCH, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, authResult.StatusCode);
            Assert.IsNull(authResult.Id);

            _authRepositoryMock.Verify(pr => pr.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_PasswordHashMismatch_ShouldReturnAuthResult_PASSWORD_HASH_MISMATCH_401_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();
            _authRepositoryMock.Setup(pr => pr.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ErrorStrings.PASSWORD_HASH_MISMATCH);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            authResult.Id.Should().BeNull();

            _authRepositoryMock.Verify(pr => pr.GetUser(loginData.Login), Times.Once);
        }


        [TestMethod]
        public async Task Login_OK_ShouldReturnAuthResult_OK()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(generateId: true, passwordHash: loginData.PasswordHash);
            _authRepositoryMock.Setup(pr => pr.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ErrorStrings.OK, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, authResult.StatusCode);
            Assert.IsNotNull(authResult.Id);
            authResult.Id.Should().BeGreaterThan(0);

            _authRepositoryMock.Verify(pr => pr.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_OK_ShouldReturnAuthResult_OK_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(generateId: true, passwordHash: loginData.PasswordHash);
            _authRepositoryMock.Setup(pr => pr.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ErrorStrings.OK);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            authResult.Id.Should().NotBeNull();
            authResult.Id.Should().BeGreaterThan(0);

            _authRepositoryMock.Verify(pr => pr.GetUser(loginData.Login), Times.Once);
        }


        //TODO: GrantRole tests
        //TODO: UpdateAccount tests
        //TODO: DeleteAccount tests
    }
}
