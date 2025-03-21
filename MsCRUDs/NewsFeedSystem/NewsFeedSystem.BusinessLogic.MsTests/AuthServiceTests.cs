using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Auth;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NewsFeedSystem.BusinessLogic.MsTests
{
    [TestClass]
    public sealed class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _authRepositoryMock;
        private readonly AuthService _authService;
        public AuthServiceTests()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();
            string key = "JWT:KEY.The encryption algorithm 'HS256' requires a key size of at least '128' bits";
            _authService = new AuthService(_authRepositoryMock.Object, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "JWT:Issuer",
                ValidateAudience = true,
                ValidAudience = "JWT:Audience",
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(key)),
                ValidateIssuerSigningKey = true
            }, key);
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

            _authRepositoryMock.Setup(ar => ar.AddUser(authUser))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _authService.Register(authUser);

            Assert.IsTrue(registerResult.Id > 0);
            Assert.AreEqual(expectedId, registerResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, registerResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.AddUser(authUser), Times.Once);
        }

        [TestMethod]
        public async Task Register_AuthUserIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Setup(ar => ar.AddUser(authUser))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _authService.Register(authUser);

            registerResult.Id.Should().BeGreaterThan(0);
            registerResult.Id.Should().Be(expectedId);
            registerResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _authRepositoryMock.Verify(ar => ar.AddUser(authUser), Times.Once);
        }

        [TestMethod]
        public async Task Register_AuthUserIsValid_ShouldReturnOk()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Setup(ar => ar.AddUser(authUser))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _authService.Register(authUser);

            Assert.IsTrue(registerResult.Id > 0);
            Assert.AreEqual(expectedId, registerResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, registerResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.AddUser(authUser), Times.Once);
        }

        [TestMethod]
        public async Task Register_AuthUserIsValid_ShouldReturnOk_FluentAssertion()
        {
            var authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Setup(ar => ar.AddUser(authUser))
                            .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _authService.Register(authUser);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _authRepositoryMock.Verify(ar => ar.AddUser(authUser), Times.Once);
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
            _authRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ErrorStrings.USER_NOT_FOUND, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, authResult.StatusCode);
            Assert.IsNull(authResult.Id);

            _authRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_UserNotFound_ShouldReturnAuthResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ErrorStrings.USER_NOT_FOUND);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            authResult.Id.Should().BeNull();

            _authRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_PasswordHashMismatch_ShouldReturnAuthResult_PASSWORD_HASH_MISMATCH_401()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();
            _authRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ErrorStrings.PASSWORD_HASH_MISMATCH, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, authResult.StatusCode);
            Assert.IsNull(authResult.Id);

            _authRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_PasswordHashMismatch_ShouldReturnAuthResult_PASSWORD_HASH_MISMATCH_401_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields();
            _authRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ErrorStrings.PASSWORD_HASH_MISMATCH);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            authResult.Id.Should().BeNull();

            _authRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }


        [TestMethod]
        public async Task Login_OK_ShouldReturnAuthResult_OK()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(generateId: true, passwordHash: loginData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ErrorStrings.OK, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, authResult.StatusCode);
            Assert.IsNotNull(authResult.Id);
            Assert.IsTrue(authResult.Id > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(authResult.Token));

            _authRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_OK_ShouldReturnAuthResult_OK_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(generateId: true, passwordHash: loginData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(authUser);

            var authResult = await _authService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ErrorStrings.OK);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            authResult.Id.Should().NotBeNull();
            authResult.Id.Should().BeGreaterThan(0);
            authResult.Token.Should().NotBeNullOrWhiteSpace();
            _authRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }


        [TestMethod]
        public async Task GrantRole_GrantRoleDataIsNull_ShouldThrowArgumentNullException()
        {
            Core.Auth.GrantRoleData grantRoleData = null;
            UpdateResult updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _authService.GrantRole(grantRoleData));
            var userId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Verify(ar => ar.GetUser(userId), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(updateResult);
            Assert.AreEqual(ErrorStrings.GRANTROLEDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task GrantRole_GrantRoleDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Core.Auth.GrantRoleData grantRoleData = null;
            UpdateResult updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _authService.GrantRole(grantRoleData));
            var userId = TestFixtures.TestFixtures.GenerateId();

            _authRepositoryMock.Verify(ar => ar.GetUser(userId), Times.Never);

            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.GRANTROLEDATA_PARAM_NAME);
            updateResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.GRANTROLEDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutLogin_ShouldReturnUpdateResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generateLogin: false);

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutLogin_ShouldReturnUpdateResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generateLogin: false);

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutPasswordHash_ShouldReturnUpdateResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generatePasswordHash: false);

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutPasswordHash_ShouldReturnUpdateResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generatePasswordHash: false);

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutGranterLogin_ShouldReturnUpdateResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generateGranterLogin: false);

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ErrorStrings.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutGranterLogin_ShouldReturnUpdateResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generateGranterLogin: false);

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ErrorStrings.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_UserNotFound_ShouldReturnUpdateResult_USER_NOT_FOUND_404()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ErrorStrings.USER_NOT_FOUND, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_UserNotFound_ShouldReturnUpdateResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ErrorStrings.USER_NOT_FOUND);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_LoginMismatch_ShouldReturnUpdateResult_LOGIN_MISMATCH_403()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ErrorStrings.LOGIN_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_LoginMismatch_ShouldReturnUpdateResult_LOGIN_MISMATCH_403_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ErrorStrings.LOGIN_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }



        [TestMethod]
        public async Task GrantRole_GranterNotFound_ShouldReturnUpdateResult_GRANTER_NOT_FOUND_404()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            AuthUser granter = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ErrorStrings.GRANTER_NOT_FOUND, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GranterNotFound_ShouldReturnUpdateResult_GRANTER_NOT_FOUND_404_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            AuthUser granter = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ErrorStrings.GRANTER_NOT_FOUND);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_GranterLoginMismatch_ShouldReturnUpdateResult_GRANTERLOGIN_MISMATCH_403_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ErrorStrings.GRANTERLOGIN_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GranterLoginMismatch_ShouldReturnUpdateResult_GRANTERLOGIN_MISMATCH_403()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ErrorStrings.GRANTERLOGIN_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_GranterPasswordHashMismatch_ShouldReturnUpdateResult_PASSWORD_HASH_MISMATCH_403()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.GranterLogin);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ErrorStrings.PASSWORD_HASH_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GranterPasswordHashMismatch_ShouldReturnUpdateResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.GranterLogin);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ErrorStrings.PASSWORD_HASH_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_Updated_ShouldReturnUpdateResult_USER_UPDATED_200()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash, login: grantRoleData.GranterLogin);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            _authRepositoryMock.Setup(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _authService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ErrorStrings.USER_UPDATED, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Once);
        }

        [TestMethod]
        public async Task GrantRole_Updated_ShouldReturnUpdateResult_USER_UPDATED_200_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            AuthUser authUser = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: grantRoleData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(authUser);

            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash, login: grantRoleData.GranterLogin);
            _authRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            _authRepositoryMock.Setup(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _authService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ErrorStrings.USER_UPDATED);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Once);
        }


        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataIsNull_ShouldThrowArgumentNullException()
        {
            Core.Auth.UpdateAccountData updateAccountData = null;
            UpdateResult updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _authService.UpdateAccount(updateAccountData));

            _authRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(updateResult);
            Assert.AreEqual(ErrorStrings.UPDATEACCOUNTDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Core.Auth.UpdateAccountData updateAccountData = null;
            UpdateResult updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _authService.UpdateAccount(updateAccountData));

            _authRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.UPDATEACCOUNTDATA_PARAM_NAME);
            updateResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.UPDATEACCOUNTDATA_PARAM_NAME);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutLogin_ShouldReturnUpdateResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateLogin: false);

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutLogin_ShouldReturnUpdateResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateLogin: false);

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutUserName_ShouldReturnUpdateResult_USERNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateName: false);

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutUserName_ShouldReturnUpdateResult_USERNAME_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateName: false);

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutEmail_ShouldReturnUpdateResult_EMAIL_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateEmail: false);

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ErrorStrings.EMAIL_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutEmail_ShouldReturnUpdateResult_EMAIL_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateEmail: false);

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ErrorStrings.EMAIL_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutPasswordHash_ShouldReturnUpdateResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generatePasswordHash: false);

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutPasswordHash_ShouldReturnUpdateResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generatePasswordHash: false);

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UserIsActual_ShouldReturnUpdateResult_OK()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _authRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.USER_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ErrorStrings.USER_IS_ACTUAL, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UserIsActual_ShouldReturnUpdateResult_OK_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _authRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.USER_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ErrorStrings.USER_IS_ACTUAL);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [TestMethod]
        public async Task UpdateAccount_UserUpdated_ShouldReturnUpdateResult_OK()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _authRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ErrorStrings.USER_UPDATED, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UserUpdated_ShouldReturnUpdateResult_OK_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _authRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new UpdateResult { Message = Core.ErrorStrings.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _authService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ErrorStrings.USER_UPDATED);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataIsNull_ShouldThrowArgumentNullException()
        {
            Core.Auth.DeleteAccountData deleteAccountData = null;
            DeleteResult deleteResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => deleteResult = await _authService.DeleteAccount(deleteAccountData));

            Assert.IsNotNull(exception);
            Assert.IsNull(deleteResult);
            Assert.AreEqual(ErrorStrings.DELETEACCOUNTDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Core.Auth.DeleteAccountData deleteAccountData = null;
            DeleteResult deleteResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => deleteResult = await _authService.DeleteAccount(deleteAccountData));

            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.DELETEACCOUNTDATA_PARAM_NAME);
            deleteResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.DELETEACCOUNTDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutLogin_ShouldReturnDeleteResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateLogin: false);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutLogin_ShouldReturnDeleteResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateLogin: false);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutPasswordHash_ShouldReturnDeleteResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generatePasswordHash: false);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutPasswordHash_ShouldReturnDeleteResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generatePasswordHash: false);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_UserNotFound_ShouldReturnDeleteResult_USER_NOT_FOUND_404()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            AuthUser userToDelete = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.USER_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_UserNotFound_ShouldReturnDeleteResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            AuthUser userToDelete = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.USER_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }



        [TestMethod]
        public async Task DeleteAccount_LoginMismatch_ShouldReturnDeleteResult_LOGIN_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.LOGIN_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_LoginMismatch_ShouldReturnDeleteResult_LOGIN_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.LOGIN_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterIdWithoutGranterLogin_ShouldReturnDeleteResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ErrorStrings.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterIdWithoutGranterLogin_ShouldReturnDeleteResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ErrorStrings.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterNotFound_ShouldReturnDeleteResult_GRANTER_NOT_FOUND_404()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            AuthUser granter = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.GRANTER_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterNotFound_ShouldReturnDeleteResult_GRANTER_NOT_FOUND_404_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            AuthUser granter = null;
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.GRANTER_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterLoginMismatch_ShouldReturnDeleteResult_GRANTERLOGIN_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true, passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.GRANTERLOGIN_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterLoginMismatch_ShouldReturnDeleteResult_GRANTERLOGIN_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true, passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.GRANTERLOGIN_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterPasswordHashMismatch_ShouldReturnDeleteResult_PASSWORD_HASH_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.PASSWORD_HASH_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterPasswordHashMismatch_ShouldReturnDeleteResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.PASSWORD_HASH_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterLoginWithoutGranterId_ShouldReturnDeleteResult_GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            int ZERO_GRANTER_ID = 0;
            _authRepositoryMock.Setup(ar => ar.GetUser(ZERO_GRANTER_ID))
                .ReturnsAsync(granter);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ErrorStrings.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.GetUser(ZERO_GRANTER_ID), Times.Never);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterLoginWithoutGranterId_ShouldReturnDeleteResult_GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            int ZERO_GRANTER_ID = 0;
            _authRepositoryMock.Setup(ar => ar.GetUser(ZERO_GRANTER_ID))
                .ReturnsAsync(granter);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ErrorStrings.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _authRepositoryMock.Verify(ar => ar.GetUser(ZERO_GRANTER_ID), Times.Never);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_UserPasswordHashMismatch_ShouldReturnDeleteResult_PASSWORD_HASH_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: deleteAccountData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.PASSWORD_HASH_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_UserPasswordHashMismatch_ShouldReturnDeleteResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: deleteAccountData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.PASSWORD_HASH_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByUser_ShouldReturnDeleteResult_OK()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            _authRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new DeleteResult { Message = Core.ErrorStrings.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByUser_ShouldReturnDeleteResult_OK_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            _authRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new DeleteResult { Message = Core.ErrorStrings.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }


        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByGranter_ShouldReturnDeleteResult_OK()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            _authRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new DeleteResult { Message = Core.ErrorStrings.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ErrorStrings.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByGranter_ShouldReturnDeleteResult_OK_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            AuthUser userToDelete = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            AuthUser granter = TestFixtures.TestFixtures.GetAuthUserFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            _authRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            _authRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new DeleteResult { Message = Core.ErrorStrings.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _authService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ErrorStrings.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _authRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _authRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }
    }
}
