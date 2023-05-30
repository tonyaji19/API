using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "tb_m_accounts");

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_date", "modified_date", "name" },
                values: new object[,]
                {
                    { new Guid("31b3f1ea-61d1-4fc4-4571-08db60bf2a9b"), new DateTime(2023, 5, 30, 11, 17, 7, 550, DateTimeKind.Local).AddTicks(9618), new DateTime(2023, 5, 30, 11, 17, 7, 550, DateTimeKind.Local).AddTicks(9627), "User" },
                    { new Guid("dc42eef7-cf1b-4624-4572-08db60bf2a9b"), new DateTime(2023, 5, 30, 11, 17, 7, 550, DateTimeKind.Local).AddTicks(9630), new DateTime(2023, 5, 30, 11, 17, 7, 550, DateTimeKind.Local).AddTicks(9631), "Manager" },
                    { new Guid("fd6bb2c1-9544-4c87-4573-08db60bf2a9b"), new DateTime(2023, 5, 30, 11, 17, 7, 550, DateTimeKind.Local).AddTicks(9633), new DateTime(2023, 5, 30, 11, 17, 7, 550, DateTimeKind.Local).AddTicks(9633), "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("31b3f1ea-61d1-4fc4-4571-08db60bf2a9b"));

            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("dc42eef7-cf1b-4624-4572-08db60bf2a9b"));

            migrationBuilder.DeleteData(
                table: "tb_m_roles",
                keyColumn: "guid",
                keyValue: new Guid("fd6bb2c1-9544-4c87-4573-08db60bf2a9b"));

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "tb_m_accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
