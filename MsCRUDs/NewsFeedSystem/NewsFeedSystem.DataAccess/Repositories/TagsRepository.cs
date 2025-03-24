using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.DataAccess.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        public async Task<CreateResult> Create(Tag request)
        {
            throw new NotImplementedException();
        }

        public async Task<DeleteResult> Delete(uint tagId)
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> Get(uint tagId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tag>> GetTags(uint? maxTagId, uint? minTagId)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateResult> Update(Tag request)
        {
            throw new NotImplementedException();
        }
    }
}
