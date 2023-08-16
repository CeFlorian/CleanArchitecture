﻿using NorthWind.Entities.Validators;

namespace NorthWind.Sales.UseCases.CreateOrder
{
    public class CreateOrderInteractor : ICreateOrderInputPort
    {
        readonly ICreateOrderOutputPort OutputPort;
        readonly INorthWindSalesCommandsRepository Repository;
        readonly IValidatorService<CreateOrderDTO> ValidatorService;
        readonly IEnumerable<IValidator<CreateOrderDTO>> Validators;


        public CreateOrderInteractor(ICreateOrderOutputPort outputPort,
            INorthWindSalesCommandsRepository repository,
            IValidatorService<CreateOrderDTO> validatorService,
            IEnumerable<IValidator<CreateOrderDTO>> validators)
        {
            OutputPort = outputPort;
            Repository = repository;
            ValidatorService = validatorService;
            Validators = validators;
        }

        public async ValueTask Handle(CreateOrderDTO orderDTO)
        {
            //ValidatorService.Validate(orderDTO, Validators, Logger);
            ValidatorService.Validate(orderDTO, Validators);

            OrderAggregate orderAggregate = new OrderAggregate
            {
                CustomerId = orderDTO.CustomerId,
                ShipAddress = orderDTO.ShipAddress,
                ShipCity = orderDTO.ShipCity,
                ShipCountry = orderDTO.ShipCountry,
                ShipPostalCode = orderDTO.ShipPostalCode
            };

            foreach (var item in orderDTO.OrderDetails)
            {
                orderAggregate.AddDetail(item.ProductId, item.UnitPrice, item.Quantity);
            }

            await Repository.CreateOrder(orderAggregate);
            await Repository.SaveChanges();
            await OutputPort.Handle(orderAggregate.Id);
        }
    }
}
