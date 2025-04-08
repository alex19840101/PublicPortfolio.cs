# NewsFeedSystem

NewsFeedSystem - pet project service for news posts, topics, tags + auth.

## **Entities:**
```csharp
AuthUser
NewsPost
Topic
Tag
```

### **Controllers:**
```csharp
AuthController
NewsController
TagsController
TopicsController
```

## **Projects:**
```csharp
NewsFeedSystem.API\
NewsFeedSystem.BusinessLogic\
NewsFeedSystem.BusinessLogic.MsTests\
NewsFeedSystem.Contracts\
NewsFeedSystem.Core\
NewsFeedSystem.DataAccess\
NewsFeedSystem.GrpcClient\
NewsFeedSystem.GrpcService\
TestFixtures\
```

## **using stack:**
**- Platform:** .NET 9.0.

**- ORM:** EF Core.

**- DataBase**: Microsoft SQL Server Express Edition v15 - LocalDB.

**- Caching:** Redis.

**ASP.NET Core Web API**:
- NewsFeedSystem.API

**gRPC service and test client**: NewsFeedSystem.GrpcService, NewsFeedSystem.GrpcClient for all methods simple testing.

**- Unit Tests for Busines Logic services:**
- MsTest tests (AuthServiceTests, NewsServiceTests, TagsServiceTests, TopicsServiceTests);
- using extra NuGet packages: Moq, AutoFixture, FluentAssertions (methods versions with FluentAssertions and without FluentAssertions in AuthServiceTests).


**- Auth. (authentication and authorization):**
NewsFeedSystem.API, NewsFeedSystem.GrpcClient, NewsFeedSystem.GrpcService
- JWT-tokens + roles. Authorization required in Create, Delete..., Update, GrantRole, GetUserInfoById, GetUserInfoByLogin methods.

**- Documentation**: Swagger (OpenAPI) in NewsFeedSystem.API.

**- Logging**: Serilog (console, file) in NewsFeedSystem.API, NewsFeedSystem.GrpcService.