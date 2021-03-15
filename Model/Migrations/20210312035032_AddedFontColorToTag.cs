using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectTracker.Model.Migrations
{
    public partial class AddedFontColorToTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFontBlack",
                table: "Tags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFontBlack",
                table: "Tags");
        }
    }
}
