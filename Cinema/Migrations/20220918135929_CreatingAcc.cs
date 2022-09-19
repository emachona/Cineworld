using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    public partial class CreatingAcc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Client",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_ID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "user_ID",
                table: "AspNetUsers");

        }
    }
}
