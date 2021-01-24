namespace devbook.api.models
{
    public class Favourite
    {


        public int FavouriterId { get; set; }    
        public int FavouriteeId { get; set; }
        public virtual User Favouriter { get; set; } 
        public virtual User Favouritee { get; set; }
        
    }
}