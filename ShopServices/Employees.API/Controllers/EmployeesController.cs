using System;
using System.Net;
using System.Threading.Tasks;
using Employees.API.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopServices.Core.Auth;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Services;
using ShopServices.Abstractions.Auth;
using Employees.API.Contracts.Requests;

namespace Employees.API.Controllers;

/// <summary> Контроллер управления аутентификацией для входа в систему работников </summary>
[ApiController]
[Asp.Versioning.ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[Produces("application/json")]
[Consumes("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesService _employeesService;

    /// <summary> Конструктор контроллера управления аутентификацией работников </summary>
    public EmployeesController(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    /// <summary> Регистрация работника </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var registerResult = await _employeesService.Register(GetCoreEmployee(request));

        if (registerResult.StatusCode == HttpStatusCode.BadRequest)
            return new BadRequestObjectResult(new ProblemDetails { Title = registerResult.Message });

        if (registerResult.StatusCode == HttpStatusCode.Conflict)
            return new ConflictObjectResult(new Result
            {
                Message = registerResult.Message,
                StatusCode = registerResult.StatusCode
            });

        if (registerResult.StatusCode != HttpStatusCode.Created)
            return new ObjectResult(new Result { Message = registerResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

        var result = new Result
        {
            Id = registerResult!.Id!.Value,
            Message = registerResult.Message,
        };
        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };

    }

    /// <summary> Вход работника в систему </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AuthResponseDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(AuthResponseDto), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var loginResult = await _employeesService.Login(GetCoreLoginData(request));

        if (loginResult.StatusCode == HttpStatusCode.BadRequest)
            return new BadRequestObjectResult(new ProblemDetails { Title = loginResult.Message });

        if (loginResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new Result { Message = loginResult.Message });

        if (loginResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(loginResult);

        var result = new AuthResponseDto
        {
            Id = loginResult!.Id!.Value,
            Message = loginResult.Message,
            Token = loginResult.Token
        };
        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
    }

    /// <summary>
    /// Предоставление (установка/сброс) роли аккаунту работника
    /// </summary>
    [HttpPatch]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.Unauthorized)]
    [Authorize(Roles = "admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GrantRole(GrantRoleRequestDto request)
    {
        var grantResult = await _employeesService.GrantRole(GetCoreGrantRoleData(request));

        if (grantResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(grantResult);

        if (grantResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new Result { Message = grantResult.Message });

        return Ok(grantResult);
    }

    /// <summary>
    /// Обновление аккаунта работника
    /// </summary>
    [HttpPatch]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> UpdateAccount(UpdateAccountRequestDto request)
    {
        var updateResult = await _employeesService.UpdateAccount(GetCoreUpdateAccountData(request));

        if (updateResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(updateResult);

        if (updateResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new Result { Message = updateResult.Message });

        if (updateResult.StatusCode == HttpStatusCode.Conflict)
            return new ConflictObjectResult(updateResult);

        return Ok(updateResult);
    }

    /// <summary> Удаление (блокировка) аккаунта работника им самим или администратором </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAccount(DeleteAccountRequestDto request)
    {
        var deleteResult = await _employeesService.DeleteAccount(GetCoreDeleteAccountData(request));

        if (deleteResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(deleteResult);

        if (deleteResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new Result { Message = deleteResult.Message });

        if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
            return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

        return Ok(deleteResult);
    }

    /// <summary> Получение информации о работнике по Id (для администраторов) </summary>
    [HttpGet]
    [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = "admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetUserInfoById(uint id)
    {
        var employee = await _employeesService.GetUserInfo(id);

        if (employee is null)
            return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

        return Ok(GetUserInfoResponseDto(employee));
    }

    /// <summary> Получение информации о работнике по логину (для администраторов) </summary>
    [HttpGet]
    [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = "admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetUserInfoByLogin(string login)
    {
        var employee = await _employeesService.GetUserInfo(login);

        if (employee is null)
            return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

        return Ok(GetUserInfoResponseDto(employee));
    }

    [NonAction]
    private static Employee GetCoreEmployee(RegisterRequestDto request) =>
        new Employee(
            id: 0,
            login: request.Login,
            name: request.Name,
            surname: request.Surname,
            address: request.Address,
            email: request.Email,
            passwordHash: SHA256Hasher.GeneratePasswordHash(request.Password, request.RepeatPassword),
            nick: request.Nick,
            phone: request.Phone,
            role: request.RequestedRole,
            granterId: null,
            createdDt: DateTime.Now,
            lastUpdateDt: null,
            shopId: request.ShopId,
            warehouseId: request.WarehouseId);

    [NonAction]
    private static LoginData GetCoreLoginData(LoginRequestDto request)
    {
        return new LoginData(
            login: request.Login,
            passwordHash: SHA256Hasher.GeneratePasswordHash(request.Password, request.Password),
            timeoutMinutes: request.TimeoutMinutes);
    }


    [NonAction]
    private static DeleteAccountData GetCoreDeleteAccountData(DeleteAccountRequestDto request)
    {
        return new DeleteAccountData(
            id: request.Id,
            login: request.Login,
            passwordHash: SHA256Hasher.GeneratePasswordHash(request.Password, request.RepeatPassword),
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
    private static GrantRoleData GetCoreGrantRoleData(GrantRoleRequestDto requestDto)
    {
        return new GrantRoleData(
            id: requestDto.Id,
            login: requestDto.Login,
            passwordHash: SHA256Hasher.GeneratePasswordHash(requestDto.Password, repeatPassword: requestDto.Password),
            newRole: requestDto.NewRole,
            granterId: requestDto.GranterId,
            granterLogin: requestDto.GranterLogin);
    }

    [NonAction]
    private static UpdateAccountData GetCoreUpdateAccountData(UpdateAccountRequestDto requestDto)
    {
        return new UpdateAccountData(
                id: requestDto.Id,
                login: requestDto.Login,
                name: requestDto.Name,
                surname: requestDto.Surname,
                address: requestDto.Address,
                email: requestDto.Email,
                passwordHash: SHA256Hasher.GeneratePasswordHash(requestDto.ExistingPassword, repeatPassword: requestDto.ExistingPassword),
                newPasswordHash: requestDto.NewPassword != null ? SHA256Hasher.GeneratePasswordHash(requestDto.NewPassword, repeatPassword: requestDto.RepeatNewPassword) : null,
                nick: requestDto.Nick,
                phone: requestDto.Phone,
                shopId: requestDto.ShopId,
                warehouseId: requestDto.WarehouseId,
                requestedRole: requestDto.RequestedRole);
    }

    [NonAction]
    private static UserInfoResponseDto GetUserInfoResponseDto(Employee employee) =>
        new UserInfoResponseDto
        {
            Id = employee.Id,
            Login = employee.Login,
            Name = employee.Name,
            Surname = employee.Surname,
            Email = employee.Email,
            Nick = employee.Nick,
            Phone = employee.Phone,
            Role = employee.Role,
            ShopId = employee.ShopId,
            WarehouseId = employee.WarehouseId,
        };
}
