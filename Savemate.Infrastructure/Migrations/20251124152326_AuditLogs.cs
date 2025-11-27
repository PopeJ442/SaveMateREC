using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savemate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReversedDate",
                table: "Transactions");

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    AuditType = table.Column<int>(type: "int", nullable: false),
                    OldAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldFromAccountId = table.Column<int>(type: "int", nullable: true),
                    NewFromAccountId = table.Column<int>(type: "int", nullable: true),
                    OldToAccountId = table.Column<int>(type: "int", nullable: true),
                    NewToAccountId = table.Column<int>(type: "int", nullable: true),
                    OldDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLog_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TransactionId",
                table: "AuditLog",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReversedDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);
        }
    }
}
