using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutores.Migrations
{
    public partial class ComentarioUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Comnetarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comnetarios_UsuarioId",
                table: "Comnetarios",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comnetarios_AspNetUsers_UsuarioId",
                table: "Comnetarios",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comnetarios_AspNetUsers_UsuarioId",
                table: "Comnetarios");

            migrationBuilder.DropIndex(
                name: "IX_Comnetarios_UsuarioId",
                table: "Comnetarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Comnetarios");
        }
    }
}
