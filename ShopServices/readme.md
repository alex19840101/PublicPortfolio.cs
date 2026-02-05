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
NotificationsSender
NotifierByEmail.API
NotifierBySms.API
Orders.API
Prices.API
Shops.API
TelegramBot.API
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
NotificationsSender\
NotifierByEmail.API\
NotifierBySms.API\
Orders.API\
Prices.API\
Shops.API\
ShopServices.Abstractions\
ShopServices.BusinessLogic\
ShopServices.BusinessLogic.MsTests\
ShopServices.Core\
ShopServices.DataAccess\
SimpleTradeWatcher\ // - Kafka consumer
TelegramBot.API\
TestFixtures\
TestResults\
Trade.API\
TradeWatcher\ // - Kafka consumer
Warehouse.API\
```

## **using stack:**
**- Platform:** .NET 9.0.

**ASP.NET Core Web API**:
- All ***.API projects.

**gRPC-services and notifiers with auth.:**
- TelegramBot.API, NotifierByEmail.API, NotifierBySms.API
- NotificationsSender as gRPC-client

**- ORM:** EF Core, + Dapper in Notifications.API.

**- DataBase**: PostgreSQL 17.

**- Caching:** Redis.
//TODO: Using for products, categories.

**- Auth. (authentication and authorization):**
- JWT-tokens + roles.
- Authorization required in methods.
Using in APIs projects, gRPC-services and NotificationsSender

**- Documentation**: Swagger (OpenAPI) in APIs.

**- Logging**: Serilog (console, file) in APIs, gRPC services and NotificationsSender.

**- Unit Tests for Busines Logic services:**
- MsTest tests (BuyerSeviceTests, EmployeeSeviceTests);
- using extra NuGet packages: Moq, AutoFixture, FluentAssertions (methods versions with FluentAssertions and without FluentAssertions in AuthServiceTests).

**- Event sourcing, Event Bus:**
- MassTransit, RabbitMQ. //Using in Buyers.API, Employees.API, Orders.API, Notifications.API
//RabbitMQ run in docker: docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4.1.2-management
- Kafka (Confluent.Kafka 2.13.0 in Trade.API (producer), SimpleTradeWatcher (consumer), TradeWatcher (consumer))

**- Notifications:**
- Telegram (using Telegram.Bot (v22.8.1) in TelegramBot.API)
- E-mail notifications (using MailKit)
- SMS-notifications (SmsNotificationsService - notifier abstract simulation and SmsNotificationsByAzureService using Azure.Communication.Sms)