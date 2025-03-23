using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        Task<IActionResult> Create(TopicDto request);

        /// <summary>
        /// Получение темы
        /// </summary>
        /// <param name="topicId"> Id темы </param>
        /// <returns></returns>
        Task<IActionResult> Get(int topicId);

        /// <summary>
        /// Получение тем
        /// </summary>
        /// <param name="maxTopicId"> Максимальный Id темы </param>
        /// <param name="minTopicId"> Минимальный Id темы </param>
        /// <returns></returns>
        Task<IEnumerable<TopicDto>> GetTopics(int? maxTopicId, int? minTopicId);

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
        Task<IActionResult> Delete(int topicId);
    }
}
