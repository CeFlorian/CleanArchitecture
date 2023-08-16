using Microsoft.AspNetCore.Mvc;
using NorthWind.Entities.Validators;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using NorthWind.Sales.BusinessObjects.Interfaces.Controllers;

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
        public async Task<IActionResult> Get()
        {
            //return Ok(await GetAllOrdersController.GetAllOrders());
            return Ok(Results.Ok(await GetAllOrdersController.GetAllOrders()));
        }


        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderDTO order)
        {
            //return Ok(await CreateOrderController.CreateOrder(order));
            try { return Ok(Results.Ok(await CreateOrderController.CreateOrder(order))); }
            catch (ValidationException ex) { return BadRequest(Results.BadRequest(ex.Message)); }

        }


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
