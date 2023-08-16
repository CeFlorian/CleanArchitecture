namespace NorthWind.EFCore.Repositories.Repositories
{
    public class NorthWindSalesCommandsRepository : INorthWindSalesCommandsRepository
    {
        readonly NorthWindSalesContext Context;
        public NorthWindSalesCommandsRepository(
            NorthWindSalesContext context) =>
            Context = context;

        public async ValueTask CreateOrder(OrderAggregate order)
        {
            await Context.AddAsync(order);
            foreach (var Item in order.OrderDetails)
            {
                await Context.AddAsync(new OrderDetail
                {
                    Order = order,
                    ProductId = Item.ProductId,
                    Quantity = Item.Quantity,
                    UnitPrice = Item.UnitPrice
                });
            }
        }

        public async ValueTask SaveChanges()
        {
            await Context.SaveChangesAsync();
        }
    }
}
