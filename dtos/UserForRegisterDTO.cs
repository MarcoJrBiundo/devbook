using System;
using System.ComponentModel.DataAnnotations;

namespace devbook.api.dtos
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }  
        [Required]
        public string Interest { get; set; }  
        [Required]
        public string FirstName { get; set; }  
        [Required]
        public string Lastname { get; set; }  
        [Required]
        public string GithubLink { get; set; }  
        [Required]
        public DateTime DateOfBirth { get; set; }   
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime LastActive { get; set; }

        public UserForRegisterDTO()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}
