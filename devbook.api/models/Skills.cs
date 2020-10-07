using System;

namespace devbook.api.models
{
    public class Skills
    {
        
        public string Skill { get; set; }
        public int Id { get; set; } 
         public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}