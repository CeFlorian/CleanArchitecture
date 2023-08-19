namespace NorthWind.Sales.BusinessObjects.POCOEntities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; } = true;
        public bool Used { get; set; } = false;
        public DateTime ExpirationDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
