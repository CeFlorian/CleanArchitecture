namespace NorthWind.EFCore.Repositories.Repositories
{
    public class NorthWindSalesQuerysRepository : INorthWindSalesQuerysRepository
    {

        readonly NorthWindSalesContext Context;

        public NorthWindSalesQuerysRepository(NorthWindSalesContext context) =>
            Context = context;

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await Context.Orders.ToListAsync();
        }
    }
}
