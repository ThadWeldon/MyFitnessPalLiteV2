namespace FitnessTracker.Dtos
{
    public class ReturnUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public double DailyCalorieGoal{ get; set; }
    }
}