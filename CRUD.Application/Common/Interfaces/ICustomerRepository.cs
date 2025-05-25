using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Domain.Entities;

namespace CRUD.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Customer> GetByDocumentAsync(string document, CancellationToken cancellationToken = default);
        Task<Customer> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
} 