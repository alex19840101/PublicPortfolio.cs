# ProjectTasksTrackService

ProjectTasksTrackService - pet project service for projects, projectSubDivisions, tasks.

## **Entities:**
```csharp
AuthUser
Project
ProjectSubDivision
ProjectTask
```

### **Controllers:**
```csharp
AuthController
ProjectsController
ProjectSubDivisionsController
TasksController
```

## **Projects:**
```csharp
ProjectTasksTrackService.API\
ProjectTasksTrackService.API.Contracts\
ProjectTasksTrackService.BusinessLogic\
ProjectTasksTrackService.BusinessLogic.MsTests\
ProjectTasksTrackService.BusinessLogic.nTests\
ProjectTasksTrackService.BusinessLogic.xTests\
ProjectTasksTrackService.Core\
ProjectTasksTrackService.DataAccess\
TestFixtures\
```

## **using stack:**
**Platform:** .NET 9.0.
**ORM:** EF Core.
**DataBase**: PostgreSQL 17.
**Unit Tests for Busines Logic services:**
- MsTest tests (AuthServiceTests, ProjectsServiceTests, SubProjectsServiceTests, TasksServiceTests);
- Nunit tests (ProjectsServiceTests, SubProjectsServiceTests, TasksServiceTests);
- XUnit tests (ProjectsServiceTests, SubProjectsServiceTests, TasksServiceTests);
- using extra NuGet packages: Moq, AutoFixture, FluentAssertions (methods versions with FluentAssertions and without FluentAssertions).
**Auth. (authentication and authorization):**
- JWT-tokens + roles. Authorization required in Delete... methods.
**Documentation**: Swagger (OpenAPI).
**Logging**: Serilog (console, file).