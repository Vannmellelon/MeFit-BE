namespace MeFit_BE.Models.DTO.WorkoutProgram
{
    public class WorkoutProgramWriteDTO
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Difficulty { get; set; }

        public int GoalId { get; set; }
    }
}
