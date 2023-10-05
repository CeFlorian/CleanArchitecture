using NorthWind.Sales.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.BusinessObjects.POCOEntities;
using NorthWind.SqlClient.Repositories.DataServices;

namespace NorthWind.SqlClient.Repositories.Repositories
{
    public class NorthWindSqlClientQueryRepository : INorthWindSalesQuerysRepository
    {

        readonly NorthWindDataAccess DataAccess;

        public NorthWindSqlClientQueryRepository(NorthWindDataAccess dataAccess) =>
            DataAccess = dataAccess;

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await DataAccess.GetAllOrders(true);
        }

        public Task<Order> GetOrderById(int id)
        {
            //throw new NotImplementedException();
            return Task.FromResult(DataAccess.GetAllOrders(true).Result.Where(o => o.Id == id).SingleOrDefault());
        }
    }
}
