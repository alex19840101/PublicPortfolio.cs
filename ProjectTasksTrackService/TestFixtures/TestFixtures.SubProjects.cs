using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ProjectTasksTrackService.Core;

namespace TestFixtures
{
    public partial class TestFixtures
    {
        public static ProjectSubDivision GetSubProjectFixtureWithAllFields(
            bool generateId = false,
            bool generateCode = true,
            bool generateName = true,
            List<int> excludeIds = null,
            int setId = 0,
            int setProjectId = 0)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : setId;
            var projectId = setProjectId == 0 ? fixture.Create<int>() : setProjectId;
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

            return new ProjectSubDivision(
                id: id,
                projectId: projectId,
                code: code,
                name: name,
                url1: url,
                url2: url,
                imageUrl: imageUrl,
                createdDt: DateTime.Now,
                lastUpdateDt: DateTime.Now,
                deadLineDt: DateTime.Now.AddDays(365),
                doneDt: null);
        }

        public static ProjectSubDivision GetSubProjectFixtureWithRequiredFields(bool generateId = false)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : 0;
            var projectId = fixture.Create<int>();
            var code = fixture.Build<string>().Create();
            var name = fixture.Build<string>().Create();

            return new ProjectSubDivision(
                id: id,
                projectId: projectId,
                code: code,
                name: name);
        }

        /// <summary>
        /// Сгенерировать набор из 5+5 подпроектов, с конфликтами ##1,3,5 ([0],[2],[4])
        /// </summary>
        /// <returns> List(ProjectSubDivision) существующие_подпроекты, List(ProjectSubDivision) импортируемые_подпроекты </returns>
        public static (List<ProjectSubDivision> existingSubProjects, List<ProjectSubDivision> subProjectToImport) Simulate10SubProjectsWithConflicts1_3_5_ToImport()
        {
            List<int> excludeIds = new List<int>();
            var sub1 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub2 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub3 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub4 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub5 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);

            excludeIds.Clear();
            var sub1conf = GetSubProjectFixtureWithAllFields(setId: sub1.Id);
            var sub3conf = GetSubProjectFixtureWithAllFields(setId: sub3.Id);
            var sub5conf = GetSubProjectFixtureWithAllFields(setId: sub5.Id);


            List<ProjectSubDivision> existingSubprojects = new List<ProjectSubDivision>
            {
                sub1,
                sub2,
                sub3,
                sub4,
                sub5,
            };
            List<ProjectSubDivision> subProjectToImport = new List<ProjectSubDivision>
            {
                sub1conf,
                sub2,
                sub3conf,
                sub4,
                sub5conf,
            };

            return (existingSubprojects, subProjectToImport);
        }

        /// <summary>
        /// Сгенерировать набор из 5+3 подпроектов без конфликтов
        /// </summary>
        /// <returns> List(ProjectSubDivision) существующие_подпроекты, List(ProjectSubDivision) импортируемые_подпроекты </returns>
        public static (List<ProjectSubDivision> existingProjects, List<ProjectSubDivision> subProjectsToImport) Simulate5And3SubProjectsWithoutConflicts()
        {
            List<int> excludeIds = new List<int>();
            var sub1 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub2 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub3 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub4 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub5 = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);

            var sub1Imp = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub2Imp = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var sub3Imp = GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds);


            List<ProjectSubDivision> existingSubProjects = new List<ProjectSubDivision>
            {
                sub1,
                sub2,
                sub3,
                sub4,
                sub5,
            };
            List<ProjectSubDivision> subProjectsToImport = new List<ProjectSubDivision>
            {
                sub1Imp,
                sub2Imp,
                sub3Imp,
            };

            return (existingSubProjects, subProjectsToImport);
        }

        public static List<ProjectSubDivision> GenerateSubProjectsList(uint count)
        {
            List<int> excludeIds = new List<int>();
            List<ProjectSubDivision> projectList = new List<ProjectSubDivision>();
            for (uint i = 0; i < count; i++)
                projectList.Add(GetSubProjectFixtureWithAllFields(generateId: true, excludeIds: excludeIds));
            
            return projectList;
        }

        /// <summary>
        /// вернуть некоторое кол-во (от 1 до всех) проектов/подпроектов/задач из списка entities
        /// </summary>
        /// <typeparam name="T"> класс (проекта/подпроекта/задачи) </typeparam>
        /// <param name="entities"></param>
        /// <returns> некоторое кол-во (от 1 до всех) проектов/подпроектов/задач из списка entities </returns>
        public static List<T> ReturnSomeOfEntities<T>(List<T> entities)
        {
            var random = new Random();

            int projectsCount = entities.Count;
            int randomCount = random.Next(1, projectsCount);

            List<T> someOfProjectsList = new List<T>();
            List<int> indexList = new List<int>();
            
            var returnCount = 0;
            int maxIndex = projectsCount - 1;
            int randomIndex = random.Next(0, maxIndex);

            while (returnCount == 0 || returnCount < randomCount)
            {
                while (indexList.Contains(randomIndex))
                    randomIndex = random.Next(0, maxIndex);

                someOfProjectsList.Add(entities[randomIndex]);
                indexList.Add(randomIndex);
                returnCount++;
            }

            return someOfProjectsList;
        }
    }
}
