using System.Collections.Generic;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Класс-модель проекта в API </summary>
    public class ProjectDto
    {
        /// <summary> Id проекта </summary>
        public required string ProjectId { get; set; }

        /// <summary> Legacy-идентификатор (номер) проекта в старой системе </summary>
        public int LegacyProjectNumber { get; set; }
        
        /// <summary> Название проекта </summary>
        public required string Name { get; set; }
        
        /// <summary> URL проекта </summary>
        public string Url { get; set; }
        
        /// <summary> Логотип (эмблема) (imageUrl) проекта </summary>
        public string ImageUrl { get; set; }
        
        /// <summary> Номера дней обязательной активности по проекту (совещаний) </summary>
        public HashSet<byte> ScheduledDayNums { get; set; }

        /// <summary> Дата и время создания </summary>
        public string CreatedDt { get; set; }

        /// <summary> Дата и время изменения </summary>
        public string LastUpdateDt { get; set; }
    }
}
