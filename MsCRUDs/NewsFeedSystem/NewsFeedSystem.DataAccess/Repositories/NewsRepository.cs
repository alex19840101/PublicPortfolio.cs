using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.DataAccess.Entities;

namespace NewsFeedSystem.DataAccess.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsFeedSystemDbContext _dbContext;
        private readonly ICacheService _cacheService;
        const int LIMIT_COUNT = 10;

        public NewsRepository(NewsFeedSystemDbContext dbContext,
            ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<CreateResult> Create(NewsPost newsPost)
        {
            ArgumentNullException.ThrowIfNull(newsPost);
            var newNewsEntity = News.NewsEntity(newsPost);

            await _dbContext.AddAsync(newNewsEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newNewsEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id

            _cacheService.Set<NewsPost>(newNewsEntity.Id.ToString(), newsPost);
            return new CreateResult
            {
                Id = newNewsEntity.Id,
                StatusCode = HttpStatusCode.Created
            };
        }

        public async Task<DeleteResult> DeleteNewsPost(uint newsId)
        {
            var entityNews = await _dbContext.News
                .AsNoTracking()
                .SingleOrDefaultAsync(n => n.Id == newsId);

            if (entityNews is null)
                return new DeleteResult(ErrorStrings.NEWS_NOT_FOUND, HttpStatusCode.NotFound);

            _dbContext.News.Remove(entityNews);
            await _dbContext.SaveChangesAsync();

            return new DeleteResult(ErrorStrings.OK, HttpStatusCode.OK);
        }

        public async Task<NewsPost?> Get(uint newsId)
        {
            var cachedNewsPost = _cacheService.Get<NewsPost>(newsId.ToString());

            if (cachedNewsPost != null)
            {
                return cachedNewsPost;
            }
            
            var entityNews = await _dbContext.News
                .AsNoTracking()
                .SingleOrDefaultAsync(n => n.Id == newsId);

            if (entityNews is null)
                return null;

            var newsPost = entityNews.GetCoreNewsPost();

            _cacheService.Set<NewsPost>(newsId.ToString(), newsPost);

            return newsPost;
        }

        public async Task<IEnumerable<HeadLine>> GetHeadlines(uint? minNewsId, uint? maxNewsId)
        {
            if (maxNewsId == null)
            {
                maxNewsId = await _dbContext.News.AsNoTracking().MaxAsync(n => n.Id);
                minNewsId ??= maxNewsId > LIMIT_COUNT ? maxNewsId - LIMIT_COUNT : 1;
            }

            minNewsId ??= maxNewsId - LIMIT_COUNT;

            List<News> entityNewsLst = await _dbContext.News.AsNoTracking()
                .Where(n => n.Id >= minNewsId && n.Id <= maxNewsId).ToListAsync();

            if (entityNewsLst.Count == 0)
                return [];

            return GetHeadlines(entityNewsLst);
        }

        public async Task<IEnumerable<HeadLine>> GetHeadlinesByTag(uint tagId, uint minNewsId)
        {
            try
            {
                uint maxNewsId = await _dbContext.News.AsNoTracking()
                .Where(n => n.Tags.Contains(tagId)).MaxAsync(n => n.Id);

                var entityNewsLst = await _dbContext.News
                    .AsNoTracking()
                    .Where(n => n.Id >= minNewsId && n.Id <= maxNewsId && n.Tags.Contains(tagId))
                    .ToListAsync();

                return GetHeadlines(entityNewsLst);
            }
            catch (InvalidOperationException ex)
            {
                if (string.Equals(ex.Message, "Sequence contains no elements."))
                    return [];

                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HeadLine>> GetHeadlinesByTopic(uint topicId, uint minNewsId)
        {
            try
            {
                uint maxNewsId = await _dbContext.News.AsNoTracking()
                    .Where(n => n.Topics.Contains(topicId)).MaxAsync(n => n.Id);

                var entityNewsLst = await _dbContext.News
                    .AsNoTracking()
                    .Where(n => n.Id >= minNewsId && n.Id <= maxNewsId && n.Topics.Contains(topicId))
                    .ToListAsync();

                return GetHeadlines(entityNewsLst);
            }
            catch (InvalidOperationException ex)
            {
                if (string.Equals(ex.Message, "Sequence contains no elements."))
                    return [];

                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UpdateResult> Update(NewsPost newsPost)
        {
            ArgumentNullException.ThrowIfNull(newsPost);

            var entityNews = await _dbContext.News
                .SingleOrDefaultAsync(n => n.Id == newsPost.Id);

            if (entityNews is null)
                return new UpdateResult(ErrorStrings.NEWS_NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(newsPost.Headline, entityNews.Headline)) entityNews.UpdateHeadline(newsPost.Headline);
            if (!string.Equals(newsPost.Text, entityNews.Text)) entityNews.UpdateText(newsPost.Text);
            
            if (!string.Equals(newsPost.URL, entityNews.Url)) entityNews.UpdateUrl(newsPost.URL);
            if (!string.Equals(newsPost.Author, entityNews.Author)) entityNews.UpdateAuthor(newsPost.Author);

            if (!newsPost.Topics.SequenceEqual(entityNews.Topics))
                entityNews.UpdateTopics(newsPost.Topics);

            if (!newsPost.Tags.SequenceEqual(entityNews.Tags))
                entityNews.UpdateTags(newsPost.Tags);

            if (newsPost.Updated != entityNews.Updated)
                entityNews.UpdateLastUpdateDt(newsPost.Updated);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                entityNews.UpdateLastUpdateDt(DateTime.Now);
                await _dbContext.SaveChangesAsync();
                _cacheService.Set<NewsPost>(newsPost.Id.ToString(), newsPost);
                return new UpdateResult(ErrorStrings.NEWS_UPDATED, HttpStatusCode.OK);
            }
            return new UpdateResult(ErrorStrings.NEWS_IS_ACTUAL, HttpStatusCode.OK);
        }

        private static IEnumerable<HeadLine> GetHeadlines(IEnumerable<News> entityNewsLst)
        {
            return entityNewsLst.Select(news => new HeadLine(
                        id: news.Id,
                        headLine: news.Headline,
                        tags: news.Tags,
                        topics: news.Topics,
                        created: news.Created));
        }
    }
}
