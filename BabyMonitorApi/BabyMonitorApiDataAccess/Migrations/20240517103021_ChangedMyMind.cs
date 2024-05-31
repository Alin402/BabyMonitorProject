using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyMonitorApiDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMyMind : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "livestream");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "livestream",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "livestream");

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "livestream",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
