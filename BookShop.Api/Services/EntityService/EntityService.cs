using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using BookShop.Domain.Entities;
using BookShop.Domain.Models;
using BookShop.Api.Data;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.Api.Services;


public abstract class EntityService<T> : IEntityService<T> where T : Entity
{
    protected readonly BookShopContext _context;

    public EntityService(BookShopContext context)
    {
        _context = context;
    }

    protected abstract DbSet<T> DbSet { get; }


    public Task<ResponseData<IEnumerable<T>>> GetAllAsync()
    {
        var response = DbSet.IsNullOrEmpty()
            ? new ResponseData<IEnumerable<T>>(errorMessage: "Entities not found")
            : new ResponseData<IEnumerable<T>>(DbSet);

        return Task.FromResult(response);
    }

    public Task<ResponseData<T>> GetByIdAsync(int id)
    {
        return FirstOrDefaultAsync(entity => entity.Id == id);
    }


    public Task<ResponseData<IEnumerable<T>>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        var entities = DbSet.Where(predicate);

        var response = entities.IsNullOrEmpty()
            ? new ResponseData<IEnumerable<T>>(errorMessage: "Entities not found")
            : new ResponseData<IEnumerable<T>>(entities);

        return Task.FromResult(response);
    }


    public Task<ResponseData<T>> FirstOrDefaultAsync()
    {
        return FirstOrDefaultAsync(entity => true);
    }

    public async Task<ResponseData<T>> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        var entity = await DbSet.FirstOrDefaultAsync(predicate);

        if (entity is null)
        {
            return new ResponseData<T>(errorMessage: "Entity not found");
        }
        else
        {
            return new ResponseData<T>(entity);
        }
    }


    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }



    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new DbUpdateConcurrencyException("Entity not found", e);
        }
    }

    public async Task UpdateByIdAsync(int id, Func<T, T> update)
    {
        var entity = DbSet.FirstOrDefault(entity => entity.Id == id);

        if (entity is null)
            throw new DbUpdateConcurrencyException("Entity not found");

        entity = update(entity);
        _context.Entry(entity).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }


    public async Task DeleteByIdAsync(int id)
    {
        var entity = _context.Books.FirstOrDefault(entity => entity.Id == id);

        if (entity is null)
            throw new DbUpdateConcurrencyException("Entity not found");

        _context.Remove(entity!);

        await _context.SaveChangesAsync();
    }

    public async Task ClearAsync()
    {
        var entities = DbSet.ToArray();
        _context.RemoveRange(entities);

        await _context.SaveChangesAsync();
    }
}
