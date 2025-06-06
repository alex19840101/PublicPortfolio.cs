using System.Net;
using System.Threading.Tasks;
using Managers.API.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Services;
namespace Managers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin,courier,manager")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ManagersController : ControllerBase
    {
        private readonly ILogger<ManagersController> _logger;
        private readonly IManagersService _managersService;

        public ManagersController(
            IManagersService managersService,
            ILogger<ManagersController> logger)
        {
            _logger = logger;
            _managersService = managersService;
        }

        /// <summary> Подсказка по регистрации аккаунта менеджера </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotImplemented)]
        public IActionResult Create()
        {
            return new ObjectResult("Use Employees.API/Register") { StatusCode = StatusCodes.Status501NotImplemented };
        }

        /// <summary> Подсказка по удалению аккаунта менеджера </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotImplemented)]
        public IActionResult Delete()
        {
            return new ObjectResult("Use Employees.API/DeleteAccount") { StatusCode = StatusCodes.Status501NotImplemented };
        }
        
        //TODO: Managers.API, ManagersController
        /// <summary> Получение информации о работнике ((менеджере)) по Id </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserInfoById(uint id)
        {
            var employee = await _managersService.GetUserInfo(id);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(UserInfoResponseDto(employee));
        }

        /// <summary> Получение информации о работнике ((менеджере)) по логину </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserInfoByLogin(string login)
        {
            var employee = await _managersService.GetUserInfo(login);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(UserInfoResponseDto(employee));
        }

        [NonAction]
        private static UserInfoResponseDto UserInfoResponseDto(Manager manager)
        {
            var employee = manager.Employee;

            return new UserInfoResponseDto
            {
                Id = employee.Id,
                Login = employee.Login,
                Name = employee.Name,
                Surname = employee.Surname,
                Email = employee.Email,
                Nick = employee.Nick,
                Phone = employee.Phone,
                Role = employee.Role
            };
        }
    }
}
