using NorthWind.Entities.POCOs;

namespace NorthWind.EFCore.Repositories.DataContexts
{
    internal class NorthWindContext : DbContext
    {
        /*
         * Add-Migration InitialCreate -p NorthWind.EFcore.Repositories -s NorthWind.EFcore.Repositories -c NorthWindContext
         * Update-Database -p NorthWind.EFcore.Repositories -s NorthWind.EFcore.Repositories -context NorthWindContext
         */

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=NorthWindDB");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());
        }
    }
}
