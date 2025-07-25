﻿namespace Buyers.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на удаление аккаунта покупателя
    /// </summary>
    public class DeleteAccountRequestDto
    {
        /// <summary>
        /// Id покупателя*
        /// </summary>
        public required uint Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) покупателя*
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Пароль*
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Пароль (повторно)*
        /// </summary>
        public required string RepeatPassword { get; set; }

        /// <summary>
        /// Id администратора (если удаление администратором)
        /// </summary>
        public uint? GranterId { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) администратора (если удаление администратором)*
        /// </summary>
        public string? GranterLogin { get; set; }
    }
}
