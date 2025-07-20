using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShopServices.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeliveriesOrderIdAndTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Couriers_Employees_Id",
                table: "Couriers");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Buyers_BuyerId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Employees_Id",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositions_Orders_OrderId",
                table: "OrderPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Orders_OrdersId",
                table: "OrderProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Products_ProductsId",
                table: "OrderProduct");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryId1",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderPositions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Deliveries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Deliveries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryId1",
                table: "Orders",
                column: "DeliveryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Couriers_Employees_Id",
                table: "Couriers",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Buyers_BuyerId",
                table: "Deliveries",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Orders_OrderId",
                table: "Deliveries",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Employees_Id",
                table: "Managers",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositions_Orders_OrderId",
                table: "OrderPositions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Orders_OrdersId",
                table: "OrderProduct",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Products_ProductsId",
                table: "OrderProduct",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Deliveries_DeliveryId1",
                table: "Orders",
                column: "DeliveryId1",
                principalTable: "Deliveries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Couriers_Employees_Id",
                table: "Couriers");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Buyers_BuyerId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Orders_OrderId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Employees_Id",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositions_Orders_OrderId",
                table: "OrderPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Orders_OrdersId",
                table: "OrderProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Products_ProductsId",
                table: "OrderProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Deliveries_DeliveryId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_OrderId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryId1",
                table: "Orders");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Orders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "OrderPositions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "OrderId",
                table: "Deliveries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Deliveries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Couriers_Employees_Id",
                table: "Couriers",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Buyers_BuyerId",
                table: "Deliveries",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Employees_Id",
                table: "Managers",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Products_ProductsId",
                table: "OrderProduct",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
