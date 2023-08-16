using NorthWind.Entities.Interfaces;
using NorthWind.Entities.POCOs;

namespace NorthWind.EFCore.Repositories.Repositories
{
    public class LogWritableRepository : ILogWritableRepository
    {
        readonly NorthWindSalesContext Context;
        readonly IApplicationStatusLogger Logger;
        public LogWritableRepository(NorthWindSalesContext context, IApplicationStatusLogger logger)
        {
            Context = context;
            Logger = logger;
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
