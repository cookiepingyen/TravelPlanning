using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TravelPlanning.Database.Entities
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<FavoriteItem> FavoriteItem { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Trip> Trip { get; set; }
        public virtual DbSet<TripDayPlace> TripDayPlace { get; set; }
        public virtual DbSet<TripDays> TripDays { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .Property(e => e.Icon)
                .IsFixedLength();

            modelBuilder.Entity<Favorite>()
                .HasMany(e => e.FavoriteItem)
                .WithRequired(e => e.Favorite)
                .HasForeignKey(e => e.Favorite_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FavoriteItem>()
                .Property(e => e.Place_id)
                .IsUnicode(false);

            modelBuilder.Entity<Trip>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Trip>()
                .Property(e => e.Cover)
                .IsFixedLength();

            modelBuilder.Entity<Trip>()
                .HasMany(e => e.TripDays)
                .WithRequired(e => e.Trip)
                .HasForeignKey(e => e.Trip_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TripDayPlace>()
                .Property(e => e.Place_id)
                .IsUnicode(false);

            modelBuilder.Entity<TripDays>()
                .Property(e => e.Place_id)
                .IsFixedLength();

            modelBuilder.Entity<TripDays>()
                .HasMany(e => e.TripDayPlace)
                .WithRequired(e => e.TripDays)
                .HasForeignKey(e => e.TripDays_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Favorite)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Trip)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_id)
                .WillCascadeOnDelete(false);
        }
    }
}
