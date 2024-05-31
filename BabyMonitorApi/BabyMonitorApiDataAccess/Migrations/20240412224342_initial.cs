using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyMonitorApiDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "api_key",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_key", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    ApiKeyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_api_key_ApiKeyId",
                        column: x => x.ApiKeyId,
                        principalTable: "api_key",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "baby",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baby", x => x.Id);
                    table.ForeignKey(
                        name: "FK_baby_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "monitoring_devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BabyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LivestreamUrl = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    ApiKeyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitoring_devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_monitoring_devices_api_key_ApiKeyId",
                        column: x => x.ApiKeyId,
                        principalTable: "api_key",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_monitoring_devices_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_baby_UserId",
                table: "baby",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_devices_ApiKeyId",
                table: "monitoring_devices",
                column: "ApiKeyId",
                unique: true,
                filter: "[ApiKeyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_devices_UserId",
                table: "monitoring_devices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_ApiKeyId",
                table: "users",
                column: "ApiKeyId",
                unique: true,
                filter: "[ApiKeyId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "baby");

            migrationBuilder.DropTable(
                name: "monitoring_devices");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "api_key");
        }
    }
}
