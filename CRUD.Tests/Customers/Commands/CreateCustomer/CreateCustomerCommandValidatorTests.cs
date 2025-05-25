using System;
using FluentValidation.TestHelper;
using Xunit;
using CRUD.Application.Customers.Commands.CreateCustomer;
using CRUD.Domain.Enums;

namespace CRUD.Tests.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidatorTests
    {
        private readonly CreateCustomerCommandValidator _validator;

        public CreateCustomerCommandValidatorTests()
        {
            _validator = new CreateCustomerCommandValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Empty(string name)
        {
            var command = new CreateCustomerCommand 
            { 
                Name = name,
                Document = "123.456.789-09",
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS"
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("Jo")]
        [InlineData("A")]
        public void Should_Have_Error_When_Name_Is_Too_Short(string name)
        {
            var command = new CreateCustomerCommand 
            { 
                Name = name,
                Document = "123.456.789-09",
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS"
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("123.456.789-09")] // CPF válido
        [InlineData("529.982.247-25")] // CPF válido
        [InlineData("12.345.678/0001-95")] // CNPJ válido
        [InlineData("60.701.190/0001-04")] // CNPJ válido
        public void Should_Not_Have_Error_When_Document_Is_Valid(string document)
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = document,
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Document);
        }

        [Theory]
        [InlineData("123.456.789-00")] // CPF inválido
        [InlineData("111.111.111-11")] // CPF com dígitos iguais
        [InlineData("12.345.678/0001-00")] // CNPJ inválido
        [InlineData("11.111.111/1111-11")] // CNPJ com dígitos iguais
        public void Should_Have_Error_When_Document_Is_Invalid(string document)
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = document,
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS"
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Document);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid-email")]
        [InlineData("test@")]
        [InlineData("@test.com")]
        public void Should_Have_Error_When_Email_Is_Invalid(string email)
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = "123.456.789-09",
                Email = email,
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS"
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234")]
        [InlineData("(11)12345-6789")]
        public void Should_Have_Error_When_Phone_Is_Invalid(string phone)
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = "123.456.789-09",
                Email = "test@test.com",
                Phone = phone,
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS"
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        [Fact]
        public void Should_Have_Error_When_BirthDate_Is_Required_For_Person()
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = "123.456.789-09",
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS",
                Type = CustomerType.Individual,
                BirthDate = null
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);
        }

        [Fact]
        public void Should_Have_Error_When_Person_Is_Under_18()
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = "123.456.789-09",
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS",
                Type = CustomerType.Individual,
                BirthDate = DateTime.Today.AddYears(-17)
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);
        }

        [Fact]
        public void Should_Have_Error_When_StateRegistration_Is_Required_For_Company()
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = "12.345.678/0001-95",
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS",
                Type = CustomerType.Corporate,
                IsStateRegistrationExempt = false,
                StateRegistration = null
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.StateRegistration);
        }

        [Theory]
        [InlineData("")]
        [InlineData("12345")]
        public void Should_Have_Error_When_ZipCode_Is_Invalid(string zipCode)
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = "123.456.789-09",
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = zipCode,
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS"
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ZipCode);
        }

        [Theory]
        [InlineData("")]
        public void Should_Have_Error_When_Address_Fields_Are_Empty(string value)
        {
            var command = new CreateCustomerCommand 
            { 
                Name = "Test Name",
                Document = "123.456.789-09",
                Email = "test@test.com",
                Phone = "(11) 99999-9999",
                ZipCode = "12345-678",
                Street = value,
                Number = value,
                Neighborhood = value,
                City = value,
                State = value
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Street);
            result.ShouldHaveValidationErrorFor(x => x.Number);
            result.ShouldHaveValidationErrorFor(x => x.Neighborhood);
            result.ShouldHaveValidationErrorFor(x => x.City);
            result.ShouldHaveValidationErrorFor(x => x.State);
        }
    }
} 