namespace FitnessTracker.Dtos
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;

        public string? email { get; set; } = string.Empty;

        public string? password { get; set; } = string.Empty;

        public double? DailyCalorieGoal { get; set; }
    }
}