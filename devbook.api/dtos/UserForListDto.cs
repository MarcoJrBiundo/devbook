using System;
using System.Collections.Generic;

namespace devbook.api.dtos
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GithubLink { get; set; }
        public int Age { get; set; }
        public double Rating { get; set; }
        public DateTime LastActive { get; set; }   
        public DateTime Created { get; set; }   
        public DateTime DateOfBirth { get; set; } 
        public string PhotoUrl { get; set; }
         public virtual ICollection<InterestsForDetailedDto> Interests { get; set; }
        public virtual ICollection<SkillForDetailedDto> Skills { get; set; }
        
    }
}