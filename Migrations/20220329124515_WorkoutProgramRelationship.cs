using MeFit_BE.Models;
using MeFit_BE.Models.Domain.WorkoutDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace MeFit_BE.Migrations
{
    public partial class WorkoutProgramRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (MeFitDbContext context = new MeFitDbContext())
            {
                WorkoutProgram workoutProgram1 = context.WorkoutPrograms
                    .Include(wp => wp.Workouts).FirstOrDefault(wp => wp.Id == 1);

                WorkoutProgram workoutProgram2 = context.WorkoutPrograms
                    .Include(wp => wp.Workouts).FirstOrDefault(w => w.Id == 2);

                WorkoutProgram workoutProgram3 = context.WorkoutPrograms
                    .Include(wp => wp.Workouts).FirstOrDefault(wp => wp.Id == 3);

                Workout workout1 = context.Workouts.Find(1);
                Workout workout2 = context.Workouts.Find(2);
                Workout workout3 = context.Workouts.Find(3);

                workoutProgram1.Workouts.Add(workout1);
                workoutProgram2.Workouts.Add(workout2);
                workoutProgram3.Workouts.Add(workout3);

                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (MeFitDbContext context = new MeFitDbContext())
            {
                WorkoutProgram workoutProgram1 = context.WorkoutPrograms
                    .Include(wp => wp.Workouts).FirstOrDefault(wp => wp.Id == 1);

                WorkoutProgram workoutProgram2 = context.WorkoutPrograms
                    .Include(wp => wp.Workouts).FirstOrDefault(w => w.Id == 2);

                WorkoutProgram workoutProgram3 = context.WorkoutPrograms
                    .Include(wp => wp.Workouts).FirstOrDefault(wp => wp.Id == 3);

                Workout workout1 = context.Workouts.Find(1);
                Workout workout2 = context.Workouts.Find(2);
                Workout workout3 = context.Workouts.Find(3);

                workoutProgram1.Workouts.Remove(workout1);
                workoutProgram2.Workouts.Remove(workout2);
                workoutProgram3.Workouts.Remove(workout3);

                context.SaveChanges();
            }
        }
    }
}
