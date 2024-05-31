using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyMonitorApiDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMonitoringDeviceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaybackUrl",
                table: "monitoring_devices");

            migrationBuilder.AlterColumn<string>(
                name: "LivestreamUrl",
                table: "monitoring_devices",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");

            migrationBuilder.AddColumn<string>(
                name: "StreamId",
                table: "monitoring_devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_monitoring_devices_BabyId",
                table: "monitoring_devices",
                column: "BabyId",
                unique: true,
                filter: "[BabyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_monitoring_devices_babies_BabyId",
                table: "monitoring_devices",
                column: "BabyId",
                principalTable: "babies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_monitoring_devices_babies_BabyId",
                table: "monitoring_devices");

            migrationBuilder.DropIndex(
                name: "IX_monitoring_devices_BabyId",
                table: "monitoring_devices");

            migrationBuilder.DropColumn(
                name: "StreamId",
                table: "monitoring_devices");

            migrationBuilder.AlterColumn<string>(
                name: "LivestreamUrl",
                table: "monitoring_devices",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaybackUrl",
                table: "monitoring_devices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
