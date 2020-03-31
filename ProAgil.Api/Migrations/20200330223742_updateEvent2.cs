using Microsoft.EntityFrameworkCore.Migrations;

namespace ProAgil.Api.Migrations
{
    public partial class updateEvent2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imagemUrl",
                table: "Eventos",
                newName: "ImagemUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagemUrl",
                table: "Eventos",
                newName: "imagemUrl");
        }
    }
}
