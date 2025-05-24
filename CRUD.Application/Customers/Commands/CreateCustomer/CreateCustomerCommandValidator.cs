using FluentValidation;

namespace CRUD.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("CPF/CNPJ é obrigatório")
                .Must(doc => 
                {
                    if (string.IsNullOrEmpty(doc))
                        return false;

                    doc = doc.Replace(".", "").Replace("-", "").Replace("/", "");
                    return doc.Length == 11 || doc.Length == 14;
                }).WithMessage("CPF/CNPJ inválido");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido")
                .MaximumLength(100).WithMessage("E-mail deve ter no máximo 100 caracteres");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .Matches(@"^\(\d{2}\)\s\d{5}-\d{4}$").WithMessage("Telefone deve estar no formato (99) 99999-9999");

            RuleFor(x => x.BirthDate)
                .Must((command, birthDate) => 
                {
                    if (command.Type == Domain.Enums.CustomerType.Individual)
                    {
                        if (!birthDate.HasValue)
                            return false;
                        
                        var age = System.DateTime.Today.Year - birthDate.Value.Year;
                        if (birthDate.Value.Date > System.DateTime.Today.AddYears(-age))
                            age--;

                        return age >= 18;
                    }
                    return true;
                })
                .WithMessage("Cliente pessoa física deve ter no mínimo 18 anos");

            RuleFor(x => x.StateRegistration)
                .Must((command, stateRegistration) =>
                {
                    if (command.Type == Domain.Enums.CustomerType.Corporate)
                    {
                        return !string.IsNullOrEmpty(stateRegistration) || command.IsStateRegistrationExempt;
                    }
                    return true;
                })
                .WithMessage("Inscrição Estadual é obrigatória para pessoa jurídica, a menos que seja isento");

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