using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig019 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Review",
                table: "StudentCourses",
                newName: "ReviewText");

            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "StudentCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewDate",
                table: "StudentCourses",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "StudentCourses");

            migrationBuilder.RenameColumn(
                name: "ReviewText",
                table: "StudentCourses",
                newName: "Review");
        }
    }
}
