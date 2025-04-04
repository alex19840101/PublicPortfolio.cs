using System;
using System.Net;
using System.Threading.Tasks;
using LiteAuthService.API.Contracts.Dto.Requests;
using LiteAuthService.API.Contracts.Dto.Responses;
using LiteAuthService.API.Contracts.Interfaces;
using LiteAuthService.Core.Auth;
using LiteAuthService.Core.Results;
using LiteAuthService.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LiteAuthService.API.Controllers;

/// <summary> Контроллер управления аутентификацией </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class AuthController : ControllerBase, IAuthAPI
{
    private readonly IAuthService _authService;

    /// <summary> </summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary> Регистрация пользователя </summary>
    [HttpPost("api/v1/Register")]
    [ProducesResponseType(typeof(CreateResponseDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var registerResult = await _authService.Register(AuthUser(request));

        if (registerResult.StatusCode == HttpStatusCode.BadRequest)
            return new BadRequestObjectResult(new ProblemDetails { Title = registerResult.Message });

        if (registerResult.StatusCode == HttpStatusCode.Conflict)
            return new ConflictObjectResult(new MessageResponseDto { Message = registerResult.Message });

        if (registerResult.StatusCode != HttpStatusCode.Created)
            return new ObjectResult(new MessageResponseDto { Message = registerResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

        var result = new CreateResponseDto
        {
            Id = registerResult.Id.Value,
            Message = registerResult.Message,
        };
        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };

    }

    /// <summary> Вход пользователя в систему </summary>
    [HttpPost("api/v1/Login")]
    [ProducesResponseType(typeof(AuthResponseDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(AuthResponseDto), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var loginResult = await _authService.Login(LoginData(request));

        if (loginResult.StatusCode == HttpStatusCode.BadRequest)
            return new BadRequestObjectResult(new ProblemDetails { Title = loginResult.Message });

        if (loginResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new MessageResponseDto { Message = loginResult.Message });

        if (loginResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(loginResult);

        var result = new AuthResponseDto
        {
            Id = loginResult.Id.Value,
            Message = loginResult.Message,
            Token = loginResult.Token
        };
        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
    }

    #region Выход пользователя из системы
    /*
    /// <summary> Выход пользователя из системы </summary>
    [HttpPost("api/v1/Logout")]
    [ProducesResponseType(typeof(AuthResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Logout(LogoutRequestDto request)
    {
        var logoutResult = await _authService.Logout(LogoutData(request));

        if (logoutResult.StatusCode == HttpStatusCode.BadRequest)
            return new BadRequestObjectResult(new ProblemDetails { Title = logoutResult.Message });

        if (logoutResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new MessageResponseDto { Message = logoutResult.Message });

        var result = new AuthResponseDto
        {
            Id = logoutResult.Id.Value,
            Message = logoutResult.Message,
            Token = logoutResult.Token
        };
        return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
    }*/
    #endregion Выход пользователя из системы

    /// <summary>
    /// Предоставление (установка/сброс) роли аккаунту
    /// </summary>
    [HttpPatch("api/v1/GrantRole")]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.Unauthorized)]
    [Authorize(Roles = "admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GrantRole(GrantRoleRequestDto request)
    {
        var grantResult = await _authService.GrantRole(GrantRoleData(request));

        if (grantResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(grantResult);

        if (grantResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new MessageResponseDto { Message = grantResult.Message });

        return Ok(grantResult);
    }

    /// <summary>
    /// Обновление аккаунта
    /// </summary>
    [HttpPatch("api/v1/UpdateAccount")]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateAccount(UpdateAccountRequestDto request)
    {
        var updateResult = await _authService.UpdateAccount(UpdateAccountData(request));

        if (updateResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(updateResult);

        if (updateResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new MessageResponseDto { Message = updateResult.Message });

        if (updateResult.StatusCode == HttpStatusCode.Conflict)
            return new ConflictObjectResult(updateResult);

        return Ok(updateResult);
    }

    /// <summary> Удаление аккаунта пользователя им самим или администратором </summary>
    [HttpDelete("api/v1/DeleteAccount")]
    [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAccount(DeleteAccountRequestDto request)
    {
        var deleteResult = await _authService.DeleteAccount(DeleteAccountData(request));

        if (deleteResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(deleteResult);

        if (deleteResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new MessageResponseDto { Message = deleteResult.Message });

        if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
            return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

        return Ok(deleteResult);
    }

    /// <summary> Получение информации о пользователе по Id (для администраторов) </summary>
    [HttpGet("api/v1/Admin/GetUserInfoById")]
    [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = "admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetUserInfoById(int id)
    {
        var authUser = await _authService.GetUserInfo(id);

        if (authUser is null)
            return NotFound(new MessageResponseDto { Message = Core.ErrorStrings.USER_NOT_FOUND });

        return Ok(UserInfoResponseDto(authUser));
    }

    /// <summary> Получение информации о пользователе по логину (для администраторов) </summary>
    [HttpGet("api/v1/Admin/GetUserInfoByLogin")]
    [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = "admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetUserInfoByLogin(string login)
    {
        var authUser = await _authService.GetUserInfo(login);

        if (authUser is null)
            return NotFound(new MessageResponseDto { Message = Core.ErrorStrings.USER_NOT_FOUND });

        return Ok(UserInfoResponseDto(authUser));
    }

    [NonAction]
    private static AuthUser AuthUser(RegisterRequestDto request) =>
        new AuthUser(
            id: 0,
            login: request.Login,
            userName: request.UserName,
            email: request.Email,
            passwordHash: GeneratePasswordHash(request.Password, request.RepeatPassword),
            nick: request.Nick,
            phone: request.Phone,
            role: request.RequestedRole,
            granterId: null,
            createdDt: DateTime.Now,
            lastUpdateDt: null);

    [NonAction]
    private static string GeneratePasswordHash(string password, string repeatPassword)
    {
        if (!string.Equals(password, repeatPassword) ||
            string.IsNullOrWhiteSpace(password))
            return null;

        return SHA256Hasher.GetHash(password);
    }

    [NonAction]
    private static LoginData LoginData(LoginRequestDto request)
    {
        return new LoginData(
            login: request.Login,
            passwordHash: GeneratePasswordHash(request.Password, request.Password),
            timeoutMinutes: request.TimeoutMinutes);
    }


    [NonAction]
    private static DeleteAccountData DeleteAccountData(DeleteAccountRequestDto request)
    {
        return new DeleteAccountData(
            id: request.Id,
            login: request.Login,
            passwordHash: GeneratePasswordHash(request.Password, request.RepeatPassword),
            granterId: request.GranterId,
            granterLogin: request.GranterLogin);
    }

    #region Logout не требуется для пет-проекта
    //[NonAction]
    //private static LogoutData LogoutData(LogoutRequestDto request)
    //{
    //    return new LogoutData(
    //        login: request.Login,
    //        id: request.Id);
    //}
    #endregion Logout не требуется для пет-проекта

    [NonAction]
    private static GrantRoleData GrantRoleData(GrantRoleRequestDto requestDto)
    {
        return new GrantRoleData(
            id: requestDto.Id,
            login: requestDto.Login,
            passwordHash: GeneratePasswordHash(requestDto.Password, repeatPassword: requestDto.Password),
            newRole: requestDto.NewRole,
            granterId: requestDto.GranterId,
            granterLogin: requestDto.GranterLogin);
    }

    [NonAction]
    private static UpdateAccountData UpdateAccountData(UpdateAccountRequestDto requestDto)
    {
        return new UpdateAccountData(
                id: requestDto.Id,
                login: requestDto.Login,
                userName: requestDto.UserName,
                email: requestDto.Email,
                passwordHash: GeneratePasswordHash(requestDto.ExistingPassword, repeatPassword: requestDto.ExistingPassword),
                newPasswordHash: requestDto.NewPassword != null ? GeneratePasswordHash(requestDto.NewPassword, repeatPassword: requestDto.RepeatNewPassword) : null,
                nick: requestDto.Nick,
                phone: requestDto.Phone,
                requestedRole: requestDto.RequestedRole);
    }

    [NonAction]
    private static UserInfoResponseDto UserInfoResponseDto(AuthUser authUser) =>
        new UserInfoResponseDto
        {
            Id = authUser.Id,
            Login = authUser.Login,
            UserName = authUser.UserName,
            Email = authUser.Email,
            Nick = authUser.Nick,
            Phone = authUser.Phone,
            Role = authUser.Role
        };
}
