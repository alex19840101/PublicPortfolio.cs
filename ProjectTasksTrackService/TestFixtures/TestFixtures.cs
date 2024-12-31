﻿using System;
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
            var legacyProjectNumber = fixture.Build<int>().Create();
            var url = fixture.Build<string>().Create();
            var imageUrl = fixture.Build<string>().Create();
            var scheduledDayNums = fixture.Build<HashSet<byte>>().Create();

            return new Project(
                projectId: projectId,
                name: name,
                legacyProjectNumber: legacyProjectNumber,
                url: url,
                imageUrl: imageUrl,
                scheduledDayNums: scheduledDayNums);
        }

        public static Project GetProjectFixtureWithRequiredFields()
        {
            var fixture = new Fixture();

            var projectId = fixture.Build<string>().Create();
            var name = fixture.Build<string>().Create();

            return new Project(
                projectId: projectId,
                name: name);
        }
    }
}
