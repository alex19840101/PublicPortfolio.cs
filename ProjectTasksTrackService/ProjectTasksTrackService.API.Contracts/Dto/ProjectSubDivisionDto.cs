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

        /// <summary> Legacy-идентификатор (номер) проекта в старой системе </summary>
        public int LegacyRootProjectNumber { get; set; }

        /// <summary> Id проекта </summary>
        public required string ProjectId { get; set; }
    }
}
