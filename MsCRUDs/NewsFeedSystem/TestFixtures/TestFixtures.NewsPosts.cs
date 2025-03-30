using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Auth;

namespace TestFixtures
{
    public partial class TestFixtures
    {
        /// <summary>
        /// Генерация фейк-объекта новостного поста (со всеми полями или частью полей)
        /// </summary>
        /// <param name="generateId"></param>
        /// <param name="generateHeadline"></param>
        /// <param name="generateText"></param>
        /// <param name="generateUrl"></param>
        /// <param name="generateAuthor"></param>
        /// <param name="generateTags"> генерировать ли список id тегов (Tags) </param>
        /// <param name="generateTopics"> генерировать ли список id тем (Topics) </param>
        /// <param name="excludeIds"></param>
        /// <param name="setId"></param>
        /// <param name="headline"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static NewsPost GetNewsPostFixtureWithAllFields(
                    bool generateId = false,
                    bool generateHeadline = true,
                    bool generateText = true,
                    bool generateUrl = true,
                    bool generateAuthor = true,
                    bool generateTags = true,
                    bool generateTopics = true,
                    List<uint> excludeIds = null,
                    uint setId = 0,
                    string headline = null,
                    string text = null)
        {
            var fixture = new Fixture();
            var id = generateId ? fixture.Create<uint>() : setId;
            if (generateId)
            {
                if (excludeIds != null && excludeIds.Any() && setId == 0)
                {
                    while (excludeIds.Contains(id))
                        id = fixture.Create<uint>();
                }
                excludeIds ??= new List<uint>();
                excludeIds.Add(id);
            }

            Random random = new Random();
            var tagsCount = random.Next(1, 10);
            var topicsCount = random.Next(1, 10);

            return new NewsPost(
                id: id,
                headLine: headline ?? GenerateStringIfTrueElseReturnNull(generateHeadline),
                text: text ?? GenerateStringIfTrueElseReturnNull(generateText),
                url: GenerateStringIfTrueElseReturnNull(generateUrl),
                author: GenerateStringIfTrueElseReturnNull(generateAuthor),
                tags: generateTags ? GenerateIdsList(fixture,(uint)tagsCount) : null,
                topics: generateTopics ? GenerateIdsList(fixture, (uint)topicsCount) : null,
                created: fixture.Create<DateTime>(),
                updated: DateTime.Now);

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }

        public static List<uint> GenerateIdsList(Fixture fixture, uint count)
        {
            List<uint> idsList = new List<uint>();
            for (int i = 0; i < count; i++)
                idsList.Add(fixture.Create<uint>());

            return idsList;
        }

        /// <summary>
        /// Генерация фейк-объекта новостного поста (со всеми обязательными полями или частью полей) без URL и Author
        /// </summary>
        /// <param name="generateId"></param>
        /// <param name="generateHeadline"></param>
        /// <param name="generateText"></param>
        /// <param name="generateTags"> генерировать ли список id тегов (Tags) </param>
        /// <param name="generateTopics"> генерировать ли список id тем (Topics) </param>
        /// <param name="excludeIds"></param>
        /// <param name="setId"></param>
        /// <param name="headline"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static NewsPost GetNewsPostFixtureWithRequiredFields(
                    bool generateId = false,
                    bool generateHeadline = true,
                    bool generateText = true,
                    bool generateTags = true,
                    bool generateTopics = true,
                    List<uint> excludeIds = null,
                    uint setId = 0,
                    string headline = null,
                    string text = null)
        {
            return GetNewsPostFixtureWithAllFields(generateId: generateId,
                    generateHeadline: generateHeadline,
                    generateText: generateText,
                    generateTags: generateTags,
                    generateTopics: generateTopics,
                    generateUrl: false,
                    generateAuthor: false,
                    excludeIds: excludeIds,
                    setId: setId,
                    headline: headline,
                    text: text);
        }



    }
}
