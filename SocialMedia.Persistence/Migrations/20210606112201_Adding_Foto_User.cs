using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMedia.Persistence.Migrations
{
    public partial class Adding_Foto_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoPerfil",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "AspNetUsers");
        }
    }
}
