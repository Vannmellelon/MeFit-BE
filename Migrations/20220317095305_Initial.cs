using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeFit_BE.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    PostalPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsContributor = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetMuscleGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContributorId = table.Column<int>(type: "int", nullable: false),
                    ContributedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercise_User_ContributedById",
                        column: x => x.ContributedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    MedicalConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disabilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workout",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContributorId = table.Column<int>(type: "int", nullable: false),
                    ContributedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workout_User_ContributedById",
                        column: x => x.ContributedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContributorId = table.Column<int>(type: "int", nullable: false),
                    ContributedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutProgram_User_ContributedById",
                        column: x => x.ContributedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Set",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseRepetitions = table.Column<int>(type: "int", nullable: false),
                    WorkoutId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Set_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Set_Workout_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Goal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Achieved = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkoutProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goal_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Goal_WorkoutProgram_WorkoutProgramId",
                        column: x => x.WorkoutProgramId,
                        principalTable: "WorkoutProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutWorkoutProgram",
                columns: table => new
                {
                    WorkoutProgramsId = table.Column<int>(type: "int", nullable: false),
                    WorkoutsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutWorkoutProgram", x => new { x.WorkoutProgramsId, x.WorkoutsId });
                    table.ForeignKey(
                        name: "FK_WorkoutWorkoutProgram_Workout_WorkoutsId",
                        column: x => x.WorkoutsId,
                        principalTable: "Workout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutWorkoutProgram_WorkoutProgram_WorkoutProgramsId",
                        column: x => x.WorkoutProgramsId,
                        principalTable: "WorkoutProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubGoal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Achieved = table.Column<bool>(type: "bit", nullable: false),
                    GoalId = table.Column<int>(type: "int", nullable: false),
                    WorkoutId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGoal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubGoal_Goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubGoal_Workout_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "Country", "PostalCode", "PostalPlace", "Street" },
                values: new object[,]
                {
                    { 1, "Norway", 2849, "Oslo", "Karl Johans gate" },
                    { 2, "Norway", 9376, "Bergen", "Lilleveien" },
                    { 3, "Norway", 3689, "Kautokeino", "Storeveien" }
                });

            migrationBuilder.InsertData(
                table: "Exercise",
                columns: new[] { "Id", "ContributedById", "ContributorId", "Description", "Image", "Name", "TargetMuscleGroup", "Video" },
                values: new object[,]
                {
                    { 1, null, 0, "Lay on your back with your hands behind your head, and move your upper body up and down.", null, "Crunch", "Stomach", null },
                    { 2, null, 0, "Hands on the floor. Straighten out your body and lift yourself down to the floor and back up by bending you arms.", null, "Push-up", "Arms", null },
                    { 3, null, 0, "Lay down on the floor. Then lift and hold yourself up on your elbows and toes. Hold and breath.", null, "Plank", "All", null },
                    { 4, null, 0, "Jump up and down while opening and closing your legs and lifting your arms over your head.", null, "Jumping Jacks", "Stamina", null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "FirstName", "IsAdmin", "IsContributor", "LastName" },
                values: new object[,]
                {
                    { 1, "kari.nordmann@gmail.com", "Kari", true, true, "Nordmann" },
                    { 2, "ola.hansen@gmail.com", "Ola", false, true, "Hansen" },
                    { 3, "else.berg@gmail.com", "Else", false, false, "Berg" }
                });

            migrationBuilder.InsertData(
                table: "Workout",
                columns: new[] { "Id", "ContributedById", "ContributorId", "Name", "Type" },
                values: new object[,]
                {
                    { 1, null, 0, "Strengthify", "Strength" },
                    { 2, null, 0, "Stamina Builder", "Stamina" },
                    { 3, null, 0, "Fitness", "Fitness" }
                });

            migrationBuilder.InsertData(
                table: "WorkoutProgram",
                columns: new[] { "Id", "Category", "ContributedById", "ContributorId", "Name" },
                values: new object[,]
                {
                    { 1, "Upper-body Strength", null, 0, "Hot and Heavy" },
                    { 2, "Fitness", null, 0, "The Wellness Yourney" },
                    { 3, "Stamina", null, 0, "The Runner" }
                });

            migrationBuilder.InsertData(
                table: "Goal",
                columns: new[] { "Id", "Achieved", "EndData", "UserId", "WorkoutProgramId" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2022, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, true, new DateTime(2022, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "Id", "AddressId", "Disabilities", "Height", "MedicalConditions", "UserId", "Weight" },
                values: new object[,]
                {
                    { 1, 1, null, 170, null, 1, 89 },
                    { 2, 2, null, 145, null, 2, 150 },
                    { 3, 3, "Wheelchair-bound", 164, null, 3, 78 }
                });

            migrationBuilder.InsertData(
                table: "Set",
                columns: new[] { "Id", "ExerciseId", "ExerciseRepetitions", "WorkoutId" },
                values: new object[,]
                {
                    { 1, 1, 20, 1 },
                    { 2, 2, 10, 2 },
                    { 4, 4, 30, 2 },
                    { 3, 3, 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "SubGoal",
                columns: new[] { "Id", "Achieved", "GoalId", "WorkoutId" },
                values: new object[,]
                {
                    { 1, true, 1, 1 },
                    { 2, false, 2, 2 },
                    { 4, false, 2, 1 },
                    { 3, true, 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_ContributedById",
                table: "Exercise",
                column: "ContributedById");

            migrationBuilder.CreateIndex(
                name: "IX_Goal_UserId",
                table: "Goal",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Goal_WorkoutProgramId",
                table: "Goal",
                column: "WorkoutProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AddressId",
                table: "Profile",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_UserId",
                table: "Profile",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Set_ExerciseId",
                table: "Set",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Set_WorkoutId",
                table: "Set",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_SubGoal_GoalId",
                table: "SubGoal",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_SubGoal_WorkoutId",
                table: "SubGoal",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Workout_ContributedById",
                table: "Workout",
                column: "ContributedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutProgram_ContributedById",
                table: "WorkoutProgram",
                column: "ContributedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutWorkoutProgram_WorkoutsId",
                table: "WorkoutWorkoutProgram",
                column: "WorkoutsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "Set");

            migrationBuilder.DropTable(
                name: "SubGoal");

            migrationBuilder.DropTable(
                name: "WorkoutWorkoutProgram");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "Goal");

            migrationBuilder.DropTable(
                name: "Workout");

            migrationBuilder.DropTable(
                name: "WorkoutProgram");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
