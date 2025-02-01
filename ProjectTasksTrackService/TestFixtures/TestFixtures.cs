using System;
using AutoFixture;
using ProjectTasksTrackService.Core;

namespace TestFixtures
{
    public class TestFixtures
    {
        public static Project GetProjectFixtureWithAllFields(bool generateId = false, bool generateCode = true, bool generateName = true)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : 0;
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
