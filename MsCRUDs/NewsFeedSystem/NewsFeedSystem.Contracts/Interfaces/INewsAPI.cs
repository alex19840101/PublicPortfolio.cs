using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsFeedSystem.API.Contracts.Responses;
using NewsFeedSystem.API.Contracts.Requests;

namespace NewsFeedSystem.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления новостными постами </summary>
    public interface INewsAPI
    {
        /// <summary>
        /// Создание новостного поста
        /// </summary>
        /// <param name="request"> Запрос на создание новостного поста </param>
        /// <returns></returns>
        Task<IActionResult> Create(CreateNewsRequestDto request);
        
        /// <summary>
        /// Получение новостного поста
        /// </summary>
        /// <param name="newsId"> Id новостного поста </param>
        /// <returns></returns>
        Task<IActionResult> GetNewsPost(uint newsId);

        /// <summary>
        /// Получение заголовков новостных постов
        /// </summary>
        /// <param name="minNewsId"> Минимальный Id новостного поста </param>
        /// <param name="maxNewsId"> Максимальный Id новостного поста </param>
        /// <returns></returns>
        Task<IEnumerable<HeadLineDto>> GetHeadlines(uint? minNewsId, uint? maxNewsId = null);

        /// <summary>
        /// Получение заголовков новостных постов по Id тега
        /// </summary>
        /// <param name="tagId"> Id тега </param>
        /// <param name="minNewsId"> Минимальный Id новостного поста </param>
        /// <returns></returns>
        Task<IEnumerable<HeadLineDto>> GetHeadlinesByTag(uint tagId, uint minNewsId);

        /// <summary>
        /// Получение заголовков новостных постов по Id темы
        /// </summary>
        /// <param name="topicId"> Id темы </param>
        /// <param name="minNewsId"> Минимальный Id новостного поста </param>
        /// <returns></returns>
        Task<IEnumerable<HeadLineDto>> GetHeadlinesByTopic(uint topicId, uint minNewsId);

        /// <summary>
        /// Обновление новостного поста
        /// </summary>
        /// <param name="request"> Запрос на обновление новостного поста </param>
        /// <returns></returns>
        Task<IActionResult> Update(UpdateNewsRequestDto request);
        /// <summary>
        /// Удаление новостного поста
        /// </summary>
        /// <param name="newsId"> Id новостного поста </param>
        /// <returns></returns>
        Task<IActionResult> Delete(uint newsId);
    }
}
