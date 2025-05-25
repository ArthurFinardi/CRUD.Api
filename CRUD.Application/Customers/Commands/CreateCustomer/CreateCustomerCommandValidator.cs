using FluentValidation;
using System;
using System.Linq;

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
            if (cpf.Length != 11 || cpf.All(x => x == cpf[0]))
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        private bool IsValidCNPJ(string cnpj)
        {
            if (cnpj.Length != 14 || cnpj.All(x => x == cnpj[0]))
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
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