namespace RandomTasks.Models
{
    public class UserViewModel
    {
        public string? Name { get; set; }   // 👈 make nullable
        public string? Email { get; set; }
        public int? Age { get; set; }
    }

}
