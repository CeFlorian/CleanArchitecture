namespace NorthWind.Sales.BusinessObjects.POCOEntities.DomainEvents
{
    public class DomainEvent<T>
    {
        private List<Action<T>> Actions { get; } = new List<Action<T>>();

        public void Register(Action<T> callback)
        {
            if (Actions.Exists(o => o.Method == callback.Method))
            {
                return;
            }

            Actions.Add(callback);
        }

        public void Unregister(Action<T> callback)
        {
            if (Actions.Exists(o => o.Method == callback.Method))
            {
                Actions.Remove(callback);
            }
        }

        public void Publish(T args)
        {
            foreach (Action<T> action in Actions)
            {
                action.Invoke(args);
            }
        }

    }
}
