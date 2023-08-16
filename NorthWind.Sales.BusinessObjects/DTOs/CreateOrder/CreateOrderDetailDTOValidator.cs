using FluentValidation;
using NorthWind.Entities.Validators;

namespace NorthWind.Sales.BusinessObjects.DTOs.CreateOrder
{
    public class CreateOrderDetailDTOValidator :
        AbstractValidator<CreateOrderDetailDTO>,
        Entities.Validators.IValidator<CreateOrderDetailDTO>
    {
        public CreateOrderDetailDTOValidator()
        {
            RuleFor(d => d.ProductId).GreaterThan(0)
                .WithMessage("Debe especificar el identificador del producto.");

            RuleFor(d => d.Quantity).GreaterThan((short)0)
                .WithMessage("Debe especificar la cantidad ordenada del producto.");

            RuleFor(d => d.UnitPrice).GreaterThan(0)
                .WithMessage("Debe especificarse el precio del producto.");

        }

        ValidationResult Entities.Validators.IValidator<CreateOrderDetailDTO>
            .Validate(CreateOrderDetailDTO instance)
        {
            var Result = base.Validate(instance);
            return new ValidationResult(
                Result.Errors?
                .Select(e =>
                new ValidationFailure(e.PropertyName, e.ErrorMessage)));
        }
    }
}
