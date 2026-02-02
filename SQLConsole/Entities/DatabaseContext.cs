using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SQLConsole.Entities
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DBContext1")
        {
        }

        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<FavoriteItem> FavoriteItem { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Trip> Trip { get; set; }
        public virtual DbSet<TripDetail> TripDetail { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Favorite>()
                .HasMany(e => e.FavoriteItem)
                .WithRequired(e => e.Favorite)
                .HasForeignKey(e => e.Favorite_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FavoriteItem>()
                .Property(e => e.Place_id)
                .IsFixedLength();

            modelBuilder.Entity<Trip>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Trip>()
                .Property(e => e.Cover)
                .IsFixedLength();

            modelBuilder.Entity<Trip>()
                .HasMany(e => e.TripDetail)
                .WithRequired(e => e.Trip)
                .HasForeignKey(e => e.Trip_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TripDetail>()
                .Property(e => e.Place_id)
                .IsFixedLength();

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
