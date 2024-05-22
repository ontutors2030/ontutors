using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "ChatMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chat_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatId",
                table: "ChatMessages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_User1Id",
                table: "Chat",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_User2Id",
                table: "Chat",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Chat_ChatId",
                table: "ChatMessages",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Chat_ChatId",
                table: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ChatId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "ChatMessages");
        }
    }
}
