using Microsoft.EntityFrameworkCore.Migrations;

namespace Unidash.TimeTable.Application.Migrations
{
    public partial class AddIsHiddenProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "CalendarEntries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "CalendarEntries");
        }
    }
}
