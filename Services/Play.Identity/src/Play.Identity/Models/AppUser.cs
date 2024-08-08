using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Play.Identity.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(50)]
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}