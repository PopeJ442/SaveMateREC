using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savemate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeUserId1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
        name: "UserId1",
        table: "Accounts");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
