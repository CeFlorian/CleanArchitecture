namespace NorthWind.EFCore.Repositories.Repositories
{
    public class NorthWindSalesQuerysRepository : INorthWindSalesQuerysRepository
    {

        readonly NorthWindSalesContext Context;

        public NorthWindSalesQuerysRepository(NorthWindSalesContext context) =>
            Context = context;


        /*
         * AsNoTrackingWithIdentityResolution: no realiza un seguimiento completo de las entidades, util cuando se realizaran cambios sobre la instancia,
         * si recupera el mismo usuario en diferentes del código, se utiliza la misma instancia en memoria para evitar inconsistencias
         * AsNoTracking: no realiza un seguimiento completo de las entidades, util cuando no se realizaran cambios sobre la instancia, 
         * no importa si hay múltiples instancias del mismo registro en memoria.
         */

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await Context.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await Context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
        }
    }
}
