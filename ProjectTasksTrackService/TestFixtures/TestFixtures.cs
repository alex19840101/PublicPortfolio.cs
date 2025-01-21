using System;
using System.Collections.Generic;
using AutoFixture;
using ProjectTasksTrackService.Core;

namespace TestFixtures
{
    public class TestFixtures
    {
        public static Project GetProjectFixtureWithAllFields()
        {
            var fixture = new Fixture();

            var projectId = fixture.Build<string>().Create();
            var name = fixture.Build<string>().Create();
            var intProjectId = fixture.Create<int>();
            var url = fixture.Build<string>().Create();
            var imageUrl = fixture.Build<string>().Create();

            return new Project(
                code: projectId,
                name: name,
                id: intProjectId,
                url: url,
                imageUrl: imageUrl,
                createdDt: DateTime.Now,
                lastUpdateDt: DateTime.Now);
        }

        public static Project GetProjectFixtureWithRequiredFields()
        {
            var fixture = new Fixture();

            var projectId = fixture.Build<string>().Create();
            var name = fixture.Build<string>().Create();
            var intProjectId = fixture.Create<int>();

            return new Project(
                code: projectId,
                name: name,
                id: intProjectId);
        }
    }
}
