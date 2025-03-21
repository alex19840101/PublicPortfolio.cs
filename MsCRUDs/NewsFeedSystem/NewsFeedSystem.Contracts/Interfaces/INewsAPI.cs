using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsFeedSystem.Contracts.Requests;

namespace NewsFeedSystem.Contracts.Interfaces
{
    public interface INewsAPI
    {
        Task<IActionResult> Create(CreateNewsRequestDto request);
        Task<IActionResult> Read(int newsId);
        Task<IActionResult> ReadHeadlines(int? maxNewsId, int? minNewsId);
        Task<IActionResult> Update(UpdateNewsRequestDto request);
        Task<IActionResult> Delete(int newsId);
    }
}
