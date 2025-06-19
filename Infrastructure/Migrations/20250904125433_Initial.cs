using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    DataOwnerUserId = table.Column<string>(type: "text", nullable: false),
                    RequesterUserId = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    RequestedItemId = table.Column<int>(type: "integer", nullable: true),
                    GrantedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_grants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_access_grants_users_DataOwnerUserId",
                        column: x => x.DataOwnerUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_access_grants_users_RequesterUserId",
                        column: x => x.RequesterUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "names",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    LastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsDefaultForCategory = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_names", x => x.Id);
                    table.ForeignKey(
                        name: "FK_names_users_UserId",
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
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
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
                    DataOwnerUserId = table.Column<string>(type: "text", nullable: false),
                    RequesterUserId = table.Column<string>(type: "text", nullable: false),
                    RequestedCategory = table.Column<int>(type: "integer", nullable: true),
                    RequestedItemId = table.Column<int>(type: "integer", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_access_requests_user_data_items_RequestedItemId",
                        column: x => x.RequestedItemId,
                        principalTable: "user_data_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_access_requests_users_DataOwnerUserId",
                        column: x => x.DataOwnerUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_access_requests_users_RequesterUserId",
                        column: x => x.RequesterUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "specific_revocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessGrantId = table.Column<int>(type: "integer", nullable: false),
                    UserDataItemId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specific_revocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_specific_revocations_access_grants_AccessGrantId",
                        column: x => x.AccessGrantId,
                        principalTable: "access_grants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_specific_revocations_user_data_items_UserDataItemId",
                        column: x => x.UserDataItemId,
                        principalTable: "user_data_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_grants_DataOwnerUserId_RequesterUserId_Category_Requ~",
                table: "access_grants",
                columns: new[] { "DataOwnerUserId", "RequesterUserId", "Category", "RequestedItemId", "RevokedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_access_grants_RequesterUserId",
                table: "access_grants",
                column: "RequesterUserId");

            migrationBuilder.CreateIndex(
                name: "ix_access_requests_owner_id",
                table: "access_requests",
                column: "DataOwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_access_requests_RequestedItemId",
                table: "access_requests",
                column: "RequestedItemId");

            migrationBuilder.CreateIndex(
                name: "ix_access_requests_requester_id",
                table: "access_requests",
                column: "RequesterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_names_UserId_Category",
                table: "names",
                columns: new[] { "UserId", "Category" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_names_UserId_Category_IsDefaultForCategory",
                table: "names",
                columns: new[] { "UserId", "Category", "IsDefaultForCategory" },
                unique: true,
                filter: "\"IsDeleted\" = false AND \"IsDefaultForCategory\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_specific_revocations_AccessGrantId_UserDataItemId",
                table: "specific_revocations",
                columns: new[] { "AccessGrantId", "UserDataItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_specific_revocations_UserDataItemId",
                table: "specific_revocations",
                column: "UserDataItemId");

            migrationBuilder.CreateIndex(
                name: "IX_user_data_items_UserId",
                table: "user_data_items",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_requests");

            migrationBuilder.DropTable(
                name: "names");

            migrationBuilder.DropTable(
                name: "ProcessedEvents");

            migrationBuilder.DropTable(
                name: "specific_revocations");

            migrationBuilder.DropTable(
                name: "access_grants");

            migrationBuilder.DropTable(
                name: "user_data_items");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
