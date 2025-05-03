using System.ComponentModel.DataAnnotations;

namespace RandomTasks.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        // 1 = Admin, 2 = Customer
        public int RoleId { get; set; }
    }
}
