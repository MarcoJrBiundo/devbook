using devbook.api.models;
using Microsoft.EntityFrameworkCore;

namespace devbook.api.Data
{
    public class DataContext :DbContext
    {
    public DataContext(DbContextOptions<DataContext> options) : base(options){}   
    public DbSet<User> Users { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Favourite> Favourite { get; set; }
    public DbSet<Messages> Messages { get; set; }
    public DbSet<Skills> Skills {get; set;}
    public DbSet<Interests> Interests {get; set;}





        protected override void OnModelCreating(ModelBuilder builder){
        builder.Entity<Favourite>()
            .HasKey(k => new {k.FavouriterId, k.FavouriteeId});
        builder.Entity<Favourite>()
            .HasOne(u => u.Favouritee)
            .WithMany(u => u.Favouriters)
            .HasForeignKey(u => u.FavouriteeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Favourite>()
            .HasOne(u => u.Favouriter)
            .WithMany(u => u.Favouritees)
            .HasForeignKey(u => u.FavouriterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Messages>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Messages>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);


        
        }

        


    }
}