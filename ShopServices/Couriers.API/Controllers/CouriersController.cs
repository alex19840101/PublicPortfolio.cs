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
        //TODO: Couriers.API, CouriersController

        /// <summary> Получение информации о работнике ((курьере)) по Id </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CourierInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCourierInfoById(uint id)
        {
            var employee = await _couriersService.GetUserInfo(id);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(CourierInfoResponseDto(employee));
        }

        /// <summary> Получение информации о работнике ((курьере)) по логину </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CourierInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCourierInfoByLogin(string login)
        {
            var employee = await _couriersService.GetUserInfo(login);

            if (employee is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(CourierInfoResponseDto(employee));
        }


        ///// <summary> Получение информации о работнике ((курьере)) по Id </summary>
        //[HttpGet]
        //[ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        //public async Task<IActionResult> GetUserInfoById(uint id)
        //{
        //    var employee = await _couriersService.GetUserInfo(id);

        //    if (employee is null)
        //        return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

        //    return Ok(UserInfoResponseDto(employee));
        //}

        ///// <summary> Получение информации о работнике ((курьере)) по логину </summary>
        //[HttpGet]
        //[ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        //public async Task<IActionResult> GetUserInfoByLogin(string login)
        //{
        //    var employee = await _couriersService.GetUserInfo(login);

        //    if (employee is null)
        //        return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

        //    return Ok(UserInfoResponseDto(employee));
        //}

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
        public async Task<IActionResult> UpdateCourier(UpdateCourierRequestDto updateCourierRequest)
        {
            var updateResult = await _couriersService.UpdateCourier(UpdateCourierRequest(updateCourierRequest));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Unauthorized)
                return new UnauthorizedObjectResult(new Result { Message = updateResult.Message });

            return Ok(updateResult);
        }


        [NonAction]
        private static UpdateCourierRequest UpdateCourierRequest(UpdateCourierRequestDto requestDto)
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



        [NonAction]
        private static CourierInfoResponseDto CourierInfoResponseDto(Courier courier)
        {
          
            return new CourierInfoResponseDto
            {
                Id = courier.Employee.Id,
                DriverLicenseCategory = courier.DriverLicenseCategory,
                Transport = courier.Transport,
                Areas = courier.Areas,
                DeliveryTimeSchedule = courier.DeliveryTimeSchedule
            };
        }
    }
}
