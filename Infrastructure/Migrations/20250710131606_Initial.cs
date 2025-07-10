using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "institutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "access_grants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    RequestedItemId = table.Column<int>(type: "integer", nullable: true),
                    GrantedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_grants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_access_grants_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_access_grants_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_data_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_data_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_data_items_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "access_requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedCategory = table.Column<int>(type: "integer", nullable: true),
                    RequestedItemId = table.Column<int>(type: "integer", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_access_requests_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_access_requests_user_data_items_RequestedItemId",
                        column: x => x.RequestedItemId,
                        principalTable: "user_data_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_access_requests_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "institutions",
                columns: new[] { "Id", "ClientId", "ContactEmail", "Name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "test-client", "test@example.com", "Test Institution" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "test@example.com", "testuser" });

            migrationBuilder.InsertData(
                table: "access_grants",
                columns: new[] { "Id", "Category", "GrantedAt", "InstitutionId", "RequestedItemId", "RevokedAt", "UserId" },
                values: new object[] { 1, 1, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000002"), null, null, new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "user_data_items",
                columns: new[] { "Id", "Category", "CreatedAt", "DeletedAt", "Key", "UserId", "Value" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "GPA", new Guid("00000000-0000-0000-0000-000000000001"), "3.8" },
                    { 2, 2, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "IBAN", new Guid("00000000-0000-0000-0000-000000000001"), "BG00TEST1234567890" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_grants_InstitutionId",
                table: "access_grants",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_access_grants_UserId_InstitutionId_Category_RequestedItemId~",
                table: "access_grants",
                columns: new[] { "UserId", "InstitutionId", "Category", "RequestedItemId", "RevokedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_access_requests_InstitutionId",
                table: "access_requests",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_access_requests_RequestedItemId",
                table: "access_requests",
                column: "RequestedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_access_requests_UserId",
                table: "access_requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_institutions_ClientId",
                table: "institutions",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_data_items_UserId",
                table: "user_data_items",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_grants");

            migrationBuilder.DropTable(
                name: "access_requests");

            migrationBuilder.DropTable(
                name: "institutions");

            migrationBuilder.DropTable(
                name: "user_data_items");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
