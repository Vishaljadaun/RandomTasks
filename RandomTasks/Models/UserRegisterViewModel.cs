using System.ComponentModel.DataAnnotations;

namespace RandomTasks.Models
{
    public class UserRegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // 1 = Admin, 2 = Customer
        [Required]
        [Display(Name = "Select Role")]
        public int RoleId { get; set; }
    }
}
