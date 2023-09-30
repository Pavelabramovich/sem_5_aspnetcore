﻿using BookShop.Domain.Entities;
using BookShop.Domain.Models;

namespace BookShop.Services.EntityService;


public abstract class EntityService<T> : IEntityService<T> where T : Entity
{
    public abstract Task<ResponseData<IEnumerable<T>>> GetAllAsync();


    public abstract Task<ResponseData<T?>> GetByIdAsync(int id);


    public abstract Task<ResponseData<IEnumerable<T>>> GetWhereAsync(Func<T, bool> predicate);

    public abstract Task<ResponseData<IEnumerable<T>>> GetWhereAsync(IEnumerable<Func<T, bool>> predicates);


    public abstract Task<ResponseData<T?>> FirstOrNullAsync();

    public abstract Task<ResponseData<T?>> FirstOrNullAsync(Func<T, bool> predicate);

    public abstract Task<ResponseData<T?>> FirstOrNullAsync(IEnumerable<Func<T, bool>> predicates);


    public abstract Task UpdateByIdAsync(int id, T entity);

    public abstract Task UpdateByIdAsync(int id, Action<T> replacement);

    public abstract Task UpdateByIdAsync(int id, IEnumerable<Action<T>> replacements);


    public abstract Task DeleteByIdAsync(int id);


    public abstract Task ClearAsync(int id);
}