using System;
using FluentValidation;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Validations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("CPF/CNPJ é obrigatório")
                .Must(BeValidDocument).WithMessage("CPF/CNPJ inválido");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido")
                .MaximumLength(100).WithMessage("E-mail deve ter no máximo 100 caracteres");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .Matches(@"^\(\d{2}\)\s\d{5}-\d{4}$").WithMessage("Telefone deve estar no formato (99) 99999-9999");

            RuleFor(x => x.BirthDate)
                .Must((customer, birthDate) => 
                {
                    if (customer.Type == Enums.CustomerType.Individual)
                    {
                        if (!birthDate.HasValue)
                            return false;
                        
                        var age = DateTime.Today.Year - birthDate.Value.Year;
                        if (birthDate.Value.Date > DateTime.Today.AddYears(-age))
                            age--;

                        return age >= 18;
                    }
                    return true;
                })
                .WithMessage("Cliente pessoa física deve ter no mínimo 18 anos");

            RuleFor(x => x.StateRegistration)
                .Must((customer, stateRegistration) =>
                {
                    if (customer.Type == Enums.CustomerType.Corporate)
                    {
                        return !string.IsNullOrEmpty(stateRegistration) || customer.IsStateRegistrationExempt;
                    }
                    return true;
                })
                .WithMessage("Inscrição Estadual é obrigatória para pessoa jurídica, a menos que seja isento");
        }

        private bool BeValidDocument(string document)
        {
            if (string.IsNullOrEmpty(document))
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
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais
            if (cpf.All(x => x == cpf[0]))
                return false;

            // Validação do primeiro dígito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (cpf[i] - '0') * (10 - i);

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            if (digit1 != (cpf[9] - '0'))
                return false;

            // Validação do segundo dígito verificador
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (cpf[i] - '0') * (11 - i);

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return digit2 == (cpf[10] - '0');
        }

        private bool IsValidCNPJ(string cnpj)
        {
            if (cnpj.Length != 14)
                return false;

            // Verifica se todos os dígitos são iguais
            if (cnpj.All(x => x == cnpj[0]))
                return false;

            // Validação do primeiro dígito verificador
            int[] multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += (cnpj[i] - '0') * multiplier1[i];

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            if (digit1 != (cnpj[12] - '0'))
                return false;

            // Validação do segundo dígito verificador
            int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += (cnpj[i] - '0') * multiplier2[i];

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return digit2 == (cnpj[13] - '0');
        }
    }
} 