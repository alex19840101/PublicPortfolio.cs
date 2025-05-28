using System.Net;
using System.Threading.Tasks;
using Couriers.API.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Services;

namespace Couriers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles ="admin,courier,manager")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CouriersController : ControllerBase
    {
        private readonly ILogger<CouriersController> _logger;
        private readonly ICouriersService _couriersService;

        public CouriersController(
            ICouriersService couriersService,
            ILogger<CouriersController> logger)
        {
            _couriersService = couriersService;
            _logger = logger;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotImplemented)]
        public IActionResult Create()
        {
            return new ObjectResult("Use Employees.API/Register") { StatusCode = StatusCodes.Status501NotImplemented };
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotImplemented)]
        public IActionResult Delete()
        {
            return new ObjectResult("Use Employees.API/DeleteAccount") { StatusCode = StatusCodes.Status501NotImplemented };
        }

        /// <summary> Получение информации о работнике ((курьере)) по Id </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserInfoById(uint id)
        {
            var employee = await _couriersService.GetUserInfo(id);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(UserInfoResponseDto(employee));
        }

        /// <summary> Получение информации о работнике ((курьере)) по логину </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserInfoByLogin(string login)
        {
            var employee = await _couriersService.GetUserInfo(login);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(UserInfoResponseDto(employee));
        }

        [NonAction]
        private static UserInfoResponseDto UserInfoResponseDto(Courier courier)
        {
            var employee = courier.Employee;
           
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
