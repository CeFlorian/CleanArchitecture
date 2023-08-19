namespace NorthWind.Sales.BusinessObjects.POCOEntities
{
    public class Claim
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
