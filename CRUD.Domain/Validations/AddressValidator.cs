using FluentValidation;
using CRUD.Domain.ValueObjects;

namespace CRUD.Domain.Validations
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("CEP é obrigatório")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("CEP deve estar no formato 99999-999");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Endereço é obrigatório")
                .MaximumLength(200).WithMessage("Endereço deve ter no máximo 200 caracteres");

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Número é obrigatório")
                .MaximumLength(10).WithMessage("Número deve ter no máximo 10 caracteres");

            RuleFor(x => x.Neighborhood)
                .NotEmpty().WithMessage("Bairro é obrigatório")
                .MaximumLength(100).WithMessage("Bairro deve ter no máximo 100 caracteres");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Cidade é obrigatória")
                .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("Estado é obrigatório")
                .Length(2).WithMessage("Estado deve ter 2 caracteres")
                .Matches(@"^[A-Z]{2}$").WithMessage("Estado deve estar no formato UF (ex: SP)");
        }
    }
} 