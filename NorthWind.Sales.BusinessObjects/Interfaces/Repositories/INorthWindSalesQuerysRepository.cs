namespace NorthWind.Sales.BusinessObjects.Interfaces.Repositories
{
    public interface INorthWindSalesQuerysRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
    }
}
