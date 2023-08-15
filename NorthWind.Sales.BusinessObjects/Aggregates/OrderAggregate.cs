namespace NorthWind.Sales.BusinessObjects.Aggregates
{
    public class OrderAggregate : Order
    {
        /*
         * Agregado como grupo de objetos de dominio que pueden ser tratados como una unidad.
         * Por ejemplo puede ser una orden con su detalle
         * Cualquier referencia desde fuera del Agregado solo debe ir a la raíz del Agregado.
         * De este modo, la raíz puede garantizar la integridad del Agregado en su conjunto.
         */

        readonly List<OrderDetail> OrderDetailsField = new List<OrderDetail>();
        public IReadOnlyCollection<OrderDetail> OrderDetails => OrderDetailsField;

        // Agregar el detalle de la orden.
        // Si ya se agregó un ID de producto previamente, sumar la cantidad.        
        void AddDetail(OrderDetail orderDetail)
        {
            var ExistingOrderDetail = OrderDetailsField.FirstOrDefault(
                                                        o => o.ProductId == orderDetail.ProductId);

            if (ExistingOrderDetail != default)
            {
                OrderDetailsField.Add(
                    ExistingOrderDetail with
                    {
                        Quantity = (short)(ExistingOrderDetail.Quantity + orderDetail.Quantity)
                    });

                OrderDetailsField.Remove(ExistingOrderDetail);
            }
            else
            {
                OrderDetailsField.Add(orderDetail);
            }
        }

        public void AddDetail(int productId, decimal unitPrice, short quantity) =>
            AddDetail(new OrderDetail(productId, unitPrice, quantity));

    }
}
