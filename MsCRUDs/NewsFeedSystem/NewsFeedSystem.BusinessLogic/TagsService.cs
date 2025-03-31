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
        public async Task<CreateResult> Create(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(ErrorStrings.TAG_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(tag.Name))
                return new CreateResult(ErrorStrings.TAG_NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);


            var createResult = await _tagsRepository.Create(tag);

            return createResult;
        }

        public async Task<DeleteResult> Delete(uint tagId)
        {
            return await _tagsRepository.Delete(tagId);
        }

        public async Task<Tag?> Get(uint tagId)
        {
            return await _tagsRepository.Get(tagId);
        }

        public async Task<IEnumerable<Tag>> GetTags(uint? minTagId, uint? maxTagId)
        {
            if (minTagId > maxTagId)
                return new List<Tag>();

            return await _tagsRepository.GetTags(minTagId, maxTagId);
        }

        public async Task<UpdateResult> Update(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(ErrorStrings.TAG_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(tag.Name))
                return new UpdateResult(ErrorStrings.TAG_NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            return await _tagsRepository.Update(tag);
        }
    }
}
