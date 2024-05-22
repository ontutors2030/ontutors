using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRequest_PaymentMethod_PaymentMethodId1",
                table: "StudentRequest");

            migrationBuilder.DropIndex(
                name: "IX_StudentRequest_PaymentMethodId1",
                table: "StudentRequest");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId1",
                table: "StudentRequest");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethodId",
                table: "StudentRequest",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTutorConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRequest_PaymentMethodId",
                table: "StudentRequest",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRequest_PaymentMethod_PaymentMethodId",
                table: "StudentRequest",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRequest_PaymentMethod_PaymentMethodId",
                table: "StudentRequest");

            migrationBuilder.DropIndex(
                name: "IX_StudentRequest_PaymentMethodId",
                table: "StudentRequest");

            migrationBuilder.DropColumn(
                name: "IsTutorConfirmed",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                table: "StudentRequest",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodId1",
                table: "StudentRequest",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRequest_PaymentMethodId1",
                table: "StudentRequest",
                column: "PaymentMethodId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRequest_PaymentMethod_PaymentMethodId1",
                table: "StudentRequest",
                column: "PaymentMethodId1",
                principalTable: "PaymentMethod",
                principalColumn: "Id");
        }
    }
}
