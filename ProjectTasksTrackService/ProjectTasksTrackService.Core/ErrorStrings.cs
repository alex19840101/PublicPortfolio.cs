namespace ProjectTasksTrackService.Core
{
    public class ErrorStrings
    {
        public const string MORE_THAN_ONE_PROJECT_FOUND = "More than one project found";
        public const string PROJECT_NOT_FOUND = "Project not found";
        public const string OK = "OK";
        public const string CODE_SHOULD_BE_THE_SAME = "Project.Code should be the same";
        public const string PROJECT_UPDATED = "Project updated";
        public const string PROJECT_IS_ACTUAL = "Project is actual";
        public const string PROJECTS_SHOULD_CONTAIN_AT_LEAST_1_PROJECT = "Projects list in Import should contain at least 1 project.";
        
        public const string SUBDIVISIONS_SHOULD_CONTAIN_AT_LEAST_1_SUBDIVISION = "Subdivision list in Import should contain at least 1 subdivision.";
        public const string SUBDIVISION_NOT_FOUND = "Subdivision not found";
        public const string SUBDIVISION_UPDATED = "Subdivision updated";
        public const string SUBDIVISION_IS_ACTUAL = "Subdivision is actual";
        public const string MORE_THAN_ONE_SUBDIVISION_FOUND = "More than one subdivision found";
        public const string PARENT_PROJECT_NOT_FOUND = "Parent project by subdivision.projectId not found";
        public const string SUBDIVISION_PROJECT_ID_SHOULD_BE_THE_SAME = "Subdivision.ProjectId should be the same";
        public const string PARENT_SUBDIVISION_NOT_FOUND = "Parent subdivision by task.projectSubDivisionId not found";
        
        public const string TASKS_SHOULD_CONTAIN_AT_LEAST_1_TASK = "Tasks list in Import should contain at least 1 task.";
        public const string TASK_NOT_FOUND = "Task not found";
        public const string TASK_PROJECT_ID_SHOULD_BE_THE_SAME = "Task.ProjectId should be the same";
        public const string TASK_PROJECTSUBDIVISIONID_SHOULD_BE_THE_SAME = "Task.ProjectSubDivisionId should be the same";
        public const string TASK_UPDATED = "Task updated";
        public const string TASK_IS_ACTUAL = "Task is actual";
        public const string TASK_REPEATSTYPE_SHOULD_BE_THE_SAME = "Task.RepeatsType should be the same";
        public const string TASK_REPEATINDAYS_SHOULD_BE_THE_SAME = "Task.RepeatInDays should be the same";

        public const string EMPTY_OR_NULL_SECRET_STRING = "Empty/null secret string";
        public const string INVALID_SECRET_STRING = "Invalid secret string";
    }
}
