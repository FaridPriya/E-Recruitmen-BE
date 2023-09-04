using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERecruitmentBE.Migrations
{
    /// <inheritdoc />
    public partial class AddTableCandidateSpecification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AIScreeningStatus",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicantSpecApprove",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CandidateSpecifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CandidateId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicantItemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicantItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateSpecifications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateSpecifications");

            migrationBuilder.DropColumn(
                name: "AIScreeningStatus",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ApplicantSpecApprove",
                table: "Candidates");
        }
    }
}
