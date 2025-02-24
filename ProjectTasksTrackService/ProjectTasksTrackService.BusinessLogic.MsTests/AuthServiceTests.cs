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
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => authResult = await _authService.Register(authUser));

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
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => authResult = await _authService.Register(authUser));

            _authRepositoryMock.Verify(ar => ar.AddUser(authUser), Times.Never);
            exception.Should().NotBeNull().And.Match<ArgumentNullException>(e => e.ParamName == ErrorStrings.AUTHUSER_PARAM_NAME);
            authResult.Should().BeNull();
            exception.ParamName.Should().Be(ErrorStrings.AUTHUSER_PARAM_NAME);
        }

        //TODO: Register tests
        //TODO: Login tests
        //TODO: GrantRole tests
        //TODO: UpdateAccount tests
        //TODO: DeleteAccount tests
    }
}
