using System.Net;
using System.Threading.Tasks;
using Couriers.API.Contracts.Requests;
using Couriers.API.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Requests;
using ShopServices.Core.Services;

namespace Couriers.API.Controllers
{
    /// <summary> Контроллер управления данными курьеров </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles ="admin,courier,manager")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CouriersController : ControllerBase
    {
        private readonly ILogger<CouriersController> _logger;
        private readonly ICouriersService _couriersService;


        /// <summary> Конструктор контроллера управления данными курьеров </summary>
        public CouriersController(
            ICouriersService couriersService,
            ILogger<CouriersController> logger)
        {
            _couriersService = couriersService;
            _logger = logger;
        }

        /// <summary> Подсказка по регистрации аккаунта курьера </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotImplemented)]
        public IActionResult Create()
        {
            return new ObjectResult("Use Employees.API/Register") { StatusCode = StatusCodes.Status501NotImplemented };
        }

        /// <summary> Подсказка по удалению аккаунта курьера </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotImplemented)]
        public IActionResult Delete()
        {
            return new ObjectResult("Use Employees.API/DeleteAccount") { StatusCode = StatusCodes.Status501NotImplemented };
        }

        /// <summary> Получение информации о работнике ((курьере)) по Id </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CourierInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCourierById(uint id)
        {
            var employee = await _couriersService.GetCourier(id);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(GetCourierInfoResponseDto(employee));
        }

        /// <summary> Получение информации о работнике ((курьере)) по логину </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CourierInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCourierByLogin(string login)
        {
            var employee = await _couriersService.GetCourier(login);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(GetCourierInfoResponseDto(employee));
        }

        /// <summary>
        /// Обновление данных курьера
        /// </summary>
        /// <param name="updateCourierRequest"> UpdateCourierRequestDto-Запрос на обновление данных курьера </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> UpdateCourier(UpdateCourierRequestDto updateCourierRequest)
        {
            var updateResult = await _couriersService.UpdateCourier(GetCoreUpdateCourierRequest(updateCourierRequest));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Unauthorized)
                return new UnauthorizedObjectResult(new Result { Message = updateResult.Message });

            if (updateResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(updateResult);

            return Ok(updateResult);
        }

        /// <summary>
        /// Маппер UpdateCourierRequestDto - Core.Models.Requests.UpdateCourierRequest
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [NonAction]
        private static UpdateCourierRequest GetCoreUpdateCourierRequest(UpdateCourierRequestDto requestDto)
        {
            return new UpdateCourierRequest
            {
                Id = requestDto.Id,
                DriverLicenseCategory = requestDto.DriverLicenseCategory,
                Transport = requestDto.Transport,
                Areas = requestDto.Areas,
                DeliveryTimeSchedule = requestDto.DeliveryTimeSchedule
            };
        }


        /// <summary>
        /// Маппер Core.Models.Courier - CourierInfoResponseDto
        /// </summary>
        /// <param name="courier"> Core.Models.Courier - курьер </param>
        /// <returns></returns>
        [NonAction]
        private static CourierInfoResponseDto GetCourierInfoResponseDto(Courier courier)
        {
            return new CourierInfoResponseDto
            {
                Id = courier.Employee.Id,
                EmployeeAccount = new EmployeeAccountDto
                {
                    Id = courier.Employee.Id,
                    Login = courier.Employee.Login,
                    Name = courier.Employee.Name,
                    Surname = courier.Employee.Surname,
                    Email = courier.Employee.Email,
                    Nick = courier.Employee.Nick,
                    Phone = courier.Employee.Phone,
                    Role = courier.Employee.Role
                },
                DriverLicenseCategory = courier.DriverLicenseCategory,
                Transport = courier.Transport,
                Areas = courier.Areas,
                DeliveryTimeSchedule = courier.DeliveryTimeSchedule
            };
        }
    }
}
