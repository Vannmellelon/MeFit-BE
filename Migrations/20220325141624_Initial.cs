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
                    Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    PostalPlace = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FitnessLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RestrictedCategories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuthId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsContributor = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContributorRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestingUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributorRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContributorRequests_User_RequestingUserId",
                        column: x => x.RequestingUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ContributorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercise_User_ContributorId",
                        column: x => x.ContributorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Difficulty = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ContributorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workout_User_ContributorId",
                        column: x => x.ContributorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Difficulty = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ContributorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutProgram_User_ContributorId",
                        column: x => x.ContributorId,
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
                    WorkoutId = table.Column<int>(type: "int", nullable: true),
                    ExerciseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Set_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Set_Workout_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workout",
                        principalColumn: "Id");
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
                    WorkoutProgramId = table.Column<int>(type: "int", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
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
                    WorkoutId = table.Column<int>(type: "int", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
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
                table: "User",
                columns: new[] { "Id", "AuthId", "Email", "FirstName", "FitnessLevel", "IsAdmin", "IsContributor", "LastName", "RestrictedCategories" },
                values: new object[,]
                {
                    { 1, null, "kari.nordmann@gmail.com", "Kari", null, true, true, "Nordmann", null },
                    { 2, null, "ola.hansen@gmail.com", "Ola", null, false, true, "Hansen", null },
                    { 3, null, "else.berg@gmail.com", "Else", null, false, false, "Berg", null },
                    { 9, null, "anneelarsen98@gmail.com", "Anne E.", null, false, true, "Larsen", null }
                });

            migrationBuilder.InsertData(
                table: "Exercise",
                columns: new[] { "Id", "Category", "ContributorId", "Description", "Image", "Name", "Video" },
                values: new object[,]
                {
                    { 1, "Core", 1, "Lay on your back with your hands behind your head, and move your upper body up and down.", "https://us.123rf.com/450wm/lioputra/lioputra2011/lioputra201100006/158485483-man-doing-sit-ups-exercise-abdominals-exercise-flat-vector-illustration-isolated-on-white-background.jpg?ver=6", "Crunch", null },
                    { 2, "Arms", 1, "Hands on the floor. Straighten out your body and lift yourself down to the floor and back up by bending you arms.", null, "Push-up", "https://youtu.be/uCNgB_rU3IQ?t=5" },
                    { 9, "Arms", 9, "Grab onto the bar and hang with your arms fully extended. Pull yourself up, with controll, untill your chin is above the bar. Try to keep the rest of your body still, be mindfull not to bend your hips or knees. Slowly lower yourself down into the starting position, with controll. Repeat.", "https://evofitness.no/wp-content/uploads/2019/12/pullupfront.png__666x666_q85_subsampling-2.jpeg", "Pull Up", null },
                    { 11, "Arms", 9, "Grasp the two bars. Extend your arms so that they support your full weight, your legs should be hanging. Lower your body down by bending your elbows. Throughout the exercise, your elbows should be in line with your wrists. Your shoulders should be almost parallell with your elbows before pushing your body up again. Repeat.", "https://upload.wikimedia.org/wikipedia/commons/thumb/5/59/Dipexercise.svg/300px-Dipexercise.svg.png", "Dips", null },
                    { 8, "Legs", 9, "Stand with your feet shoulder-width apart. Grasp the bar with your hands just outside your legs. Lift the bar by driving your hips forward, keeping a flat back. Lower the bar with controll. Repeat.", "https://cdn.mos.cms.futurecdn.net/pcDfKtAmMLgLLbXc8sSAkF-970-80.jpg.webp", "Deadlift", "https://youtu.be/ABga0-lEY58?t=5" },
                    { 10, "Arms", 9, "Lie down on your back on the bench. Your feet should rest flat on the ground. Grasp the bar, positioning your hands slightly wider than your shoulders. Lift the bar and hold it over your chest. Slowly lower the bar towards your chest. Push the bar away from your chest, until your arms are fully extended. Repeat", "https://image.shutterstock.com/image-illustration/closegrip-barbell-bench-press-3d-260nw-430936051.jpg", "Bench Press", null },
                    { 6, "Arms", 9, "Adjust seat and weights to an approperiate level. Grab handles, your elbows should point to the floor. Lift the handles by extending your elbows all the way. Make sure your lower back remains in contact with the backrest throughout. Lower arms with controll. Repeat.", null, "Seated Shoulder-press Machine", "https://youtu.be/OD5pz7-703U" },
                    { 3, "Full body", 2, "Lay down on the floor. Then lift and hold yourself up on your elbows and toes. Hold and breath.", null, "Plank", "https://youtu.be/HW4yjoCkbm0?t=5" },
                    { 4, "Stamina", 2, "Jump up and down while opening and closing your legs and lifting your arms over your head.", null, "Jumping Jacks", null },
                    { 7, "Arms", 9, "Row, row, row your boat.", null, "Rowing Machine", "https://youtu.be/g2Q-etHs9LI?t=4" },
                    { 5, "Arms", 9, "Adjust seat and weights to an approperiate level. Grab handles, your elbows should be parallell to the floor. Push handles away from your chest by extending your elbows all the way. Make sure your back remains in contact with the backrest throughout. Pull your arms back towards you with controll. Repeat.", null, "Chest-press Machine", "https://youtu.be/IbeA5ypeMns?t=5" }
                });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "Id", "AddressId", "Disabilities", "Height", "MedicalConditions", "UserId", "Weight" },
                values: new object[,]
                {
                    { 1, 1, null, 170, null, 1, 89 },
                    { 3, 3, "Wheelchair-bound", 164, null, 3, 78 },
                    { 2, 2, null, 145, null, 2, 150 }
                });

            migrationBuilder.InsertData(
                table: "Workout",
                columns: new[] { "Id", "Category", "ContributorId", "Difficulty", "Name" },
                values: new object[,]
                {
                    { 3, "Full body", 2, "Intermediate", "Fitness" },
                    { 2, "Stamina", 1, "Expert", "Stamina Builder" },
                    { 1, "Core", 1, "Beginner", "Strengthify" },
                    { 4, "Arms", 9, "Beginner", "Machine Trio" },
                    { 5, "Full body", 9, "Intermediate", "The Compound Collection 1" },
                    { 6, "Arms", 9, "Intermediate", "The Compound Collection 2" }
                });

            migrationBuilder.InsertData(
                table: "WorkoutProgram",
                columns: new[] { "Id", "Category", "ContributorId", "Difficulty", "Name" },
                values: new object[,]
                {
                    { 3, "Stamina", 2, "Expert", "The Runner" },
                    { 2, "Flexibility", 1, "Intermediate", "The Wellness Yourney" },
                    { 1, "Full body", 1, "Beginner", "Hot and Heavy" },
                    { 4, "Full body", 9, "Beginner", "Nice and Easy" },
                    { 5, "Full body", 9, "Intermediate", "The Compound Collection" }
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
                table: "Set",
                columns: new[] { "Id", "ExerciseId", "ExerciseRepetitions", "WorkoutId" },
                values: new object[,]
                {
                    { 1, 1, 20, 1 },
                    { 2, 2, 10, 2 },
                    { 4, 4, 30, 2 },
                    { 3, 3, 1, 3 },
                    { 5, 5, 15, 4 },
                    { 6, 6, 15, 4 },
                    { 7, 7, 15, 4 },
                    { 8, 8, 12, 5 },
                    { 9, 9, 12, 5 },
                    { 10, 10, 12, 6 },
                    { 11, 11, 12, 6 }
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
                name: "IX_ContributorRequests_RequestingUserId",
                table: "ContributorRequests",
                column: "RequestingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_ContributorId",
                table: "Exercise",
                column: "ContributorId");

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
                name: "IX_Workout_ContributorId",
                table: "Workout",
                column: "ContributorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutProgram_ContributorId",
                table: "WorkoutProgram",
                column: "ContributorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutWorkoutProgram_WorkoutsId",
                table: "WorkoutWorkoutProgram",
                column: "WorkoutsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContributorRequests");

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
