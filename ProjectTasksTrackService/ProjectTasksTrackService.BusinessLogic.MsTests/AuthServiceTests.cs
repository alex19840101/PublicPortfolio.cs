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
            Assert.IsTrue(authResult.Id > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(authResult.Token));

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
            authResult.Token.Should().NotBeNullOrWhiteSpace();
            _authRepositoryMock.Verify(pr => pr.GetUser(loginData.Login), Times.Once);
        }


        //TODO: GrantRole tests
        /*
        GrantRole_GrantRoleDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        
        if (grantRoleData == null)
                throw new ArgumentNullException(ErrorStrings.GRANTROLEDATA_PARAM_NAME);

        GrantRole_GrantRoleDataWithoutLogin_ShouldReturnUpdateResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
            if (string.IsNullOrWhiteSpace(grantRoleData.Login))
                return new UpdateResult(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

        GrantRole_GrantRoleDataWithoutPasswordHash_ShouldReturnUpdateResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()

            if (string.IsNullOrWhiteSpace(grantRoleData.PasswordHash))
                return new UpdateResult(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);


        GrantRole_GrantRoleDataWithoutGranterLogin_ShouldReturnUpdateResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
            if (string.IsNullOrWhiteSpace(grantRoleData.GranterLogin))
                return new UpdateResult(ErrorStrings.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var user = await _authRepository.GetUser(grantRoleData.Id);

        GrantRole_UserNotFound_ShouldReturnUpdateResult_USER_NOT_FOUND_404_FluentAssertion()
            if (user is null)
                return new UpdateResult(message: Core.ErrorStrings.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

        GrantRole_LoginMismatch_ShouldReturnUpdateResult_LOGIN_MISMATCH_403_FluentAssertion()
            if (!string.Equals(user.Login, grantRoleData.Login))
                return new UpdateResult(message: Core.ErrorStrings.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

            AuthUser granter = await _authRepository.GetUser(grantRoleData.GranterId);

        GrantRole_GranterNotFound_ShouldReturnUpdateResult_GRANTER_ID_NOT_FOUND_404_FluentAssertion()
            if (granter is null)
                return new UpdateResult(message: Core.ErrorStrings.GRANTER_ID_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

        GrantRole_GranterLoginMismatch_ShouldReturnUpdateResult_GRANTERLOGIN_MISMATCH_403_FluentAssertion()
            if (!string.Equals(granter.Login, grantRoleData.GranterLogin))
                return new UpdateResult(message: Core.ErrorStrings.GRANTERLOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
        
        GrantRole_GranterPasswordHashMismatch_ShouldReturnUpdateResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
            if (!string.Equals(granter.PasswordHash, grantRoleData.PasswordHash))
                return new UpdateResult(message: Core.ErrorStrings.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
        GrantRole_OK__ShouldReturnUpdateResult_OK_FluentAssertion()
        ok
         * */


        //TODO: UpdateAccount tests
        /*
        UpdateAccount_UpdateAccountDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        if (updateAccountData == null)
                throw new ArgumentNullException(ErrorStrings.LOGINDATA_PARAM_NAME);


        UpdateAccount_UpdateAccountDataWithoutLogin_ShouldReturnUpdateResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
            if (string.IsNullOrWhiteSpace(updateAccountData.Login))
                return new UpdateResult
                {
                    Message = ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

        UpdateAccount_UpdateAccountDataWithoutUserName_ShouldReturnUpdateResult_USERNAME_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
            if (string.IsNullOrWhiteSpace(updateAccountData.UserName))
                return new UpdateResult
                {
                    Message = ErrorStrings.USERNAME_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
        UpdateAccount_UpdateAccountDataWithoutEmail_ShouldReturnUpdateResult_EMAIL_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
            if (string.IsNullOrWhiteSpace(updateAccountData.Email))
                return new UpdateResult
                {
                    Message = ErrorStrings.EMAIL_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
        UpdateAccount_UpdateAccountDataWithoutPasswordHash_ShouldReturnUpdateResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
            if (string.IsNullOrWhiteSpace(updateAccountData.PasswordHash))
                return new UpdateResult
                {
                    Message = ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
        UpdateAccount_ShouldReturnUpdateResult_OK_FluentAssertion()
        ok
         * */
        //TODO: DeleteAccount tests

        /*
         * DeleteAccount_DeleteAccountDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
          if (deleteAccountData == null)
                throw new ArgumentNullException(ErrorStrings.DELETEACCOUNTDATA_PARAM_NAME);

        DeleteAccount_DeleteAccountDataWithoutLogin_ShouldReturnDeleteResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
            if (string.IsNullOrWhiteSpace(deleteAccountData.Login))
                return new DeleteResult(ErrorStrings.LOGIN_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

        DeleteAccount_DeleteAccountDataWithoutPasswordHash_ShouldReturnDeleteResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
            if (string.IsNullOrWhiteSpace(deleteAccountData.PasswordHash))
                return new DeleteResult(ErrorStrings.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var user = await _authRepository.GetUser(deleteAccountData.Id);

        DeleteAccount_UserNotFound_ShouldReturnDeleteResult_USER_NOT_FOUND_404_FluentAssertion()
            if (user is null)
                return new DeleteResult(message: Core.ErrorStrings.USER_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);

        DeleteAccount_LoginMismatch_ShouldReturnDeleteResult_LOGIN_MISMATCH_403_FluentAssertion()
            if (!string.Equals(user.Login, deleteAccountData.Login))
                return new DeleteResult(message: Core.ErrorStrings.LOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

        DeleteAccount_GranterIdWithoutGranterLogin_ShouldReturnDeleteResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE_400_FluentAssertion()
            if (deleteAccountData.GranterId != null)
            {
                if (string.IsNullOrWhiteSpace(deleteAccountData.GranterLogin))
                    return new DeleteResult(ErrorStrings.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE, System.Net.HttpStatusCode.BadRequest);

                AuthUser granter = await _authRepository.GetUser(deleteAccountData.GranterId.Value);


        DeleteAccount_GranterNotFound_ShouldReturnDeleteResult_GRANTER_ID_NOT_FOUND_404_FluentAssertion()
                if (granter is null)
                    return new DeleteResult(message: Core.ErrorStrings.GRANTER_ID_NOT_FOUND, statusCode: System.Net.HttpStatusCode.NotFound);


        DeleteAccount_GranterLoginMismatch_ShouldReturnDeleteResult_GRANTERLOGIN_MISMATCH_403_FluentAssertion()
                if (!string.Equals(granter.Login, deleteAccountData.GranterLogin))
                    return new DeleteResult(message: Core.ErrorStrings.GRANTERLOGIN_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);

         DeleteAccount_GranterPasswordHashMismatch_ShouldReturnDeleteResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
                if (!string.Equals(granter.PasswordHash, deleteAccountData.PasswordHash))
                    return new DeleteResult(message: Core.ErrorStrings.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
        DeleteAccount_GranterLoginWithoutGranterId_ShouldReturnDeleteResult_GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE_400_FluentAssertion()        
        if (!string.IsNullOrWhiteSpace(deleteAccountData.GranterLogin))
                    return new DeleteResult(ErrorStrings.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE, System.Net.HttpStatusCode.BadRequest);

        DeleteAccount_UserPasswordHashMismatch_ShouldReturnDeleteResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
                if (!string.Equals(user.PasswordHash, deleteAccountData.PasswordHash))
                    return new DeleteResult(message: Core.ErrorStrings.PASSWORD_HASH_MISMATCH, statusCode: System.Net.HttpStatusCode.Forbidden);
            }

        DeleteAccount_ShouldReturnDeleteResult_OK_FluentAssertion()
        ok
         */
    }
}
