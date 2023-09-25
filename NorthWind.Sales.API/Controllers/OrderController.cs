using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using NorthWind.Sales.BusinessObjects.Interfaces.Controllers.Orders;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NorthWind.Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly ICreateOrderController CreateOrderController;
        readonly IGetAllOrdersController GetAllOrdersController;

        public OrderController(ICreateOrderController createOrderController, IGetAllOrdersController getAllOrdersController)
        {
            CreateOrderController = createOrderController;
            GetAllOrdersController = getAllOrdersController;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public async Task<IResult> Get() =>
            Results.Ok(await GetAllOrdersController.GetAllOrders());


        // POST api/<OrderController>
        [HttpPost]
        public async Task<IResult> Post([FromBody] CreateOrderDTO order) =>
            Results.Ok(await CreateOrderController.CreateOrder(order));


        //// GET api/<OrderController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        //// PUT api/<OrderController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}
        //// DELETE api/<OrderController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
