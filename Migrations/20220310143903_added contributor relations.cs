using Microsoft.EntityFrameworkCore.Migrations;

namespace MeFit_BE.Migrations
{
    public partial class addedcontributorrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContributedById",
                table: "Workout",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContributorId",
                table: "Workout",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContributedById",
                table: "Program",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContributorId",
                table: "Program",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContributedById",
                table: "Exercise",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContributorId",
                table: "Exercise",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Workout_ContributedById",
                table: "Workout",
                column: "ContributedById");

            migrationBuilder.CreateIndex(
                name: "IX_Program_ContributedById",
                table: "Program",
                column: "ContributedById");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_ContributedById",
                table: "Exercise",
                column: "ContributedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_User_ContributedById",
                table: "Exercise",
                column: "ContributedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Program_User_ContributedById",
                table: "Program",
                column: "ContributedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workout_User_ContributedById",
                table: "Workout",
                column: "ContributedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_User_ContributedById",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_Program_User_ContributedById",
                table: "Program");

            migrationBuilder.DropForeignKey(
                name: "FK_Workout_User_ContributedById",
                table: "Workout");

            migrationBuilder.DropIndex(
                name: "IX_Workout_ContributedById",
                table: "Workout");

            migrationBuilder.DropIndex(
                name: "IX_Program_ContributedById",
                table: "Program");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_ContributedById",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "ContributedById",
                table: "Workout");

            migrationBuilder.DropColumn(
                name: "ContributorId",
                table: "Workout");

            migrationBuilder.DropColumn(
                name: "ContributedById",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "ContributorId",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "ContributedById",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "ContributorId",
                table: "Exercise");
        }
    }
}
