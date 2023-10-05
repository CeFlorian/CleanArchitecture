namespace NorthWind.Sales.BusinessObjects.POCOEntities.DomainEvents
{
    public static class Events
    {
        public static readonly DomainEvent<AccessTokenGenerated> AccessTokenGenerated = new DomainEvent<AccessTokenGenerated>();

    }
}
