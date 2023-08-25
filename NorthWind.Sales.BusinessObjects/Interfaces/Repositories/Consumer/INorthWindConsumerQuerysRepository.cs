namespace NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer
{
    public interface INorthWindConsumerQuerysRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
    }
}
