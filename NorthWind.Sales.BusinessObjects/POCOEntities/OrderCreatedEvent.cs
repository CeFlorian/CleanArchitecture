using NorthWind.Sales.BusinessObjects.POCOEntities.Events;

namespace NorthWind.Sales.BusinessObjects.POCOEntities
{
    public class OrderCreatedEvent : Event
    {
        public string CustomerId { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipCountry { get; set; }
        public string ShipPostalCode { get; set; }
        public List<CreateOrderDetailDTO> OrderDetails { get; set; }
    }
}
