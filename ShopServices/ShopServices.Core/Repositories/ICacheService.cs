namespace ShopServices.Core.Repositories
{
    /// <summary> Интерфейс кэш-сервиса </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Получение объекта из кэша
        /// </summary>
        /// <typeparam name="T"> Класс кэшируемого объекта типа T </typeparam>
        /// <param name="key"> Ключ </param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Помещение объекта в кэш
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"> Ключ </param>
        /// <param name="value"> Кэшируемый объект типа T </param>
        /// <returns></returns>
        T Set<T>(string key, T value);
    }
}
