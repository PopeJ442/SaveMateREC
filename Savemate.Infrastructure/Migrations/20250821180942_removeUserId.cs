using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savemate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
    name: "UserId1",
    table: "Accounts");

        }
    }
}
