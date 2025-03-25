using System.Collections.Generic;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Repositories
{
    public interface ITagsRepository
    {
        Task<CreateResult> Create(Tag tag);
        Task<Tag> Get(uint tagId);
        Task<IEnumerable<Tag>> GetTags(uint? maxTagId, uint? minTagId);
        Task<UpdateResult> Update(Tag tag);
        Task<DeleteResult> Delete(uint tagId);
    }
}
