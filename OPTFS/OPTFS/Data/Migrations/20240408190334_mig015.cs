using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPTFS.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig015 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRequest_PaymentMethod_PaymentMethodId",
                table: "StudentRequest");

            migrationBuilder.DropTable(
                name: "RatingDetail");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "RatingDetailType");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_StudentRequest_PaymentMethodId",
                table: "StudentRequest");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "StudentRequest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodId",
                table: "StudentRequest",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rating_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatingDetailType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingDetailType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RatingDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingDetailTypeId = table.Column<int>(type: "int", nullable: true),
                    RatingId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethodId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Val = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingDetail_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RatingDetail_RatingDetailType_RatingDetailTypeId",
                        column: x => x.RatingDetailTypeId,
                        principalTable: "RatingDetailType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RatingDetail_Rating_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Rating",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentRequest_PaymentMethodId",
                table: "StudentRequest",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_CourseId",
                table: "Rating",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_StudentId",
                table: "Rating",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingDetail_PaymentMethodId",
                table: "RatingDetail",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingDetail_RatingDetailTypeId",
                table: "RatingDetail",
                column: "RatingDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingDetail_RatingId",
                table: "RatingDetail",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRequest_PaymentMethod_PaymentMethodId",
                table: "StudentRequest",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id");
        }
    }
}
