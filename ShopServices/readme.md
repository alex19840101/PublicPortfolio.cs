# ShopServices (In development)

ShopServices - pet project services for chain of shops (stores).

## **Entities:**
```csharp
Availability - availability of a product at shops (stores) and warehouses.
Buyer
Category (GoodsGroup) - a group of goods (products)
Courier
Delivery
EmailNotification
Employee
Manager
Order
OrderPosition
PhoneNotification
Price
Product (one unit of goods)
Shop
Trade - transaction information for order position (purchases and payments).
Warehouse
```

### **API, Controllers:**
```csharp
Buyers.API
Couriers.API
Deliveries.API
Employees.API
Goods.API
GoodsGroups.API
Managers.API
Notifications.API
Orders.API
Prices.API
Shops.API
Trade.API
Warehouses.API
```

## **Projects:**
```csharp
Buyers.API\
Couriers.API\
Delivery.API\
Employees.API\
Goods.API\
GoodsGroups.API\
Managers.API\
Notifications.API\
Orders.API\
Prices.API\
Shops.API\
ShopServices.Abstractions\
ShopServices.BusinessLogic\
ShopServices.BusinessLogic.MsTests\
ShopServices.Core\
ShopServices.DataAccess\
TestFixtures\
TestResults\
Trade.API\
Warehouse.API\
```

## **using stack:**
**- Platform:** .NET 9.0.

**ASP.NET Core Web API**:
- All ***.API projects.

**gRPC service and test client with auth.:**
- //TODO: GrpcService(s),
- //TODO: GrpcClient(s) for methods simple testing with auth.

**- ORM:** EF Core.

**- DataBase**: PostgreSQL 17.

**- Caching:** Redis.
//TODO: Using for products, categories.

**- Auth. (authentication and authorization):**
- JWT-tokens + roles.
- Authorization required in methods.
Using in APIs projects //TODO: auth. in GrpcService(s), GrpcClient(s)

**- Documentation**: Swagger (OpenAPI) in APIs.

**- Logging**: Serilog (console, file) in APIs //TODO: in GrpcService.

**- Unit Tests for Busines Logic services:**
- MsTest tests (BuyerSeviceTests, EmployeeSeviceTests);
- using extra NuGet packages: Moq, AutoFixture, FluentAssertions (methods versions with FluentAssertions and without FluentAssertions in AuthServiceTests).