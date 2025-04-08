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

**ASP.NET Core Web API**:
- NewsFeedSystem.API

**gRPC service and test client with auth.:**
- NewsFeedSystem.GrpcService,
- NewsFeedSystem.GrpcClient for all methods simple testing with auth.

**- ORM:** EF Core.

**- DataBase**: Microsoft SQL Server Express Edition v15 - LocalDB.

**- Caching:** Redis.
Using for news posts caching.

**- Auth. (authentication and authorization):**
- JWT-tokens + roles.
- Authorization required in Create, Delete..., Update, GrantRole, GetUserInfoById, GetUserInfoByLogin methods.
Using in projects:
- NewsFeedSystem.API,
- NewsFeedSystem.GrpcService,
- NewsFeedSystem.GrpcClient

**- Documentation**: Swagger (OpenAPI) in NewsFeedSystem.API.

**- Logging**: Serilog (console, file) in NewsFeedSystem.API, NewsFeedSystem.GrpcService.

**- Unit Tests for Busines Logic services:**
- MsTest tests (AuthServiceTests, NewsServiceTests, TagsServiceTests, TopicsServiceTests);
- using extra NuGet packages: Moq, AutoFixture, FluentAssertions (methods versions with FluentAssertions and without FluentAssertions in AuthServiceTests).