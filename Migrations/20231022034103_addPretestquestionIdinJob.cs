using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERecruitmentBE.Migrations
{
    /// <inheritdoc />
    public partial class addPretestquestionIdinJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PretestQuestionId",
                table: "JobVacancys",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PretestQuestionId",
                table: "JobVacancys");
        }
    }
}
