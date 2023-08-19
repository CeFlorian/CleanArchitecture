namespace NorthWind.Sales.BusinessObjects.Interfaces.Controllers.Orders
{
    public interface IGetAllOrdersController
    {
        Task<IEnumerable<Order>> GetAllOrders();

    }
}
