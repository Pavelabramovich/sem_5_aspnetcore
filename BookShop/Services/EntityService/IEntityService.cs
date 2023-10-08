using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Services.EntityService;


public interface IEntityService<T> where T : Entity
{
    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="GetAllAsync"]' />
    public Task<ResponseData<List<T>>> GetAllAsync();


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="GetByIdAsync"]' />
    public Task<ResponseData<T?>> GetByIdAsync(int id);


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="GetWhereAsync"]/first' />
    public Task<ResponseData<IEnumerable<T>>> GetWhereAsync(Func<T, bool> predicate);

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="GetWhereAsync"]/second' />
    public Task<ResponseData<IEnumerable<T>>> GetWhereAsync(IEnumerable<Func<T, bool>> predicates);


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="FirstOrNullAsync"]/first' />
    public Task<ResponseData<T?>> FirstOrNullAsync();

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="FirstOrNullAsync"]/second' />
    public Task<ResponseData<T?>> FirstOrNullAsync(Func<T, bool> predicate);

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="FirstOrNullAsync"]/third' />
    public Task<ResponseData<T?>> FirstOrNullAsync(IEnumerable<Func<T, bool>> predicates);


    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="UpdateByIdAsync"]/first' />
    public Task UpdateByIdAsync(int id, T entity);

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="UpdateByIdAsync"]/second' />
    public Task UpdateByIdAsync(int id, Action<T> replacement);

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="UpdateByIdAsync"]/third' />
    public Task UpdateByIdAsync(int id, IEnumerable<Action<T>> replacements);


    public Task AddAsync(T entity);

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="DeleteByIdAsync"]' />
    public Task DeleteByIdAsync(int id);

    /// <include file='IEntityService.cs.xml' path='doc/class[@name="IEntityService"]/method[@name="ClearAsync"]' />
    public Task ClearAsync(int id);
}
