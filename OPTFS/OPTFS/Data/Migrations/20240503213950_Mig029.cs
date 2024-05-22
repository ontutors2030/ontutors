using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Mig029 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentSources_PaymentSourceId",
                table: "Payments");*/

            //migrationBuilder.DropTable(name: "PaymentSources");

            migrationBuilder.RenameColumn(
                name: "PaymentSourceId",
                table: "Payments",
                newName: "PSourceId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_PaymentSourceId",
                table: "Payments",
                newName: "IX_Payments_PSourceId");

            migrationBuilder.CreateTable(
                name: "PSources",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gateway_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    response_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    authorization_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer_country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer_card_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer_card_category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PSources", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PSources_PSourceId",
                table: "Payments",
                column: "PSourceId",
                principalTable: "PSources",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PSources_PSourceId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "PSources");

            migrationBuilder.RenameColumn(
                name: "PSourceId",
                table: "Payments",
                newName: "PaymentSourceId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_PSourceId",
                table: "Payments",
                newName: "IX_Payments_PaymentSourceId");

            migrationBuilder.CreateTable(
                name: "PaymentSources",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    authorization_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gateway_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer_card_category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer_card_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer_country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issuer_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    response_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSources", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentSources_PaymentSourceId",
                table: "Payments",
                column: "PaymentSourceId",
                principalTable: "PaymentSources",
                principalColumn: "id");
        }
    }
}
