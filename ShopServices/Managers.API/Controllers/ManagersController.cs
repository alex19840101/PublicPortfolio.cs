using System.Net;
using System.Threading.Tasks;
using Managers.API.Contracts.Requests;
using Managers.API.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;
using ShopServices.Core.Services;
namespace Managers.API.Controllers
{
    /// <summary> Контроллер управления данными менеджеров </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
    [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
    public class ManagersController : ControllerBase
    {
        private readonly ILogger<ManagersController> _logger;
        private readonly IManagersService _managersService;

        /// <summary> Конструктор контроллера управления данными менеджеров </summary>
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
        
        /// <summary> Получение информации о работнике ((менеджере)) по Id </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ManagerInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetManagerById(uint id)
        {
            var employee = await _managersService.GetManager(id);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(GetManagerResponseDto(employee));
        }

        /// <summary> Получение информации о работнике ((менеджере)) по логину </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ManagerInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetManagerByLogin(string login)
        {
            var employee = await _managersService.GetManager(login);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(GetManagerResponseDto(employee));
        }

        /// <summary>
        /// Обновление данных менеджера
        /// </summary>
        /// <param name="updateManagerRequest"> UpdateManagerRequestDto-Запрос на обновление данных менеджера </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> UpdateManager(UpdateManagerRequestDto updateManagerRequest)
        {
            var updateResult = await _managersService.UpdateManager(GetCoreUpdateManagerRequest(updateManagerRequest));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Unauthorized)
                return new UnauthorizedObjectResult(new Result { Message = updateResult.Message });

            if (updateResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(updateResult);

            return Ok(updateResult);
        }

        /// <summary>
        /// Маппер Core.Models.Manager - ManagerResponseDto
        /// </summary>
        /// <param name="manager"> Core.Models.Manager - менеджер </param>
        /// <returns></returns>
        [NonAction]
        private static ManagerInfoResponseDto GetManagerResponseDto(Manager manager)
        {
            var employee = manager.Employee;

            return new ManagerInfoResponseDto
            {
                Id = employee.Id,
                EmployeeAccount = new EmployeeAccountDto
                {
                    Id = manager.Employee.Id,
                    Login = manager.Employee.Login,
                    Name = manager.Employee.Name,
                    Surname = manager.Employee.Surname,
                    Email = manager.Employee.Email,
                    Nick = manager.Employee.Nick,
                    Phone = manager.Employee.Phone,
                    Role = manager.Employee.Role
                },
                WorkStatus = manager.WorkStatus,
                ClientGroups = manager.ClientGroups,
                GoodsCategories = manager.GoodsCategories,
                Services = manager.Services
            };
        }

        /// <summary>
        /// Маппер UpdateManagerRequestDto - Core.Models.Requests.UpdateManagerRequest
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [NonAction]
        private static UpdateManagerRequest GetCoreUpdateManagerRequest(UpdateManagerRequestDto requestDto)
        {
            return new UpdateManagerRequest
            {
                Id = requestDto.Id,
                WorkStatus = requestDto.WorkStatus,
                ClientGroups = requestDto.ClientGroups,
                GoodsCategories = requestDto.GoodsCategories,
                Services = requestDto.Services
            };
        }
    }
}
