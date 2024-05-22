using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig016 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "StudentCourses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "StudentCourses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "StudentCourses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "courseId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "requestId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalReviewes",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_RequestId",
                table: "StudentCourses",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_StudentRequest_RequestId",
                table: "StudentCourses",
                column: "RequestId",
                principalTable: "StudentRequest",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_StudentRequest_RequestId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_RequestId",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "courseId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "requestId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "TotalReviewes",
                table: "Course");
        }
    }
}
