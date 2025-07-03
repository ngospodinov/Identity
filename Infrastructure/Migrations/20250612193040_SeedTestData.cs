using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "test@example.com", "testuser" });

            migrationBuilder.InsertData(
                table: "access_grants",
                columns: new[] { "Id", "Category", "ClientId", "GrantedAt", "InstitutionId", "UserId" },
                values: new object[] { 1, 1, "test-client", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "user_data_items",
                columns: new[] { "Id", "Category", "CreatedAt", "Key", "UserId", "Value" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GPA", new Guid("00000000-0000-0000-0000-000000000001"), "3.8" },
                    { 2, 2, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "IBAN", new Guid("00000000-0000-0000-0000-000000000001"), "BG00TEST1234567890" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "access_grants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "user_data_items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "user_data_items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
