using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ProjectTasksTrackService.Core;

namespace TestFixtures
{
    public class TestFixtures
    {
        public static Project GetProjectFixtureWithAllFields(
            bool generateId = false,
            bool generateCode = true,
            bool generateName = true,
            List<int> excludeIds = null)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : 0;
            if (generateId)
            {
                if (excludeIds != null && excludeIds.Any())
                {
                    while (excludeIds.Contains(id))
                        id = fixture.Create<int>();
                }
                excludeIds ??= new List<int>();
                excludeIds.Add(id);
            }

            var code = generateCode ? fixture.Build<string>().Create() : null;
            var name = generateName ? fixture.Build<string>().Create() : null;
            var url = fixture.Build<string>().Create();
            var imageUrl = fixture.Build<string>().Create();

            return new Project(
                id: id,
                code: code,
                name: name,
                url: url,
                imageUrl: imageUrl,
                createdDt: DateTime.Now,
                lastUpdateDt: DateTime.Now);
        }

        public static Project GetProjectFixtureWithRequiredFields(bool generateId = false)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : 0;
            var code = fixture.Build<string>().Create();
            var name = fixture.Build<string>().Create();

            return new Project(
                id: id,
                code: code,
                name: name);
        }

        public static int GenerateId() => new Fixture().Create<int>();
    }
}
