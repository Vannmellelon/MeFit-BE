﻿namespace MeFit_BE.Models.DTO.Exercise
{
    public class ExerciseEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string TargetMuscleGroup { get; set; }

        public string Image { get; set; }

        public string Video { get; set; }
    }
}