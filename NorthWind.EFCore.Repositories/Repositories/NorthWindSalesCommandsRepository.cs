﻿using NorthWind.Entities.Interfaces;

namespace NorthWind.EFCore.Repositories.Repositories
{
    public class NorthWindSalesCommandsRepository : INorthWindSalesCommandsRepository
    {
        readonly NorthWindSalesContext Context;
        readonly IApplicationStatusLogger Logger;

        public NorthWindSalesCommandsRepository(NorthWindSalesContext context, IApplicationStatusLogger logger)
        {
            Context = context;
            Logger = logger;
        }


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
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }

        }
    }
}
