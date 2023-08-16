using NorthWind.Entities.POCOs;

namespace NorthWind.EFCore.Repositories.DataContexts
{
    public class NorthWindSalesContext : DbContext
    {
        public NorthWindSalesContext(
            DbContextOptions<NorthWindSalesContext> options)
            : base(options) { }

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
