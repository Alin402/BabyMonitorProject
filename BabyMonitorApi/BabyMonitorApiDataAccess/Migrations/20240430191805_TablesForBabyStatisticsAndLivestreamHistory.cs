using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyMonitorApiDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TablesForBabyStatisticsAndLivestreamHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "livestream",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BabyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonitoringDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Time = table.Column<double>(type: "float", nullable: true),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_livestream", x => x.Id);
                    table.ForeignKey(
                        name: "FK_livestream_babies_BabyId",
                        column: x => x.BabyId,
                        principalTable: "babies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "baby_state",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LivestreamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AtSecond = table.Column<double>(type: "float", nullable: false),
                    Emotion = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Awake = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baby_state", x => x.Id);
                    table.ForeignKey(
                        name: "FK_baby_state_livestream_LivestreamId",
                        column: x => x.LivestreamId,
                        principalTable: "livestream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_baby_state_LivestreamId",
                table: "baby_state",
                column: "LivestreamId");

            migrationBuilder.CreateIndex(
                name: "IX_livestream_BabyId",
                table: "livestream",
                column: "BabyId");

            migrationBuilder.CreateIndex(
                name: "IX_livestream_MonitoringDeviceId",
                table: "livestream",
                column: "MonitoringDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_livestream_UserId",
                table: "livestream",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "baby_state");

            migrationBuilder.DropTable(
                name: "livestream");
        }
    }
}
