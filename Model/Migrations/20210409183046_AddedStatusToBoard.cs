using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectTracker.Model.Migrations
{
    public partial class AddedStatusToBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Boards",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Boards");
        }
    }
}
