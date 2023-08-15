namespace NorthWind.Sales.Presenters
{
    public class CreateOrderPresenter : ICreateOrderPresenter
    {
        public int OrderId { get; private set; }

        // Se puede agregar logica para formatear el dato para el traslado a la capa de externa Frameworks y Drivers
        public ValueTask Handle(int orderId)
        {
            OrderId = orderId;
            return ValueTask.CompletedTask;
        }
    }
}
