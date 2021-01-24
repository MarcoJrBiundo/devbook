using System;

namespace devbook.api.models
{
    public class Interests
    {
        public int Id { get; set; } 
        public string Interest { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}