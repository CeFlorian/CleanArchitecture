namespace NorthWind.Sales.BusinessObjects.POCOEntities.DomainEvents
{
    //Para que sea inmutable (record) y de tipo valor (struct)
    public record struct AccessTokenGenerated(Guid userId, string name);
}
