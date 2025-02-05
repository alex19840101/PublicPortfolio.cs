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
            List<int> excludeIds = null,
            int setId = 0)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : setId;
            if (generateId)
            {
                if (excludeIds != null && excludeIds.Any() && setId == 0)
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

        /// <summary>
        /// Сгенерировать набор из 5+5 проектов, с конфликтами ##1,3,5 ([0],[2],[4])
        /// </summary>
        /// <returns> List(Project) существующие_проекты, List(Project) импортируемые_проекты </returns>
        public static (List<Project> existingProjects, List<Project> projectToImport) Simulate10ProjectsWithConflicts1_3_5_ToImport()
        {
            List<int> excludeIds = new List<int>();
            var project1 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project2 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project3 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project4 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project5 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);

            excludeIds.Clear();
            var project1conf = GetProjectFixtureWithAllFields(setId: project1.Id);
            var project3conf = GetProjectFixtureWithAllFields(setId: project3.Id);
            var project5conf = GetProjectFixtureWithAllFields(setId: project5.Id);


            List<Project> projects = new List<Project>
            {
                project1,
                project2,
                project3,
                project4,
                project5,
            };
            List<Project> imProjects = new List<Project>
            {
                project1conf,
                project2,
                project3conf,
                project4,
                project5conf,
            };

            return (projects, imProjects);
        }

        /// <summary>
        /// Сгенерировать набор из 5+3 проектов без конфликтов
        /// </summary>
        /// <returns> List(Project) существующие_проекты, List(Project) импортируемые_проекты </returns>
        public static (List<Project> existingProjects, List<Project> projectToImport) Simulate5And3ProjectsWithoutConflicts()
        {
            List<int> excludeIds = new List<int>();
            var project1 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project2 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project3 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project4 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project5 = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);

            var project1Imp = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project2Imp = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var project3Imp = GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);


            List<Project> projects = new List<Project>
            {
                project1,
                project2,
                project3,
                project4,
                project5,
            };
            List<Project> imProjects = new List<Project>
            {
                project1Imp,
                project2Imp,
                project3Imp,
            };

            return (projects, imProjects);
        }

        public static List<Project> GenerateProjectsList(uint count)
        {
            List<int> excludeIds = new List<int>();
            List<Project> projectList = new List<Project>();
            for (uint i = 0; i < count; i++)
                projectList.Add(GetProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds));
            
            return projectList;
        }

        public static string GenerateString()
        {
            var fixture = new Fixture();

            return fixture.Build<string>().Create();
        }

        public static List<Project> ReturnSomeOfProjects(List<Project> projects)
        {
            var random = new Random();

            int projectsCount = projects.Count;
            int randomCount = random.Next(1, projectsCount);

            List<Project> someOfProjectsList = new List<Project>();
            List<int> indexList = new List<int>();
            
            var returnCount = 0;
            int maxIndex = projectsCount - 1;
            int randomIndex = random.Next(0, maxIndex);

            while (returnCount == 0 || returnCount < randomCount)
            {
                while (indexList.Contains(randomIndex))
                    randomIndex = random.Next(0, maxIndex);

                someOfProjectsList.Add(projects[randomIndex]);
                indexList.Add(randomIndex);
                returnCount++;
            }

            return someOfProjectsList;
        }
    }
}
