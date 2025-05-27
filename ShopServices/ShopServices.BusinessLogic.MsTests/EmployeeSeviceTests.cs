using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ShopServices.Abstractions;
using ShopServices.Abstractions.Auth;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Repositories;

namespace ShopServices.BusinessLogic.MsTests
{
    [TestClass]
    public sealed class EmployeeSeviceTests
    {
        private readonly Mock<IEmployeesRepository> _employeesRepositoryMock;
        private readonly EmployeesService _employeesService;
        public EmployeeSeviceTests()
        {
            _employeesRepositoryMock = new Mock<IEmployeesRepository>();
            string key = "JWT:KEY.The encryption algorithm 'HS256' requires a key size of at least '128' bits";
            _employeesService = new EmployeesService(_employeesRepositoryMock.Object, new TokenValidationParameters
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
        public async Task Register_EmployeeIsNull_ShouldThrowArgumentNullException()
        {
            Employee employee = null;
            AuthResult authResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _employeesService.Register(employee));

            _employeesRepositoryMock.Verify(ar => ar.AddUser(employee), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(authResult);
            Assert.AreEqual(ResultMessager.EMPLOYEE_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Register_EmployeeIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Employee employee = null;
            AuthResult authResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _employeesService.Register(employee));

            _employeesRepositoryMock.Verify(ar => ar.AddUser(employee), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.EMPLOYEE_PARAM_NAME);
            authResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.EMPLOYEE_PARAM_NAME);
        }

        [TestMethod]
        public async Task Register_EmployeeWithNotZeroUserId_ShouldReturnAuthResult_USER_ID_SHOULD_BE_ZERO_400()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateId: true);

            var authResult = await _employeesService.Register(employee);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.USER_ID_SHOULD_BE_ZERO, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_EmployeeWithNotZeroUserId_ShouldReturnAuthResult_USER_ID_SHOULD_BE_ZERO_400_FluentAssertion()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateId: true);

