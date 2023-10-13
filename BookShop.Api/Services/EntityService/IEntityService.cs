using System.Linq.Expressions;

using BookShop.Domain.Entities;
using BookShop.Domain.Models;


namespace BookShop.Api.Services;


public interface IEntityService<T> where T : Entity
{
    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="GetAllAsync"]' />
    public Task<ResponseData<IEnumerable<T>>> GetAllAsync();


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="GetByIdAsync"]' />
    public Task<ResponseData<T>> GetByIdAsync(int id);


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="GetWhereAsync"]' />
    public Task<ResponseData<IEnumerable<T>>> GetWhereAsync(Expression<Func<T, bool>> predicate);


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="FirstOrDefaultAsync"]/first' />
    public Task<ResponseData<T>> FirstOrDefaultAsync();

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="FirstOrDefaultAsync"]/second' />
    public Task<ResponseData<T>> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="AddAsync"]' />
    public Task AddAsync(T entity);


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="UpdateAsync"]' />
    public Task UpdateAsync(T entity);

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="UpdateByIdAsync"]' />
    public Task UpdateByIdAsync(int id, Func<T, T> replacement);


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="DeleteByIdAsync"]' />
    public Task DeleteByIdAsync(int id);

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="ClearAsync"]' />
    public Task ClearAsync();
}