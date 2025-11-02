using Microsoft.AspNetCore.Identity;

namespace FitnessTracker.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public double DailyCalorieGoal { get; set; } = 2000;
    }
}
