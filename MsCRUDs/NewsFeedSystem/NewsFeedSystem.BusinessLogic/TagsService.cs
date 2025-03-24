using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;

namespace NewsFeedSystem.BusinessLogic
{
    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsService(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }
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
