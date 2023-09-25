namespace NorthWind.Sales.BusinessObjects.Interfaces.Controllers.Orders
{
    public interface ICreateOrderController
    {
        ValueTask<int> CreateOrder(CreateOrderDTO order);
    }
}
