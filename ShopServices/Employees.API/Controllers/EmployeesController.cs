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
using ShopServices.Core.Enums;
using System.Collections.Generic;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Models.Events;

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
    /// <summary> MassTransit-публикатор ((в RabbitMQ и/или Kafka)) </summary>
    private readonly IPublishEndpoint _massTransitPublishEndpoint;
    private readonly ILogger<EmployeesController> _logger;

    /// <summary> Конструктор контроллера управления аутентификацией работников </summary>
    public EmployeesController(IEmployeesService employeesService,
        IPublishEndpoint publishEndpoint,
        ILogger<EmployeesController> logger)
    {
        _employeesService = employeesService;
        _massTransitPublishEndpoint = publishEndpoint;
        _logger = logger;
    }

    /// <summary> Регистрация работника </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var registerResult = await _employeesService.Register(EmployeesMapper.GetCoreEmployee(request));

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
        _logger.LogInformation((EventId)(int)result!.Id!, @"Registered Employee {result.Id}", result!.Id!);
        await _massTransitPublishEndpoint.Publish<EmployeeRegistered>(message: EmployeesMapper.GetEmployeeRegistered(employeeId: (uint)registerResult!.Id!.Value));

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
        var loginResult = await _employeesService.Login(EmployeesMapper.GetCoreLoginData(request));

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
    [Authorize(Roles = Roles.Admin)]
    [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
    public async Task<IActionResult> GrantRole(GrantRoleRequestDto request)
    {
        var grantResult = await _employeesService.GrantRole(EmployeesMapper.GetCoreGrantRoleData(request));

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
        var updateResult = await _employeesService.UpdateAccount(EmployeesMapper.GetCoreUpdateAccountData(request));

        if (updateResult.StatusCode == HttpStatusCode.NotFound)
            return NotFound(updateResult);

        if (updateResult.StatusCode == HttpStatusCode.Unauthorized)
            return new UnauthorizedObjectResult(new Result { Message = updateResult.Message });

        if (updateResult.StatusCode == HttpStatusCode.Conflict)
            return new ConflictObjectResult(updateResult);

        await _massTransitPublishEndpoint.Publish<EmployeeUpdated>(message: EmployeesMapper.GetEmployeeUpdated(employeeId: request!.Id));

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
        var deleteResult = await _employeesService.DeleteAccount(EmployeesMapper.GetCoreDeleteAccountData(request));

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
    [Authorize(Roles = Roles.Admin)]
    [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
    public async Task<IActionResult> GetUserInfoById(uint id)
    {
        var employee = await _employeesService.GetUserInfo(id);

        if (employee is null)
            return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

        return Ok(EmployeesMapper.GetUserInfoResponseDto(employee));
    }

    /// <summary> Получение информации о работнике по логину (для администраторов) </summary>
    [HttpGet]
    [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.Admin)]
    [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
    public async Task<IActionResult> GetUserInfoByLogin(string login)
    {
        var employee = await _employeesService.GetUserInfo(login);

        if (employee is null)
            return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

        return Ok(EmployeesMapper.GetUserInfoResponseDto(employee));
    }

    
}
