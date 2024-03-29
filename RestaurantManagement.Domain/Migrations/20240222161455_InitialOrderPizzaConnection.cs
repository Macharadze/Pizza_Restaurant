using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Domain.Migrations
{
    public partial class InitialOrderPizzaConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPizzaas_Addresss_AddresId",
                table: "OrderPizzaas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPizzaas_Pizzas_PizzaId",
                table: "OrderPizzaas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPizzaas_Users_UserId",
                table: "OrderPizzaas");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderPizzaas_OrderPizzaId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderPizzaId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderPizzaId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzaas_OrderId",
                table: "OrderPizzaas",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPizzaas_Addresss_AddresId",
                table: "OrderPizzaas",
                column: "AddresId",
                principalTable: "Addresss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPizzaas_Orders_OrderId",
                table: "OrderPizzaas",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPizzaas_Pizzas_PizzaId",
                table: "OrderPizzaas",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPizzaas_Users_UserId",
                table: "OrderPizzaas",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPizzaas_Addresss_AddresId",
                table: "OrderPizzaas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPizzaas_Orders_OrderId",
                table: "OrderPizzaas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPizzaas_Pizzas_PizzaId",
                table: "OrderPizzaas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPizzaas_Users_UserId",
                table: "OrderPizzaas");

            migrationBuilder.DropIndex(
                name: "IX_OrderPizzaas_OrderId",
                table: "OrderPizzaas");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderPizzaId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderPizzaId",
                table: "Orders",
                column: "OrderPizzaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPizzaas_Addresss_AddresId",
                table: "OrderPizzaas",
                column: "AddresId",
                principalTable: "Addresss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPizzaas_Pizzas_PizzaId",
                table: "OrderPizzaas",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPizzaas_Users_UserId",
                table: "OrderPizzaas",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderPizzaas_OrderPizzaId",
                table: "Orders",
                column: "OrderPizzaId",
                principalTable: "OrderPizzaas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
