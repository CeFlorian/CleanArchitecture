namespace NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer
{
    public interface INorthWindConsumerQuerysRepository
    {
        Task<IEnumerable<OrderAggregate>> GetAllOrders();
        Task<OrderAggregate> GetOrderById(int id);
    }
}
