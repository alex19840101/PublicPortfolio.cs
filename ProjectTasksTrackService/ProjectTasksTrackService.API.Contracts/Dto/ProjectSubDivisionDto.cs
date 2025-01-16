namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Направление (подраздел/модуль/часть) проекта </summary>
    public class ProjectSubDivisionDto
    {
        /// <summary> Id направления (подраздела/модуля/части) проекта </summary>
        public required int Id { get; set; }

        /// <summary> Название направления (подраздела/модуля/части) проекта </summary>
        public required string Name { get; set; }

        /// <summary> Дата и время создания </summary>
        public string CreatedDt { get; set; }
        
        /// <summary> Дата и время изменения </summary>
        public string LastUpdateDt { get; set; }

        /// <summary> Дата и время завершения подраздела/модуля/части проекта </summary>
        public string DoneDateTime { get; set; }

        /// <summary> Флаг готовности подраздела/модуля/части проекта </summary>
        public bool IsFinished { get; set; }

        /// <summary> Числовой идентификатор (номер) проекта, как в старой системе </summary>
        public int IntProjectId { get; set; }

        /// <summary> Id проекта </summary>
        public required string ProjectId { get; set; }
        
        /// <summary> Ссылка #1 ((локальная)) </summary>
        public string Url1 { get; set; }

        /// <summary> Ссылка #2 ((Интернет)) </summary>
        public string Url2 { get; set; }

        /// <summary> Изображение </summary>
        public string ImageUrl { get; set; }
        
        /// <summary> Срок (дата и время завершения) подпроекта (при необходимости) </summary>
        public string DeadLineDt { get; set; }
    }
}
