namespace MeFit_BE.Models.Domain
{
    public static class Category
    { 
        public static readonly string STAMINA = "Stamina";
        public static readonly string CORE = "Core";
        public static readonly string ARMS = "Arms";
        public static readonly string LEGS = "Legs";
        public static readonly string FULL_BODY = "Full body";
        public static readonly string FLEXIBILITY = "Flexibility";

        /// <summary>
        /// Method checks if the given string matches any of the categories.
        /// </summary>
        /// <param name="category">String category.</param>
        /// <returns>Boolean</returns>
        public static bool IsValid(string category)
        {
            if (category == STAMINA) return true;
            if (category == CORE) return true;
            if (category == ARMS) return true;
            if (category == LEGS) return true;
            if (category == FULL_BODY) return true;
            if (category == FLEXIBILITY) return true;
            return false;
        }
    }

    public static class Difficulty
    {
        public static readonly string BEGINNER = "Beginner";
        public static readonly string INTERMEDIATE = "Intermediate";
        public static readonly string EXPERT = "Expert";


        /// <summary>
        /// Method checks if the given string matches any of the difficulties.
        /// </summary>
        /// <param name="difficulty">String difficulty</param>
        /// <returns>Boolean</returns>
        public static bool IsValid(string difficulty)
        {
            if (difficulty == BEGINNER) return true;
            if (difficulty == INTERMEDIATE) return true;
            if (difficulty == EXPERT) return true;
            return false;
        }
    }
}
