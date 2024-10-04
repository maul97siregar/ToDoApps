using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTokenDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "LoginHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "LoginHistories");
        }
    }
}
