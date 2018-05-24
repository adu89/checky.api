using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Checky.api.Migrations
{
    public partial class MIG8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRoles_UserRoleId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationItems_OrganizationId",
                table: "OrganizationItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "OrderItems"
            );

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "OrderItems",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                table: "InventoryItems"
            );

            migrationBuilder.AddColumn<int>(
                name: "InventoryItemId",
                table: "InventoryItems",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "UserRoleId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_OrganizationItems_OrganizationId_ItemId",
                table: "OrganizationItems",
                columns: new[] { "OrganizationId", "ItemId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "OrderItemId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_OrderItems_OrderId_ItemId",
                table: "OrderItems",
                columns: new[] { "OrderId", "ItemId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems",
                column: "InventoryItemId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_InventoryItems_InventoryId_ItemId",
                table: "InventoryItems",
                columns: new[] { "InventoryId", "ItemId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRoles_UserId_RoleId",
                table: "UserRoles");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_OrganizationItems_OrganizationId_ItemId",
                table: "OrganizationItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_OrderItems_OrderId_ItemId",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_InventoryItems_InventoryId_ItemId",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<int>(
                name: "OrderItemId",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "InventoryItemId",
                table: "InventoryItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRoles_UserRoleId",
                table: "UserRoles",
                column: "UserRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                columns: new[] { "OrderId", "ItemId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems",
                columns: new[] { "InventoryId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationItems_OrganizationId",
                table: "OrganizationItems",
                column: "OrganizationId");
        }
    }
}
