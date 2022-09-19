using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    public partial class Last : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_AspNetUsers_userId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_userId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "user",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Client",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_userId",
                table: "Client",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_AspNetUsers_userId",
                table: "Client",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
