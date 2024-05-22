using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Mig006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactInfo",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Course",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromTime",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "Course",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToTime",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "FromTime",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "ToTime",
                table: "Course");

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
