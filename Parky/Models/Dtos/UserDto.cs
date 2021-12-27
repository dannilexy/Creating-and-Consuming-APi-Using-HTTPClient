using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkyApi.Models.Dtos
{
    public class UserDto
    {
      
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
       
    }
}
