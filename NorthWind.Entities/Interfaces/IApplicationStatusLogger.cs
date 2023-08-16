namespace NorthWind.Entities.Interfaces
{
    public interface IApplicationStatusLogger
    {
        Task Log(string message);
    }
}
