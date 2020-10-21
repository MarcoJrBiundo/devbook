using System;
using System.Collections.Generic;

namespace devbook.api.models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
         public string GithubLink { get; set; }
        public string Status { get; set; }
        public double Rating { get; set; }
        public string Gender { get; set; }
        public string Interest { get; set; }
        public DateTime LastActive { get; set; }   
        public DateTime Created { get; set; }   
        public DateTime DateOfBirth { get; set; } 
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Favourite> Favouriters { get; set; }
        public virtual ICollection<Favourite> Favouritees { get; set; }
        public virtual ICollection<Messages> MessagesSent { get; set; }
        public virtual ICollection<Messages> MessagesReceived  { get; set; }
        public virtual ICollection<Skills> Skills { get; set; }
    }
}