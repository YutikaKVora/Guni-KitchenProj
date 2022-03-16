using Microsoft.EntityFrameworkCore.Migrations;

namespace Guni_Kitchen.Migrations
{
    public partial class Products1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImageContentType",
                table: "Products",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImageFileUrl",
                table: "Products",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImageContentType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImageFileUrl",
                table: "Products");
        }
    }
}
