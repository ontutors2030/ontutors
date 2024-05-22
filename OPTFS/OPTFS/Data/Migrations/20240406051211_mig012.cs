using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    fee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refunded = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    refunded_at = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    captured = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    captured_at = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    voided_at = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amount_format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fee_format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refunded_format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    captured_format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    invoice_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    callback_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sourceid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Payments_PSources_sourceid",
                        column: x => x.sourceid,
                        principalTable: "PSources",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_sourceid",
                table: "Payments",
                column: "sourceid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PSources");
        }
    }
}
