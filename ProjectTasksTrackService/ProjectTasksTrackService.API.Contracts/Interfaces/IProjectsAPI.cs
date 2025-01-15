using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.API.Contracts.Dto;

namespace ProjectTasksTrackService.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления проектами </summary>
    public interface IProjectsAPI
    {
        /// <summary> Импорт проектов (из старой системы проектов) </summary>
        Task<string> Import(IEnumerable<OldProjectDto> projects);
        /// <summary> Создание проекта </summary>
        Task<string> Create(ProjectDto project);

        /// <summary> Получение списка проектов (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        Task<IEnumerable<OldProjectDto>> GetProjectsOldDto();

        /// <summary> Получение списка проектов </summary>
        Task<IEnumerable<ProjectDto>> GetProjects();
        /// <summary> Получение проекта по projectId </summary>
        Task<ProjectDto> GetProjectById(string projectId);
        /// <summary> Получение проекта по legacyProjectNumber </summary>
        Task<ProjectDto> GetProjectByNum(int legacyProjectNumber);
        /// <summary> Получение проекта по названию (name) </summary>
        Task<ProjectDto> GetProjectByName(string name);
        
        /// <summary> Обновление проекта </summary>
        Task<string> UpdateProject(ProjectDto projectDto);
        /// <summary> Обновление названия проекта </summary>
        Task<string> UpdateName(string projectId, string newName);
        /// <summary> Обновление ссылки (url) проекта </summary>
        Task<string> UpdateUrl(string projectId, string url);
        /// <summary> Обновление логотипа (эмблемы) (imageUrl) проекта </summary>
        Task<string> UpdateImageUrl(string projectId, string imageUrl);
        /// <summary> Обновление номеров дней обязательной активности по проекту (совещаний) </summary>
        Task<string> UpdateScheduledDayNums(string projectId, HashSet<byte> scheduledDayNums);
        
        /// <summary> Удаление проекта </summary>
        Task<string> DeleteProject(string projectId, string projectSecretString);
    }
}
