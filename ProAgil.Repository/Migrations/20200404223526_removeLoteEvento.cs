using Microsoft.EntityFrameworkCore.Migrations;

namespace ProAgil.Repository.Migrations
{
    public partial class removeLoteEvento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lote",
                table: "Eventos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lote",
                table: "Eventos",
                nullable: true);
        }
    }
}
