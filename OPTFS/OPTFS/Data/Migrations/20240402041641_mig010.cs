using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Fri",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Mon",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sat",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sun",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Thi",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Tue",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Wen",
                table: "Course",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fri",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Mon",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Sat",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Sun",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Thi",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Tue",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Wen",
                table: "Course");
        }
    }
}
