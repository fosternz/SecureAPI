using System.ComponentModel.DataAnnotations;

namespace SecureAPI.Models
{
    public class UserModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }
    }
}
