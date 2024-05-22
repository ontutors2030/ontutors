using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig014 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Course",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Course",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Course");
        }
    }
}
