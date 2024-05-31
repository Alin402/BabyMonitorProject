using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyMonitorApiDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedApiKeyRelationshipFromUsersAndMonitoringDevices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_monitoring_devices_api_keys_ApiKeyId",
                table: "monitoring_devices");

            migrationBuilder.DropForeignKey(
                name: "FK_users_api_keys_ApiKeyId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_ApiKeyId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_monitoring_devices_ApiKeyId",
                table: "monitoring_devices");

            migrationBuilder.DropColumn(
                name: "ApiKeyId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ApiKeyId",
                table: "monitoring_devices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApiKeyId",
                table: "users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApiKeyId",
                table: "monitoring_devices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_ApiKeyId",
                table: "users",
                column: "ApiKeyId",
                unique: true,
                filter: "[ApiKeyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_devices_ApiKeyId",
                table: "monitoring_devices",
                column: "ApiKeyId",
                unique: true,
                filter: "[ApiKeyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_monitoring_devices_api_keys_ApiKeyId",
                table: "monitoring_devices",
                column: "ApiKeyId",
                principalTable: "api_keys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_api_keys_ApiKeyId",
                table: "users",
                column: "ApiKeyId",
                principalTable: "api_keys",
                principalColumn: "Id");
        }
    }
}
