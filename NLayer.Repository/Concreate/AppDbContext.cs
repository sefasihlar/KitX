using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using System.Reflection;

namespace NLayer.Repository.Concreate
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<QrCode> QrCodes { get; set; }
        public DbSet<IPAddress> IPAddresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<AnimalProductFeature> AnimalProductFeature { get; set; }
        public DbSet<BelongingProductFeature> BelongingProductFeature { get; set; }
        public DbSet<PersonProductFeature> PersonProductFeature { get; set; }
        public DbSet<SpecialProductFeature> SpecialProductFeature { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Log> Logs { get; set; }


    }



}
