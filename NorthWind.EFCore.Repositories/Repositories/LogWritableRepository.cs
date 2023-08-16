using NorthWind.Entities.Interfaces;
using NorthWind.Entities.POCOs;

namespace NorthWind.EFCore.Repositories.Repositories
{
    public class LogWritableRepository : ILogWritableRepository
    {
        readonly NorthWindSalesContext Context;
        public LogWritableRepository(NorthWindSalesContext context)
        {
            Context = context;
        }

        public async Task Add(Log log)
        {
            Context.Add(log);
            await SaveChanges();
        }

        public async Task Add(string description)
        {
            await Add(new Log(description));
        }

        public async ValueTask SaveChanges()
        {
            await Context.SaveChangesAsync();
        }

    }

}
