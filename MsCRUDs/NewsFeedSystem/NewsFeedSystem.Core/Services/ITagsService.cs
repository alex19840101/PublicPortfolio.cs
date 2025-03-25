using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Services
{
    public interface ITagsService
    {
        Task<CreateResult> Create(Tag tag);
        Task<Tag> Get(uint tagId);
        Task<IEnumerable<Tag>> GetTags(uint? minTagId, uint? maxTagId);
        Task<UpdateResult> Update(Tag tag);
        Task<DeleteResult> Delete(uint tagId);
    }
}
