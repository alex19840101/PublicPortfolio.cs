using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsFeedSystem.API.Contracts.Auth.Requests;

namespace NewsFeedSystem.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера аутентификации и авторизации </summary>
    public interface IAuthAPI
    {
        /// <summary>
        /// Регистрация аккаунта
        /// </summary>
        Task<IActionResult> Register(RegisterRequestDto request);

        /// <summary>
        /// Вход в систему
        /// </summary>
        Task<IActionResult> Login(LoginRequestDto request);

        ///// <summary>
        ///// Выход из системы
        ///// </summary>
        //Task<IActionResult> Logout(LogoutRequestDto request);
        
        /// <summary>
        /// Обновление аккаунта
        /// </summary>
        Task<IActionResult> UpdateAccount(UpdateAccountRequestDto request);


        /// <summary>
        /// Предоставление (установка/сброс) роли аккаунту
        /// </summary>
        Task<IActionResult> GrantRole(GrantRoleRequestDto request);


        /// <summary>
        /// Удаление аккаунта
        /// </summary>
        Task<IActionResult> DeleteAccount(DeleteAccountRequestDto request);
    }
}
