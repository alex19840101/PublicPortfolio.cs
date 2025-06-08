using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models.Requests
{
    /// <summary> Запрос на обновление данных менеджера </summary>
    public class UpdateManagerRequest
    {
        /// <summary>
        /// Id менеджера*
        /// </summary>
        public uint Id { get; set; }
    }
}
