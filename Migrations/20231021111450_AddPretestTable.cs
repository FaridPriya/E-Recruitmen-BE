using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERecruitmentBE.Migrations
{
    /// <inheritdoc />
    public partial class AddPretestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PretestQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PretestQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PretestQuestionItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PretestQuestionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IndexNo = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PretestQuestionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PretestQuestionItems_PretestQuestions_PretestQuestionId",
                        column: x => x.PretestQuestionId,
                        principalTable: "PretestQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PretestQuestionItems_PretestQuestionId",
                table: "PretestQuestionItems",
                column: "PretestQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PretestQuestionItems");

            migrationBuilder.DropTable(
                name: "PretestQuestions");
        }
    }
}
