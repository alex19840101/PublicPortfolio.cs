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
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.BusinessLogic.MsTests
{
    [TestClass]
    public sealed class BuyerSeviceTests
    {
        private readonly Mock<IBuyersRepository> _buyersRepositoryMock;
        private readonly BuyersService _buyersService;
        public BuyerSeviceTests()
        {
            _buyersRepositoryMock = new Mock<IBuyersRepository>();
            string key = "JWT:KEY.The encryption algorithm 'HS256' requires a key size of at least '128' bits";
            _buyersService = new BuyersService(_buyersRepositoryMock.Object, new TokenValidationParameters
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
        public async Task Register_BuyerIsNull_ShouldThrowArgumentNullException()
        {
            Buyer buyer = null;
            AuthResult authResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _buyersService.Register(buyer));

            _buyersRepositoryMock.Verify(ar => ar.AddUser(buyer), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(authResult);
            Assert.AreEqual(ResultMessager.EMPLOYEE_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task Register_BuyerIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            Buyer buyer = null;
            AuthResult authResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _buyersService.Register(buyer));

            _buyersRepositoryMock.Verify(ar => ar.AddUser(buyer), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.EMPLOYEE_PARAM_NAME);
            authResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.EMPLOYEE_PARAM_NAME);
        }

        [TestMethod]
        public async Task Register_BuyerWithNotZeroUserId_ShouldReturnAuthResult_USER_ID_SHOULD_BE_ZERO_400()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateId: true);

            var authResult = await _buyersService.Register(buyer);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.USER_ID_SHOULD_BE_ZERO, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_BuyerWithNotZeroUserId_ShouldReturnAuthResult_USER_ID_SHOULD_BE_ZERO_400_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateId: true);

            var authResult = await _buyersService.Register(buyer);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.USER_ID_SHOULD_BE_ZERO);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_BuyerWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateLogin: false);

            var authResult = await _buyersService.Register(buyer);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_BuyerWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateLogin: false);

            var authResult = await _buyersService.Register(buyer);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_BuyerWithoutSurname_ShouldReturnAuthResult_SURNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateSurname: false);

            var authResult = await _buyersService.Register(buyer);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_BuyerWithoutSurname_ShouldReturnAuthResult_SURNAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateSurname: false);

            var authResult = await _buyersService.Register(buyer);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_BuyerWithoutAddress_ShouldReturnAuthResult_ADDRESS_SHOULD_NOT_BE_EMPTY_400()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateAddress: false);

            var authResult = await _buyersService.Register(buyer);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_BuyerWithoutAddress_ShouldReturnAuthResult_ADDRESS_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateAddress: false);

            var authResult = await _buyersService.Register(buyer);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_BuyerWithoutUserName_ShouldReturnAuthResult_USERNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateName: false);

            var authResult = await _buyersService.Register(buyer);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_BuyerWithoutUserName_ShouldReturnAuthResult_USERNAME_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateName: false);

            var authResult = await _buyersService.Register(buyer);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_BuyerWithoutEmail_ShouldReturnAuthResult_EMAIL_SHOULD_NOT_BE_EMPTY_400()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateEmail: false);

            var authResult = await _buyersService.Register(buyer);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_BuyerWithoutEmail_ShouldReturnAuthResult_EMAIL_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generateEmail: false);

            var authResult = await _buyersService.Register(buyer);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_BuyerWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generatePasswordHash: false);

            var authResult = await _buyersService.Register(buyer);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Register_BuyerWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields(generatePasswordHash: false);

            var authResult = await _buyersService.Register(buyer);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Register_BuyerIsValidAndFull_ShouldReturnOk()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _buyersRepositoryMock.Setup(ar => ar.AddUser(buyer))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _buyersService.Register(buyer);

            Assert.IsTrue(registerResult.Id > 0);
            Assert.AreEqual(expectedId, registerResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, registerResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.AddUser(buyer), Times.Once);
        }

        [TestMethod]
        public async Task Register_BuyerIsValidAndFull_ShouldReturnOk_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithAllFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _buyersRepositoryMock.Setup(ar => ar.AddUser(buyer))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _buyersService.Register(buyer);

            registerResult.Id.Should().BeGreaterThan(0);
            registerResult.Id.Should().Be(expectedId);
            registerResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _buyersRepositoryMock.Verify(ar => ar.AddUser(buyer), Times.Once);
        }

        [TestMethod]
        public async Task Register_BuyerIsValid_ShouldReturnOk()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _buyersRepositoryMock.Setup(ar => ar.AddUser(buyer))
                .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var registerResult = await _buyersService.Register(buyer);

            Assert.IsTrue(registerResult.Id > 0);
            Assert.AreEqual(expectedId, registerResult.Id);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, registerResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.AddUser(buyer), Times.Once);
        }

        [TestMethod]
        public async Task Register_BuyerIsValid_ShouldReturnOk_FluentAssertion()
        {
            var buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields();

            var expectedId = TestFixtures.TestFixtures.GenerateId();

            _buyersRepositoryMock.Setup(ar => ar.AddUser(buyer))
                            .ReturnsAsync(new AuthResult { Id = expectedId, StatusCode = System.Net.HttpStatusCode.Created });

            var createResult = await _buyersService.Register(buyer);

            createResult.Id.Should().BeGreaterThan(0);
            createResult.Id.Should().Be(expectedId);
            createResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            _buyersRepositoryMock.Verify(ar => ar.AddUser(buyer), Times.Once);
        }

        [TestMethod]
        public async Task Login_LoginDataIsNull_ShouldThrowArgumentNullException()
        {
            LoginData loginData = null;
            AuthResult authResult = null;
            string login = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _buyersService.Login(loginData));

            _buyersRepositoryMock.Verify(ar => ar.GetUser(login), Times.Never);
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
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => authResult = await _buyersService.Login(loginData));

            _buyersRepositoryMock.Verify(ar => ar.GetUser(login), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.LOGINDATA_PARAM_NAME);
            authResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.LOGINDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task Login_LoginDataWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutLogin();

            var authResult = await _buyersService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutLogin_ShouldReturnAuthResult_LOGIN_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutLogin();

            var authResult = await _buyersService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutPasswordHash();

            var authResult = await _buyersService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, authResult.StatusCode);
            Assert.IsNull(authResult.Id);
        }

        [TestMethod]
        public async Task Login_LoginDataWithoutPasswordHash_ShouldReturnAuthResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithoutPasswordHash();

            var authResult = await _buyersService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            authResult.Id.Should().BeNull();
        }

        [TestMethod]
        public async Task Login_UserNotFound_ShouldReturnAuthResult_USER_NOT_FOUND_404()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Buyer buyer = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(buyer);

            var authResult = await _buyersService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ResultMessager.USER_NOT_FOUND, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, authResult.StatusCode);
            Assert.IsNull(authResult.Id);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_UserNotFound_ShouldReturnAuthResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Buyer buyer = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(buyer);

            var authResult = await _buyersService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ResultMessager.USER_NOT_FOUND);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            authResult.Id.Should().BeNull();

            _buyersRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_PasswordHashMismatch_ShouldReturnAuthResult_PASSWORD_HASH_MISMATCH_401()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields();
            _buyersRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(buyer);

            var authResult = await _buyersService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ResultMessager.PASSWORD_HASH_MISMATCH, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, authResult.StatusCode);
            Assert.IsNull(authResult.Id);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_PasswordHashMismatch_ShouldReturnAuthResult_PASSWORD_HASH_MISMATCH_401_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields();
            _buyersRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(buyer);

            var authResult = await _buyersService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ResultMessager.PASSWORD_HASH_MISMATCH);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            authResult.Id.Should().BeNull();

            _buyersRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }


        [TestMethod]
        public async Task Login_OK_ShouldReturnAuthResult_OK()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(generateId: true, passwordHash: loginData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(buyer);

            var authResult = await _buyersService.Login(loginData);

            Assert.IsNotNull(authResult);
            Assert.AreEqual(Core.ResultMessager.OK, authResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, authResult.StatusCode);
            Assert.IsNotNull(authResult.Id);
            Assert.IsTrue(authResult.Id > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(authResult.Token));

            _buyersRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }

        [TestMethod]
        public async Task Login_OK_ShouldReturnAuthResult_OK_FluentAssertion()
        {
            var loginData = TestFixtures.TestFixtures.GetLoginDataWithRequiredFields();
            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(generateId: true, passwordHash: loginData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(loginData.Login))
                .ReturnsAsync(buyer);

            var authResult = await _buyersService.Login(loginData);

            authResult.Should().NotBeNull();
            authResult.Message.Should().Be(Core.ResultMessager.OK);
            authResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            authResult.Id.Should().NotBeNull();
            authResult.Id.Should().BeGreaterThan(0);
            authResult.Token.Should().NotBeNullOrWhiteSpace();
            _buyersRepositoryMock.Verify(ar => ar.GetUser(loginData.Login), Times.Once);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_ChangeDiscountGroupsDataIsNull_ShouldThrowArgumentNullException()
        {
            ChangeDiscountGroupsData changeDiscountGroupsData = null;
            Result updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData));
            var userId = TestFixtures.TestFixtures.GenerateId();

            _buyersRepositoryMock.Verify(ar => ar.GetUser(userId), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(updateResult);
            Assert.AreEqual(ResultMessager.CHANGEDISCOUNTGROUPSDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_ChangeDiscountGroupsDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            ChangeDiscountGroupsData changeDiscountGroupsData = null;
            Result updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData));
            var userId = TestFixtures.TestFixtures.GenerateId();

            _buyersRepositoryMock.Verify(ar => ar.GetUser(userId), Times.Never);

            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.CHANGEDISCOUNTGROUPSDATA_PARAM_NAME);
            updateResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.CHANGEDISCOUNTGROUPSDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_ChangeDiscountGroupsDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture(generateLogin: false);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_ChangeDiscountGroupsDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture(generateLogin: false);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_ChangeDiscountGroupsDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture(generatePasswordHash: false);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_ChangeDiscountGroupsDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture(generatePasswordHash: false);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_ChangeDiscountGroupsDataWithoutGranterLogin_ShouldReturnResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture(generateGranterLogin: false);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_ChangeDiscountGroupsDataWithoutGranterLogin_ShouldReturnResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture(generateGranterLogin: false);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_UserNotFound_ShouldReturnResult_USER_NOT_FOUND_404()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.USER_NOT_FOUND, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_UserNotFound_ShouldReturnResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.USER_NOT_FOUND);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_LoginMismatch_ShouldReturnResult_LOGIN_MISMATCH_403()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(passwordHash: changeDiscountGroupsData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.LOGIN_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_LoginMismatch_ShouldReturnResult_LOGIN_MISMATCH_403_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(passwordHash: changeDiscountGroupsData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.LOGIN_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }



        [TestMethod]
        public async Task ChangeDiscountGroups_GranterNotFound_ShouldReturnResult_GRANTER_NOT_FOUND_404()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            Buyer granter = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.GRANTER_NOT_FOUND, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.GranterId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_GranterNotFound_ShouldReturnResult_GRANTER_NOT_FOUND_404_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            Buyer granter = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.GRANTER_NOT_FOUND);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.GranterId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_GranterLoginMismatch_ShouldReturnResult_GRANTERLOGIN_MISMATCH_403_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(passwordHash: changeDiscountGroupsData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.GRANTERLOGIN_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.GranterId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups
            (changeDiscountGroupsData.BuyerId,
            changeDiscountGroupsData.DiscountGroups,
            changeDiscountGroupsData.GranterId), Times.Never);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_GranterLoginMismatch_ShouldReturnResult_GRANTERLOGIN_MISMATCH_403()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(passwordHash: changeDiscountGroupsData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.GRANTERLOGIN_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.GranterId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_GranterPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.GranterLogin);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.PASSWORD_HASH_MISMATCH, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.GranterId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_GranterPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.GranterLogin);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.GranterId))
                .ReturnsAsync(granter);

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.PASSWORD_HASH_MISMATCH);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.GranterId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Never);
        }


        [TestMethod]
        public async Task ChangeDiscountGroups_Updated_ShouldReturnResult_USER_UPDATED_200()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(passwordHash: changeDiscountGroupsData.PasswordHash, login: changeDiscountGroupsData.GranterLogin);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.GranterId))
                .ReturnsAsync(granter);

            _buyersRepositoryMock.Setup(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.USER_UPDATED, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.GranterId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Once);
        }

        [TestMethod]
        public async Task ChangeDiscountGroups_Updated_ShouldReturnResult_USER_UPDATED_200_FluentAssertion()
        {
            var changeDiscountGroupsData = TestFixtures.TestFixtures.GetChangeDiscountGroupsDataFixture();

            Buyer buyer = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: changeDiscountGroupsData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.BuyerId))
                .ReturnsAsync(buyer);

            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(passwordHash: changeDiscountGroupsData.PasswordHash, login: changeDiscountGroupsData.GranterLogin);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(changeDiscountGroupsData.GranterId))
                .ReturnsAsync(granter);

            _buyersRepositoryMock.Setup(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _buyersService.ChangeDiscountGroups(changeDiscountGroupsData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.USER_UPDATED);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.BuyerId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(changeDiscountGroupsData.GranterId), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.ChangeDiscountGroups(
                changeDiscountGroupsData.BuyerId,
                changeDiscountGroupsData.DiscountGroups,
                changeDiscountGroupsData.GranterId),
                Times.Once);
        }


        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataIsNull_ShouldThrowArgumentNullException()
        {
            UpdateAccountData updateAccountData = null;
            Result updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _buyersService.UpdateAccount(updateAccountData));

            _buyersRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Never);
            Assert.IsNotNull(exception);
            Assert.IsNull(updateResult);
            Assert.AreEqual(ResultMessager.UPDATEACCOUNTDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            UpdateAccountData updateAccountData = null;
            Result updateResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => updateResult = await _buyersService.UpdateAccount(updateAccountData));

            _buyersRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.UPDATEACCOUNTDATA_PARAM_NAME);
            updateResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.UPDATEACCOUNTDATA_PARAM_NAME);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateLogin: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateLogin: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutSurname_ShouldReturnResult_SURNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateSurname: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutSurname_ShouldReturnResult_USERNAME_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateSurname: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.SURNAME_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutAddress_ShouldReturnResult_ADDRESS_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateAddress: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutAddress_ShouldReturnResult_ADDRESS_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateAddress: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutUserName_ShouldReturnResult_SURNAME_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateName: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutUserName_ShouldReturnResult_USERNAME_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateName: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutEmail_ShouldReturnResult_EMAIL_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateEmail: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutEmail_ShouldReturnResult_EMAIL_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generateEmail: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generatePasswordHash: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UpdateAccountDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture(generatePasswordHash: false);

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task UpdateAccount_UserIsActual_ShouldReturnResult_OK()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _buyersRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            _buyersRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Once);
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.USER_IS_ACTUAL, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UserIsActual_ShouldReturnResult_OK_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _buyersRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_IS_ACTUAL, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            _buyersRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Once);
            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.USER_IS_ACTUAL);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [TestMethod]
        public async Task UpdateAccount_UserUpdated_ShouldReturnResult_OK()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _buyersRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            _buyersRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Once);
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(Core.ResultMessager.USER_UPDATED, updateResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, updateResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAccount_UserUpdated_ShouldReturnResult_OK_FluentAssertion()
        {
            var updateAccountData = TestFixtures.TestFixtures.GetUpdateAccountDataFixture();

            _buyersRepositoryMock.Setup(ar => ar.UpdateUser(updateAccountData))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.USER_UPDATED, StatusCode = System.Net.HttpStatusCode.OK });

            var updateResult = await _buyersService.UpdateAccount(updateAccountData);

            _buyersRepositoryMock.Verify(ar => ar.UpdateUser(updateAccountData), Times.Once);
            updateResult.Should().NotBeNull();
            updateResult.Message.Should().Be(Core.ResultMessager.USER_UPDATED);
            updateResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataIsNull_ShouldThrowArgumentNullException()
        {
            DeleteAccountData deleteAccountData = null;
            Result deleteResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => deleteResult = await _buyersService.DeleteAccount(deleteAccountData));

            Assert.IsNotNull(exception);
            Assert.IsNull(deleteResult);
            Assert.AreEqual(ResultMessager.DELETEACCOUNTDATA_PARAM_NAME, exception.ParamName);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataIsNull_ShouldThrowArgumentNullException_FluentAssertion()
        {
            DeleteAccountData deleteAccountData = null;
            Result deleteResult = null;
            var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => deleteResult = await _buyersService.DeleteAccount(deleteAccountData));

            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ResultMessager.DELETEACCOUNTDATA_PARAM_NAME);
            deleteResult.Should().BeNull();
            exception.ParamName.Should().Be(ResultMessager.DELETEACCOUNTDATA_PARAM_NAME);
        }


        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateLogin: false);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutLogin_ShouldReturnResult_LOGIN_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateLogin: false);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ResultMessager.LOGIN_SHOULD_NOT_BE_EMPTY);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generatePasswordHash: false);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_DeleteAccountDataWithoutPasswordHash_ShouldReturnResult_PASSWORD_HASH_SHOULD_NOT_BE_EMPTY_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generatePasswordHash: false);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ResultMessager.PASSWORD_HASH_SHOULD_NOT_BE_EMPTY);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_UserNotFound_ShouldReturnResult_USER_NOT_FOUND_404()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Buyer userToDelete = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.USER_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_UserNotFound_ShouldReturnResult_USER_NOT_FOUND_404_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Buyer userToDelete = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.USER_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }



        [TestMethod]
        public async Task DeleteAccount_LoginMismatch_ShouldReturnResult_LOGIN_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.LOGIN_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_LoginMismatch_ShouldReturnResult_LOGIN_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.LOGIN_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterIdWithoutGranterLogin_ShouldReturnResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterIdWithoutGranterLogin_ShouldReturnResult_GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ResultMessager.GRANTERLOGIN_SHOULD_NOT_BE_EMPTY_DELETE);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterNotFound_ShouldReturnResult_GRANTER_NOT_FOUND_404()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Buyer granter = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.GRANTER_NOT_FOUND, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterNotFound_ShouldReturnResult_GRANTER_NOT_FOUND_404_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Buyer granter = null;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.GRANTER_NOT_FOUND);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterLoginMismatch_ShouldReturnResult_GRANTERLOGIN_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true, passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.GRANTERLOGIN_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterLoginMismatch_ShouldReturnResult_GRANTERLOGIN_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true, passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.GRANTERLOGIN_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.PASSWORD_HASH_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.PASSWORD_HASH_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_GranterLoginWithoutGranterId_ShouldReturnResult_GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE_400()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            uint ZERO_GRANTER_ID = 0;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(ZERO_GRANTER_ID))
                .ReturnsAsync(granter);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(ResultMessager.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(ZERO_GRANTER_ID), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_GranterLoginWithoutGranterId_ShouldReturnResult_GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE_400_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);
            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            uint ZERO_GRANTER_ID = 0;
            _buyersRepositoryMock.Setup(ar => ar.GetUser(ZERO_GRANTER_ID))
                .ReturnsAsync(granter);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(ResultMessager.GRANTERLOGIN_SHOULD_BE_EMPTY_DELETE);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(ZERO_GRANTER_ID), Times.Never);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }


        [TestMethod]
        public async Task DeleteAccount_UserPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: deleteAccountData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.PASSWORD_HASH_MISMATCH, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_UserPasswordHashMismatch_ShouldReturnResult_PASSWORD_HASH_MISMATCH_403_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: deleteAccountData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.PASSWORD_HASH_MISMATCH);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByUser_ShouldReturnResult_OK()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            _buyersRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByUser_ShouldReturnResult_OK_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture();

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(login: deleteAccountData.Login, passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            _buyersRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }


        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByGranter_ShouldReturnResult_OK()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login,
                passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            _buyersRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(Core.ResultMessager.OK, deleteResult.Message);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAccount_SuccessDeleteByGranter_ShouldReturnResult_OK_FluentAssertion()
        {
            var deleteAccountData = TestFixtures.TestFixtures.GetDeleteAccountDataFixture(generateGranterId: true, generateGranterLogin: true);

            Buyer userToDelete = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true,
                login: deleteAccountData.Login);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.Id))
                .ReturnsAsync(userToDelete);

            Buyer granter = TestFixtures.TestFixtures.GetBuyerFixtureWithRequiredFields(
                generateId: true, login: deleteAccountData.GranterLogin, passwordHash: deleteAccountData.PasswordHash);
            _buyersRepositoryMock.Setup(ar => ar.GetUser(deleteAccountData.GranterId.Value))
                .ReturnsAsync(granter);

            _buyersRepositoryMock.Setup(ar => ar.DeleteUser(deleteAccountData.Id))
                .ReturnsAsync(new Result { Message = Core.ResultMessager.OK, StatusCode = System.Net.HttpStatusCode.OK });

            var deleteResult = await _buyersService.DeleteAccount(deleteAccountData);

            deleteResult.Should().NotBeNull();
            deleteResult.Message.Should().Be(Core.ResultMessager.OK);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.Id), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.GetUser(deleteAccountData.GranterId.Value), Times.Once);
            _buyersRepositoryMock.Verify(ar => ar.DeleteUser(deleteAccountData.Id), Times.Once);
        }
    }
}
