using System;
using System.Collections.Generic;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Класс-модель проекта в API </summary>
    public class ProjectDto
    {
        /// <summary> Числовой идентификатор - номер проекта, как в старой системе </summary>
        public int Id { get; set; }
        
        /// <summary> Код проекта </summary>
        public required string Code { get; set; }
        
        /// <summary> Название проекта </summary>
        public required string Name { get; set; }
        
        /// <summary> URL проекта </summary>
        public string Url { get; set; }
        
        /// <summary> Логотип (эмблема) (imageUrl) проекта </summary>
        public string ImageUrl { get; set; }
        
        /// <summary> Дата и время создания </summary>
        public DateTime? CreatedDt { get; set; }

        /// <summary> Дата и время изменения </summary>
        public DateTime? LastUpdateDt { get; set; }

        /// <summary> Дата и время завершения проекта </summary>
        public DateTime? DoneDateTime { get; set; }

        /// <summary> Срок (дата и время завершения) проекта по плану </summary>
        public DateTime? DeadLineDt { get; set; }
    }
}
