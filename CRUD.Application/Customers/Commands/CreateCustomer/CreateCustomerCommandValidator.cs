using FluentValidation;
using System;

namespace CRUD.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("O CPF/CNPJ é obrigatório")
                .Must(BeValidDocument).WithMessage("CPF/CNPJ inválido");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("Email inválido")
                .MaximumLength(100).WithMessage("O email deve ter no máximo 100 caracteres");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("O telefone é obrigatório")
                .Matches(@"^\(\d{2}\)\s\d{5}-\d{4}$").WithMessage("O telefone deve estar no formato (99) 99999-9999");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Tipo de cliente inválido");

            When(x => x.Type == Domain.Enums.CustomerType.Person, () =>
            {
                RuleFor(x => x.BirthDate)
                    .NotEmpty().WithMessage("A data de nascimento é obrigatória para pessoa física")
                    .Must(BeAtLeast18YearsOld).WithMessage("O cliente deve ter pelo menos 18 anos");
            });

            When(x => x.Type == Domain.Enums.CustomerType.Company, () =>
            {
                RuleFor(x => x.StateRegistration)
                    .NotEmpty().When(x => !x.IsStateRegistrationExempt)
                    .WithMessage("A inscrição estadual é obrigatória para pessoa jurídica não isenta");
            });

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("O CEP é obrigatório")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato 99999-999");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("O endereço é obrigatório")
                .MaximumLength(200).WithMessage("O endereço deve ter no máximo 200 caracteres");

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("O número é obrigatório")
                .MaximumLength(10).WithMessage("O número deve ter no máximo 10 caracteres");

            RuleFor(x => x.Neighborhood)
                .NotEmpty().WithMessage("O bairro é obrigatório")
                .MaximumLength(100).WithMessage("O bairro deve ter no máximo 100 caracteres");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("A cidade é obrigatória")
                .MaximumLength(100).WithMessage("A cidade deve ter no máximo 100 caracteres");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("O estado é obrigatório")
                .Length(2).WithMessage("O estado deve ter 2 caracteres");
        }

        private bool BeValidDocument(string document)
        {
            if (string.IsNullOrWhiteSpace(document))
                return false;

            document = document.Replace(".", "").Replace("-", "").Replace("/", "");

            if (document.Length == 11)
                return IsValidCPF(document);
            else if (document.Length == 14)
                return IsValidCNPJ(document);

            return false;
        }

        private bool IsValidCPF(string cpf)
        {
            // Implementação da validação de CPF
            // TODO: Implementar validação real de CPF
            return true;
        }

        private bool IsValidCNPJ(string cnpj)
        {
            // Implementação da validação de CNPJ
            // TODO: Implementar validação real de CNPJ
            return true;
        }

        private bool BeAtLeast18YearsOld(DateTime? birthDate)
        {
            if (!birthDate.HasValue)
                return false;

            var age = DateTime.Today.Year - birthDate.Value.Year;
            if (birthDate.Value.Date > DateTime.Today.AddYears(-age))
                age--;

            return age >= 18;
        }
    }
} 