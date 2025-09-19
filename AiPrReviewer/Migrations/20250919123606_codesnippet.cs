using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiPrReviewer.Migrations
{
    /// <inheritdoc />
    public partial class codesnippet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeSnippet",
                table: "AuditLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeSnippet",
                table: "AuditLogs");
        }
    }
}
