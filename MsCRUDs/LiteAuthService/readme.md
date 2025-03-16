# LiteAuthService

LiteAuthService - pet project service for Auth (Authentication and Authorization).

## **Entities:**
```csharp
AuthUser
```

### **Controllers:**
```csharp
AuthController
```

## **Projects:**
```csharp
LiteAuthService.API\
LiteAuthService.API.Contracts\
LiteAuthService.BusinessLogic\
LiteAuthService.BusinessLogic.MsTests\
LiteAuthService.Core\
LiteAuthService.DataAccess\
LiteAuthService.Database\
TestFixtures\
TestFixtures\
```

## **using stack:**
**- Platform:** .NET 9.0.

**- micro ORM:** Dapper.

**- DataBase**: Microsoft SQL Server Express Edition v15 - LocalDB.

**- Unit Tests for Busines Logic services:**
- MsTest tests (AuthServiceTests);
- using extra NuGet packages: Moq, AutoFixture, FluentAssertions (methods versions with FluentAssertions and without FluentAssertions).


**- Auth. (authentication and authorization):**
- JWT-tokens + roles. Authorization required in 3 methods (GetUserInfoById, GetUserInfoByLogin, GrantRole).

**- Documentation**: Swagger (OpenAPI).

**- Logging**: Serilog (console, file).