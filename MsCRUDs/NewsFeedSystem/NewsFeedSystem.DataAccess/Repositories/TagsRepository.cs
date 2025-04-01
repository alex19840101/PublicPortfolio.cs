using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.DataAccess.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly NewsFeedSystemDbContext _dbContext;
        const int LIMIT_COUNT = 100;

        public TagsRepository(NewsFeedSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateResult> Create(Core.Tag tag)
        {
            ArgumentNullException.ThrowIfNull(tag);

            var newTagEntity = Entities.Tag.TagEntity(tag);

            await _dbContext.AddAsync(newTagEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newTagEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new CreateResult
            {
                Id = newTagEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ErrorStrings.OK
            };
        }

        public async Task<DeleteResult> Delete(uint tagId)
        {
            var entityTag = await _dbContext.Tags
            .AsNoTracking()
            .SingleOrDefaultAsync(t => t.Id == tagId);

            if (entityTag is null)
                return new DeleteResult(ErrorStrings.TAG_NOT_FOUND, HttpStatusCode.NotFound);

            _dbContext.Tags.Remove(entityTag);
            await _dbContext.SaveChangesAsync();

            return new DeleteResult(ErrorStrings.OK, HttpStatusCode.OK);
        }

        public async Task<Core.Tag?> Get(uint tagId)
        {
            var entityTag = await _dbContext.Tags
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == tagId);

            if (entityTag is null)
                return null;

            return entityTag.GetCoreTag();
        }

        public async Task<IEnumerable<Core.Tag>> GetTags(uint? minTagId, uint? maxTagId)
        {
            if (maxTagId == null)
            {
                maxTagId = await _dbContext.Tags.AsNoTracking().MaxAsync(t => t.Id);
                minTagId ??= maxTagId > LIMIT_COUNT ? maxTagId - LIMIT_COUNT : 1;
            }

            minTagId ??= maxTagId - LIMIT_COUNT;
            
            List<Entities.Tag> entityTagsLst = await _dbContext.Tags.AsNoTracking()
                .Where(t => t.Id >= minTagId && t.Id <= maxTagId).ToListAsync();

            if (entityTagsLst.Count == 0)
                return [];

            return GetTags(entityTagsLst);
        }

        public async Task<UpdateResult> Update(Core.Tag tag)
        {
            ArgumentNullException.ThrowIfNull(tag);

            var entityTag = await _dbContext.Tags
                .SingleOrDefaultAsync(t => t.Id == tag.Id);

            if (entityTag is null)
                return new UpdateResult(ErrorStrings.TAG_NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(tag.Name, entityTag.Name)) entityTag.UpdateTagName(tag.Name);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
                return new UpdateResult(ErrorStrings.TAG_UPDATED, HttpStatusCode.OK);
            }
            return new UpdateResult(ErrorStrings.TAG_IS_ACTUAL, HttpStatusCode.OK);
        }

        private static IEnumerable<Core.Tag> GetTags(IEnumerable<Entities.Tag> entityTagsLst)
        {
            return entityTagsLst.Select(t => new Core.Tag(
                        id: t.Id,
                        name: t.Name));
        }
    }
}
