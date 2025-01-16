using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface ITasksRepository
    {
        Task<string> Add(ProjectTask project);
        Task<string> UpdateTask(ProjectTask project);
    }
}
