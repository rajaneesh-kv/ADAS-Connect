using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CvTracker.Web.Migrations
{
    public partial class AddSchoolRegistrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CVPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InterviewCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    FatherProfession = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    FatherMobile = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FatherEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FatherLandline = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MotherProfession = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MotherMobile = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MotherEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MotherLandline = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AreaOfLiving = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolRegistrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CandidateStatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateStatusHistories_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateStatusHistories_CandidateId",
                table: "CandidateStatusHistories",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateStatusHistories");

            migrationBuilder.DropTable(
                name: "SchoolRegistrations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Candidates");
        }
    }
}
