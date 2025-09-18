using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiPrReviewer.Migrations
{
    /// <inheritdoc />
    public partial class prTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrTitle",
                table: "AuditLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrTitle",
                table: "AuditLogs");
        }
    }
}
