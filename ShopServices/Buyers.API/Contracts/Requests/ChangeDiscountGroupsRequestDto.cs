using System.Collections.Generic;

namespace Buyers.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на изменение групп скидок для покупателя
    /// </summary>
    public class ChangeDiscountGroupsRequestDto
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
        /// Список id групп скидок для покупателя
        /// </summary>
        public List<uint>? DiscountGroups { get; set; }

        /// <summary>
        /// Id администратора/менеджера*
        /// </summary>
        public required uint GranterId { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) администратора/менеджера*
        /// </summary>
        public required string GranterLogin { get; set; }
    }
}
