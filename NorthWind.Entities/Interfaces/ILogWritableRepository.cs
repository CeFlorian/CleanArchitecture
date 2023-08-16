using NorthWind.Entities.POCOs;

namespace NorthWind.Entities.Interfaces
{
    public interface ILogWritableRepository : IUnitOfWork
    {
        Task Add(Log log);
        Task Add(string description);
    }
}
