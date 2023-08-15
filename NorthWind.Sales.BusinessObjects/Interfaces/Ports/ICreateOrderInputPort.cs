namespace NorthWind.Sales.BusinessObjects.Interfaces.Ports
{
    public interface ICreateOrderInputPort
    {
        ValueTask Handle(CreateOrderDTO orderDto);
    }
}
