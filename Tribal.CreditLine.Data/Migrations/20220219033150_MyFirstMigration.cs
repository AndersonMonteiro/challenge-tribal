using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tribal.CreditLine.Data.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditLineRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FoundingType = table.Column<int>(type: "integer", nullable: false),
                    CashBalance = table.Column<double>(type: "double precision", nullable: false),
                    MonthlyRevenue = table.Column<double>(type: "double precision", nullable: false),
                    RequestedCreditLine = table.Column<double>(type: "double precision", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditLineRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditLineRequests");
        }
    }
}
