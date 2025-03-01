using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ProjectTasksTrackService.Core;

namespace TestFixtures
{
    public partial class TestFixtures
    {
        public static ProjectTask GetTaskFixtureWithAllFields(
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

            return new ProjectTask(
                id: id,
                projectId: projectId,
                code: code,
                name: name,
                repeatsType: ProjectTasksTrackService.Core.Enums.TaskRepeatsType.OneTimeTaskEvent,
                repeatInDays: null,
                projectSubDivisionId: fixture.Create<int>(),
                url1: url,
                url2: url,
                imageUrl: imageUrl,
                createdDt: DateTime.Now,
                lastUpdateDt: DateTime.Now,
                deadLineDt: DateTime.Now.AddDays(365),
                doneDt: null);
        }

        public static ProjectTask GetTaskFixtureWithRequiredFields(
            bool generateId = false,
            bool generateCode = true)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : 0;
            var projectId = fixture.Create<int>();
            var code = generateCode ? fixture.Build<string>().Create() : null;
            var name = fixture.Build<string>().Create();

            return new ProjectTask(
                id: id,
                projectId: projectId,
                code: code,
                name: name,
                repeatsType: ProjectTasksTrackService.Core.Enums.TaskRepeatsType.OneTimeTaskEvent,
                repeatInDays: null);
        }

        /// <summary>
        /// Сгенерировать набор из 5+5 задач, с конфликтами ##1,3,5 ([0],[2],[4])
        /// </summary>
        /// <returns> List(ProjectTask) существующие_задачи, List(ProjectTask) импортируемые_задачи </returns>
        public static (List<ProjectTask> existingTasks, List<ProjectTask> tasksImport) Simulate10TasksWithConflicts1_3_5_ToImport()
        {
            List<int> excludeIds = new List<int>();
            var task1 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task2 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task3 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task4 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task5 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);

            excludeIds.Clear();
            var task1conf = GetTaskFixtureWithAllFields(setId: task1.Id);
            var task3conf = GetTaskFixtureWithAllFields(setId: task3.Id);
            var task5conf = GetTaskFixtureWithAllFields(setId: task5.Id);


            List<ProjectTask> existingTasks = new List<ProjectTask>
            {
                task1,
                task2,
                task3,
                task4,
                task5,
            };
            List<ProjectTask> tasksToImport = new List<ProjectTask>
            {
                task1conf,
                task2,
                task3conf,
                task4,
                task5conf,
            };

            return (existingTasks, tasksToImport);
        }

        /// <summary>
        /// Сгенерировать набор из 5+3 задач без конфликтов
        /// </summary>
        /// <returns> List(ProjectTask) существующие_задачи, List(ProjectTask) импортируемые_задачи </returns>
        public static (List<ProjectTask> existingTasks, List<ProjectTask> tasksToImport) Simulate5And3TasksWithoutConflicts()
        {
            List<int> excludeIds = new List<int>();
            var task1 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task2 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task3 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task4 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task5 = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);

            var task1Imp = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task2Imp = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);
            var task3Imp = GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds);


            List<ProjectTask> existingTasks = new List<ProjectTask>
            {
                task1,
                task2,
                task3,
                task4,
                task5,
            };
            List<ProjectTask> tasksToImport = new List<ProjectTask>
            {
                task1Imp,
                task2Imp,
                task3Imp,
            };

            return (existingTasks, tasksToImport);
        }

        public static List<ProjectTask> GenerateTasksList(uint count)
        {
            List<int> excludeIds = new List<int>();
            List<ProjectTask> projectList = new List<ProjectTask>();
            for (uint i = 0; i < count; i++)
                projectList.Add(GetTaskFixtureWithAllFields(generateId: true, excludeIds: excludeIds));
            
            return projectList;
        }

        
    }
}
