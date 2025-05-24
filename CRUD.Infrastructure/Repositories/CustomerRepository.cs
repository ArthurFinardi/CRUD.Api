 using CRUD.Application.Interfaces;
using CRUD.Domain.Entities;
using CRUD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.Address)
            .ToListAsync();
    }

    public async Task<Customer> GetByDocumentAsync(string document)
    {
        return await _context.Customers
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Document == document);
    }

    public async Task<Customer> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Guid> AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer.Id;
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Customer customer)
    {
        _context.Customers.Remove(customer);
        return await _context.SaveChangesAsync() > 0;
    }
}