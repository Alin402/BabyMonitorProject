using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyMonitorApiDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_livestream_MonitoringDeviceId",
                table: "livestream");

            migrationBuilder.DropIndex(
                name: "IX_livestream_UserId",
                table: "livestream");

            migrationBuilder.DropColumn(
                name: "MonitoringDeviceId",
                table: "livestream");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "livestream");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MonitoringDeviceId",
                table: "livestream",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "livestream",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_livestream_MonitoringDeviceId",
                table: "livestream",
                column: "MonitoringDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_livestream_UserId",
                table: "livestream",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_livestream_monitoring_devices_MonitoringDeviceId",
                table: "livestream",
                column: "MonitoringDeviceId",
                principalTable: "monitoring_devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_livestream_users_UserId",
                table: "livestream",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
