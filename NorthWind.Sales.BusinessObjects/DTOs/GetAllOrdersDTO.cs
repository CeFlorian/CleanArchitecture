namespace NorthWind.Sales.BusinessObjects.DTOs
{
    public class GetAllOrdersDTO
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string ShipAddress { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
