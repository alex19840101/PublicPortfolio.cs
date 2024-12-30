using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class ErrorStrings
    {
        public const string PROJECT_ID_SHOULD_NOT_BE_EMPTY = "ProjectId should be not empty";
        public const string PROJECT_NAME_SHOULD_NOT_BE_EMPTY = "Project Name should be not empty";
        public const string PROJECTS_LIST_TO_IMPORT_SHOULD_NOT_BE_NULL = "Projects list to import should not be null";
        public const string PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED = "Projects list to import should contain at least 1 project";
    }
}