            var authResult = await _employeesService.Register(employee);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.USER_ID_SHOULD_BE_ZERO);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateLogin: false);

            var authResult = await _employeesService.Register(employee);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateLogin: false);

            var authResult = await _employeesService.Register(employee);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutSurname_ShouldReturnAuthResult_SURNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateSurname: false);

            var authResult = await _employeesService.Register(employee);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutSurname_ShouldReturnAuthResult_SURNAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateSurname: false);

            var authResult = await _employeesService.Register(employee);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutUserName_ShouldReturnAuthResult_USERNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateName: false);

            var authResult = await _employeesService.Register(employee);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutUserName_ShouldReturnAuthResult_USERNAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateName: false);

            var authResult = await _employeesService.Register(employee);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutEmail_ShouldReturnAuthResult_EMAIL_SHOULD_NOT_BE_EMPTY_400()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateEmail: false);

            var authResult = await _employeesService.Register(employee);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutEmail_ShouldReturnAuthResult_EMAIL_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generateEmail: false);

            var authResult = await _employeesService.Register(employee);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generatePasswordHash: false);

            var authResult = await _employeesService.Register(employee);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_EmployeeWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields(generatePasswordHash: false);

            var authResult = await _employeesService.Register(employee);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_EmployeeIsValidAndFull_ShouldReturnOk()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _employeesRepositoryMock.Setup(ar => ar.AddUser(employee))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _employeesService.Register(employee);

            Assert.IsTrue(registerResult.Id > 0);
            Assert.AreEqual(expectedId, registerResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, registerResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.AddUser(employee), Times.Once);
        }

        [TestMethod]
        public async Task Register_EmployeeIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _employeesRepositoryMock.Setup(ar => ar.AddUser(employee))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _employeesService.Register(employee);

            registerResult.Id.Should().BeGreaterThan(0);
            registerResult.Id.Should().Be(expectedId);
            registerResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _employeesRepositoryMock.Verify(ar => ar.AddUser(employee), Times.Once);
        }

        [TestMethod]
        public async Task Register_EmployeeIsValid_ShouldReturnOk()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _employeesRepositoryMock.Setup(ar => ar.AddUser(employee))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _employeesService.Register(employee);

            Assert.IsTrue(registerResult.Id > 0);
            Assert.AreEqual(expectedId, registerResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, registerResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.AddUser(employee), Times.Once);
        }

        [TestMethod]
        public async Task Register_EmployeeIsValid_ShouldReturnOk_FluentAssertion()
        {
            var employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _employeesRepositoryMock.Setup(ar => ar.AddUser(employee))
                            .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _employeesService.Register(employee);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _employeesRepositoryMock.Verify(ar => ar.AddUser(employee), Times.Once);
        }

        [TestMethod]
        public async Task Login_LoginDataIsNull_ShouldThrowArgumentNullException()
        {
            LoginData loginData = null;
            AuthResult authResult = null;
            string login = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _employeesService.Login(loginData));

            _employeesRepositoryMock.Verify(ar => ar.GetUser(login), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(authResult);
            Assert.AreEqual(ResultMessager.LOGINDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Login_LoginDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            LoginData loginData = null;
            AuthResult authResult = null;
            string login = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _employeesService.Login(loginData));

            _employeesRepositoryMock.Verify(ar => ar.GetUser(login), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.LOGINDATA_PARAM_NAME);
            authResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.LOGINDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task Login_LoginDataWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutLogin();

            var authResult = await _employeesService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutLogin();

            var authResult = await _employeesService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutPasswordHash();

            var authResult = await _employeesService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutPasswordHash();

            var authResult = await _employeesService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Login_UserNotFound_ShouldReturnAuthResult_USER_NOT_FOUND_404()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Employee employee = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(employee);

            var authResult = await _employeesService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ResultMessager.USER_NOT_FOUND, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, authResult.StatusCode);
            Assert.IsNull(authResult.Id);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_UserNotFound_ShouldReturnAuthResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Employee employee = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(employee);

            var authResult = await _employeesService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ResultMessager.USER_NOT_FOUND);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            authResult.Id.Should().BeNull();

            _employeesRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_PasswordHashMismatch_ShouldReturnAuthResult_PASSWORD_HASH_MISMATCH_401()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields();
            _employeesRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(employee);

            var authResult = await _employeesService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ResultMessager.PASSWORD_HASH_MISMATCH, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, authResult.StatusCode);
            Assert.IsNull(authResult.Id);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_PasswordHashMismatch_ShouldReturnAuthResult_PASSWORD_HASH_MISMATCH_401_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields();
            _employeesRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(employee);

            var authResult = await _employeesService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ResultMessager.PASSWORD_HASH_MISMATCH);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            authResult.Id.Should().BeNull();

            _employeesRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }


        [TestMethod]
        public async Task Login_OK_ShouldReturnAuthResult_OK()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(generateId: true, passwordHash: loginData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(employee);

            var authResult = await _employeesService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ResultMessager.OK, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, authResult.StatusCode);
            Assert.IsNotNull(authResult.Id);
            Assert.IsTrue(authResult.Id > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(authResult.Token));

            _employeesRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_OK_ShouldReturnAuthResult_OK_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(generateId: true, passwordHash: loginData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(employee);

            var authResult = await _employeesService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ResultMessager.OK);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            authResult.Id.Should().NotBeNull();
            authResult.Id.Should().BeGreaterThan(0);
            authResult.Token.Should().NotBeNullOrWhiteSpace();
            _employeesRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }


        [TestMethod]
        public async Task GrantRole_GrantRoleDataIsNull_ShouldThrowArgumentNullException()
        {
            GrantRoleData grantRoleData = null;
            Result updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _employeesService.GrantRole(grantRoleData));
            var userId = TestFixtures.TestFixtures.GenerateId();

            _employeesRepositoryMock.Verify(ar => ar.GetUser(userId), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(updateResult);
            Assert.AreEqual(ResultMessager.GRANTROLEDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task GrantRole_GrantRoleDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            GrantRoleData grantRoleData = null;
            Result updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _employeesService.GrantRole(grantRoleData));
            var userId = TestFixtures.TestFixtures.GenerateId();

            _employeesRepositoryMock.Verify(ar => ar.GetUser(userId), Times.Never);

            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.GRANTROLEDATA_PARAM_NAME);
            updateResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.GRANTROLEDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generateLogin: false);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generateLogin: false);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generatePasswordHash: false);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generatePasswordHash: false);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutGranterLogin_ShouldReturnResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generateGranterLogin: false);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GrantRoleDataWithoutGranterLogin_ShouldReturnResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture(generateGranterLogin: false);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_UserNotFound_ShouldReturnResult_USER_NOT_FOUND_404()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.USER_NOT_FOUND, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_UserNotFound_ShouldReturnResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.USER_NOT_FOUND);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_LoginMismatch_ShouldReturnResult_LOGIN_MISMATCH_403()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.LOGIN_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_LoginMismatch_ShouldReturnResult_LOGIN_MISMATCH_403_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.LOGIN_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }



        [TestMethod]
        public async Task GrantRole_GranterNotFound_ShouldReturnResult_GRANTER_NOT_FOUND_404()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            Employee granter = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.GRANTER_NOT_FOUND, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GranterNotFound_ShouldReturnResult_GRANTER_NOT_FOUND_404_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            Employee granter = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.GRANTER_NOT_FOUND);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_GranterLoginMismatch_ShouldReturnResult_GRANTERLOGIN_MISMATCH_403_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.GRANTERLOGIN_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GranterLoginMismatch_ShouldReturnResult_GRANTERLOGIN_MISMATCH_403()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.GRANTERLOGIN_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_GranterPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.GranterLogin);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.PASSWORD_HASH_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task GrantRole_GranterPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.GranterLogin);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.PASSWORD_HASH_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Never);
        }


        [TestMethod]
        public async Task GrantRole_Updated_ShouldReturnResult_USER_UPDATED_200()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash, login: grantRoleData.GranterLogin);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            _employeesRepositoryMock.Setup(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.USER_UPDATED, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Once);
        }

        [TestMethod]
        public async Task GrantRole_Updated_ShouldReturnResult_USER_UPDATED_200_FluentAssertion()
        {
            var grantRoleData = TestFixtures.TestFixtures.GetGrantRoleDataFixture();

            Employee employee = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: grantRoleData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.Id))
                .ReturnsAsync(employee);

            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(passwordHash: grantRoleData.PasswordHash, login: grantRoleData.GranterLogin);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(grantRoleData.GranterId))
                .ReturnsAsync(granter);

            _employeesRepositoryMock.Setup(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _employeesService.GrantRole(grantRoleData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.USER_UPDATED);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(grantRoleData.GranterId), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GrantRole(grantRoleData.Id, grantRoleData.NewRole, grantRoleData.GranterId), Times.Once);
        }


        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataIsNull_ShouldThrowArgumentNullException()
        {
            UpdateAccountData updateAccountData = null;
            Result updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _employeesService.UpdateAccount(updateAccountData));

            _employeesRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(updateResult);
            Assert.AreEqual(ResultMessager.UPDATEACCOUNTDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            UpdateAccountData updateAccountData = null;
            Result updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _employeesService.UpdateAccount(updateAccountData));

            _employeesRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.UPDATEACCOUNTDATA_PARAM_NAME);
            updateResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.UPDATEACCOUNTDATA_PARAM_NAME);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateLogin: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateLogin: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutSurname_ShouldReturnResult_SURNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateSurname: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutSurname_ShouldReturnResult_USERNAME_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateSurname: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }



        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutUserName_ShouldReturnResult_SURNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateName: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutUserName_ShouldReturnResult_USERNAME_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateName: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutEmail_ShouldReturnResult_EMAIL_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateEmail: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutEmail_ShouldReturnResult_EMAIL_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateEmail: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generatePasswordHash: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generatePasswordHash: false);

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UserIsActual_ShouldReturnResult_OK()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _employeesRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            _employeesRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Once);
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.USER_IS_ACTUAL, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UserIsActual_ShouldReturnResult_OK_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _employeesRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            _employeesRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Once);
            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.USER_IS_ACTUAL);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [TestMethod]
        public async Task UpdateAccount_UserUpdated_ShouldReturnResult_OK()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _employeesRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            _employeesRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Once);
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.USER_UPDATED, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UserUpdated_ShouldReturnResult_OK_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _employeesRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _employeesService.UpdateAccount(updateAccountData);

            _employeesRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Once);
            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.USER_UPDATED);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataIsNull_ShouldThrowArgumentNullException()
        {
            DeleteAccountData deleteAccountData = null;
            Result deleteResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => deleteResult = await _employeesService.DeleteAccount(deleteAccountData));

            Assert.IsNotNull(exception);
            Assert.IsNull(deleteResult);
            Assert.AreEqual(ResultMessager.DELETEACCOUNTDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            DeleteAccountData deleteAccountData = null;
            Result deleteResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => deleteResult = await _employeesService.DeleteAccount(deleteAccountData));

            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.DELETEACCOUNTDATA_PARAM_NAME);
            deleteResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.DELETEACCOUNTDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateLogin: false);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateLogin: false);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generatePasswordHash: false);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generatePasswordHash: false);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_UserNotFound_ShouldReturnResult_USER_NOT_FOUND_404()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Employee userToDelete = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.USER_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_UserNotFound_ShouldReturnResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Employee userToDelete = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.USER_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }



        [TestMethod]
        public async Task DeleteAccount_LoginMismatch_ShouldReturnResult_LOGIN_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.LOGIN_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_LoginMismatch_ShouldReturnResult_LOGIN_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.LOGIN_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterIdWithoutGranterLogin_ShouldReturnResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterIdWithoutGranterLogin_ShouldReturnResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterNotFound_ShouldReturnResult_GRANTER_NOT_FOUND_404()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Employee granter = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.GRANTER_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterNotFound_ShouldReturnResult_GRANTER_NOT_FOUND_404_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Employee granter = null;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.GRANTER_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterLoginMismatch_ShouldReturnResult_GRANTERLOGIN_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true, passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.GRANTERLOGIN_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterLoginMismatch_ShouldReturnResult_GRANTERLOGIN_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true, passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.GRANTERLOGIN_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.PASSWORD_HASH_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.PASSWORD_HASH_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterLoginWithoutGranterId_ShouldReturnResult_GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            uint ZERO_GRANTER_ID = 0;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(ZERO_GRANTER_ID))
                .ReturnsAsync(granter);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ResultMessager.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(ZERO_GRANTER_ID), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterLoginWithoutGranterId_ShouldReturnResult_GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            uint ZERO_GRANTER_ID = 0;
            _employeesRepositoryMock.Setup(ar => ar.GetUser(ZERO_GRANTER_ID))
                .ReturnsAsync(granter);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ResultMessager.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(ZERO_GRANTER_ID), Times.Never);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_UserPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: deleteAccountData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.PASSWORD_HASH_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_UserPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: deleteAccountData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.PASSWORD_HASH_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByUser_ShouldReturnResult_OK()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            _employeesRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByUser_ShouldReturnResult_OK_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            _employeesRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }


        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByGranter_ShouldReturnResult_OK()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            _employeesRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByGranter_ShouldReturnResult_OK_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Employee userToDelete = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            Employee granter = TestFixtures.TestFixtures.GetEmployeeFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            _employeesRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            _employeesRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _employeesService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _employeesRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }
    }
}
