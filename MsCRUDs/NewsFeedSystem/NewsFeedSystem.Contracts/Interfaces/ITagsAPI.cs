using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NewsFeedSystem.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления новостными тегами </summary>
    public interface ITagsAPI
    {
        /// <summary>
        /// Создание тега
        /// </summary>
        /// <param name="request"> Запрос на создание тега </param>
        /// <returns></returns>
        Task<IActionResult> Create(TagDto request);

        /// <summary>
        /// Получение тега
        /// </summary>
        /// <param name="tagId"> Id тега </param>
        /// <returns></returns>
        Task<IActionResult> Get(int tagId);

        /// <summary>
        /// Получение тегов
        /// </summary>
        /// <param name="maxTagId"> Максимальный Id тега </param>
        /// <param name="minTagId"> Минимальный Id тега </param>
        /// <returns></returns>
        Task<IEnumerable<TagDto>> GetTags(int? maxTagId, int? minTagId);

        /// <summary>
        /// Обновление тега
        /// </summary>
        /// <param name="request"> Запрос на обновление тега </param>
        /// <returns></returns>
        Task<IActionResult> Update(TagDto request);

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="tagId"> Id тега </param>
        /// <returns></returns>
        Task<IActionResult> Delete(int tagId);
    }
}
