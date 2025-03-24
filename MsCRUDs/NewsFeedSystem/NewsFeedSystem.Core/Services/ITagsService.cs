using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Services
{
    public interface ITagsService
    {
        Task<CreateResult> Create(Tag request);
        Task<Tag> Get(uint tagId);
        Task<IEnumerable<Tag>> GetTags(uint? maxTagId, uint? minTagId);
        Task<UpdateResult> Update(Tag request);
        Task<DeleteResult> Delete(uint tagId);
    }
}
