namespace NorthWind.Sales.BusinessObjects.Interfaces.Ports
{
    public interface IGetAllOrdersOutputPort
    {
        Task Handle(IEnumerable<Order> orders);
    }
}
