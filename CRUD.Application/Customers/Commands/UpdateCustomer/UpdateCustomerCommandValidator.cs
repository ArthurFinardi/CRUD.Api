using FluentValidation;

namespace CRUD.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID do cliente é obrigatório");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido")
                .MaximumLength(100).WithMessage("E-mail deve ter no máximo 100 caracteres");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .Matches(@"^\(\d{2}\)\s\d{5}-\d{4}$").WithMessage("Telefone deve estar no formato (99) 99999-9999");

            RuleFor(x => x.Address).NotNull().WithMessage("Endereço é obrigatório");
            RuleFor(x => x.Address.ZipCode)
                .NotEmpty().WithMessage("CEP é obrigatório")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("CEP deve estar no formato 99999-999");

            RuleFor(x => x.Address.Street)
                .NotEmpty().WithMessage("Endereço é obrigatório")
                .MaximumLength(200).WithMessage("Endereço deve ter no máximo 200 caracteres");

            RuleFor(x => x.Address.Number)
                .NotEmpty().WithMessage("Número é obrigatório")
                .MaximumLength(10).WithMessage("Número deve ter no máximo 10 caracteres");

            RuleFor(x => x.Address.Neighborhood)
                .NotEmpty().WithMessage("Bairro é obrigatório")
                .MaximumLength(100).WithMessage("Bairro deve ter no máximo 100 caracteres");

            RuleFor(x => x.Address.City)
                .NotEmpty().WithMessage("Cidade é obrigatória")
                .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres");

            RuleFor(x => x.Address.State)
                .NotEmpty().WithMessage("Estado é obrigatório")
                .Length(2).WithMessage("Estado deve ter 2 caracteres")
                .Matches(@"^[A-Z]{2}$").WithMessage("Estado deve estar no formato UF (ex: SP)");
        }
    }
} 