using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace directory.web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToForklifts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Forklifts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Forklifts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forklifts_DeletedAt",
                table: "Forklifts",
                column: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Forklifts_DeletedAt",
                table: "Forklifts");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Forklifts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Forklifts");
        }
    }
}
