namespace NorthWind.Sales.BusinessObjects.Interfaces.Controllers
{
    public interface IGetAllOrdersController
    {
        Task<IEnumerable<Order>> GetAllOrders();

    }
}
