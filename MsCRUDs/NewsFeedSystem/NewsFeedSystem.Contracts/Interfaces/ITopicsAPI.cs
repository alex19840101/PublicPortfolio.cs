using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsFeedSystem.API.Contracts.Requests;

namespace NewsFeedSystem.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления новостными темами </summary>
    public interface ITopicsAPI
    {
        /// <summary>
        /// Создание новостной темы
        /// </summary>
        /// <param name="request"> Запрос на создание темы </param>
        /// <returns></returns>
        Task<IActionResult> Create(CreateTopicRequestDto request);

        /// <summary>
        /// Получение темы
        /// </summary>
        /// <param name="topicId"> Id темы </param>
        /// <returns></returns>
        Task<IActionResult> Get(uint topicId);

        /// <summary>
        /// Получение тем
        /// </summary>
        /// <param name="minTopicId"> Минимальный Id темы </param>
        /// <param name="maxTopicId"> Максимальный Id темы </param>
        /// <returns></returns>
        Task<IEnumerable<TopicDto>> GetTopics(uint? minTopicId, uint? maxTopicId = null);

        /// <summary>
        /// Обновление темы
        /// </summary>
        /// <param name="request"> Запрос на обновление темы </param>
        /// <returns></returns>
        Task<IActionResult> Update(TopicDto request);

        /// <summary>
        /// Удаление темы
        /// </summary>
        /// <param name="topicId"> Id темы </param>
        /// <returns></returns>
        Task<IActionResult> Delete(uint topicId);
    }
}
