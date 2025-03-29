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
TestFixtures\
```

## **using stack:**
**- Platform:** .NET 9.0.

**- ORM:** EF Core.

**- DataBase**: Microsoft SQL Server Express Edition v15 - LocalDB.

**- Caching: Redis.

**- Unit Tests for Busines Logic services:**
- MsTest tests (AuthServiceTests);
- using extra NuGet packages: Moq, AutoFixture, FluentAssertions (methods versions with FluentAssertions and without FluentAssertions in AuthServiceTests).


**- Auth. (authentication and authorization):**
- JWT-tokens + roles. Authorization required in Create, Delete... methods.

**- Documentation**: Swagger (OpenAPI).

**- Logging**: Serilog (console, file).