using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Notification",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "StudentRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StudentRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRequest_CourseId",
                table: "StudentRequest",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRequest_Course_CourseId",
                table: "StudentRequest",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRequest_Course_CourseId",
                table: "StudentRequest");

            migrationBuilder.DropIndex(
                name: "IX_StudentRequest_CourseId",
                table: "StudentRequest");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "StudentRequest");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StudentRequest");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Notification",
                newName: "Created");
        }
    }
}
