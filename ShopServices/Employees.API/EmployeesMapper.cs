using System;
using Employees.API.Contracts.Requests;
using Employees.API.Contracts.Responses;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Enums;
using ShopServices.Core.Models.Events;

namespace Employees.API
{
    internal static class EmployeesMapper
    {
        internal static Employee GetCoreEmployee(RegisterRequestDto request) =>
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
                telegramChatId: null,
                notificationMethods: [NotificationMethod.Email, NotificationMethod.SMS],
                role: request.RequestedRole,
                granterId: null,
                createdDt: DateTime.Now,
                lastUpdateDt: null,
                shopId: request.ShopId,
                warehouseId: request.WarehouseId);

        internal static LoginData GetCoreLoginData(LoginRequestDto request)
        {
            return new LoginData(
                login: request.Login,
                passwordHash: SHA256Hasher.GeneratePasswordHash(request.Password, request.Password),
                timeoutMinutes: request.TimeoutMinutes);
        }


        internal static DeleteAccountData GetCoreDeleteAccountData(DeleteAccountRequestDto request)
        {
            return new DeleteAccountData(
                id: request.Id,
                login: request.Login,
                passwordHash: SHA256Hasher.GeneratePasswordHash(request.Password, request.RepeatPassword),
                granterId: request.GranterId,
                granterLogin: request.GranterLogin);
        }

        #region Logout не требуется для пет-проекта
        //private static LogoutData LogoutData(LogoutRequestDto request)
        //{
        //    return new LogoutData(
        //        login: request.Login,
        //        id: request.Id);
        //}
        #endregion Logout не требуется для пет-проекта

        internal static GrantRoleData GetCoreGrantRoleData(GrantRoleRequestDto requestDto)
        {
            return new GrantRoleData(
                id: requestDto.Id,
                login: requestDto.Login,
                passwordHash: SHA256Hasher.GeneratePasswordHash(requestDto.Password, repeatPassword: requestDto.Password),
                newRole: requestDto.NewRole,
                granterId: requestDto.GranterId,
                granterLogin: requestDto.GranterLogin);
        }

        internal static UpdateAccountData GetCoreUpdateAccountData(UpdateAccountRequestDto requestDto)
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
                    telegramChatId: requestDto.TelegramChatId,
                    shopId: requestDto.ShopId,
                    warehouseId: requestDto.WarehouseId,
                    requestedRole: requestDto.RequestedRole);
        }

        internal static UserInfoResponseDto GetUserInfoResponseDto(Employee employee) =>
            new UserInfoResponseDto
            {
                Id = employee.Id,
                Login = employee.Login,
                Name = employee.Name,
                Surname = employee.Surname,
                Email = employee.Email,
                Nick = employee.Nick,
                Phone = employee.Phone,
                TelegramChatId = employee.TelegramChatId,
                NotificationMethods = employee.NotificationMethods,
                Role = employee.Role,
                ShopId = employee.ShopId,
                WarehouseId = employee.WarehouseId,
            };

        /// <summary> Маппер в класс события "Работник зарегистрировался" </summary>
        /// <param name="employeeId"></param>
        /// <returns> Событие "Работник зарегистрировался" </returns>
        internal static EmployeeRegistered GetEmployeeRegistered(uint employeeId)
        {
            return new EmployeeRegistered(
                id: Guid.NewGuid(),
                created: DateTime.Now,
                message: nameof(EmployeeRegistered),
                notification: NotificationMessages.REGISTERED_NEW_EMPLOYEE.Replace("{id}", employeeId.ToString()),
                employeeId: employeeId);
        }

        /// <summary> Маппер в класс события "Работник обновил личные данные" </summary>
        /// <param name="employeeId"></param>
        /// <returns> Событие "Работник обновил личные данные" </returns>
        internal static EmployeeUpdated GetEmployeeUpdated(uint employeeId)
        {
            return new EmployeeUpdated(
                id: Guid.NewGuid(),
                created: DateTime.Now,
                message: nameof(EmployeeUpdated),
                notification: NotificationMessages.EMPLOYEE_UPDATED.Replace("{id}", employeeId.ToString()),
                employeeId: employeeId);
        }
    }
}
