using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyMonitorApiDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baby_users_UserId",
                table: "baby");

            migrationBuilder.DropForeignKey(
                name: "FK_monitoring_devices_api_key_ApiKeyId",
                table: "monitoring_devices");

            migrationBuilder.DropForeignKey(
                name: "FK_users_api_key_ApiKeyId",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_baby",
                table: "baby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_api_key",
                table: "api_key");

            migrationBuilder.RenameTable(
                name: "baby",
                newName: "babies");

            migrationBuilder.RenameTable(
                name: "api_key",
                newName: "api_keys");

            migrationBuilder.RenameIndex(
                name: "IX_baby_UserId",
                table: "babies",
                newName: "IX_babies_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_babies",
                table: "babies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_api_keys",
                table: "api_keys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_babies_users_UserId",
                table: "babies",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_babies_users_UserId",
                table: "babies");

            migrationBuilder.DropForeignKey(
                name: "FK_monitoring_devices_api_keys_ApiKeyId",
                table: "monitoring_devices");

            migrationBuilder.DropForeignKey(
                name: "FK_users_api_keys_ApiKeyId",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_babies",
                table: "babies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_api_keys",
                table: "api_keys");

            migrationBuilder.RenameTable(
                name: "babies",
                newName: "baby");

            migrationBuilder.RenameTable(
                name: "api_keys",
                newName: "api_key");

            migrationBuilder.RenameIndex(
                name: "IX_babies_UserId",
                table: "baby",
                newName: "IX_baby_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_baby",
                table: "baby",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_api_key",
                table: "api_key",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_baby_users_UserId",
                table: "baby",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_monitoring_devices_api_key_ApiKeyId",
                table: "monitoring_devices",
                column: "ApiKeyId",
                principalTable: "api_key",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_api_key_ApiKeyId",
                table: "users",
                column: "ApiKeyId",
                principalTable: "api_key",
                principalColumn: "Id");
        }
    }
}
