﻿namespace Employees.API.Contracts.Responses
{
    /// <summary> Класс ответа на запросы к /Auth </summary>
    public class AuthResponseDto
    {
        /// <summary> Числовой идентификатор - id пользователя </summary>
        public uint? Id { get; set; }

        /// <summary> Токен </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; } = default!;
    }
}
