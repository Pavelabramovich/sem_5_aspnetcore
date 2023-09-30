using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Domain.Models;

public class ResponseData<T>
{
    public T Data { get; init; }

    public string? ErrorMessage { get; init; }


    public ResponseData(T data, string? errorMessage = null)
    {
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static implicit operator bool(ResponseData<T> response)
    {
        return response.ErrorMessage is null;
    }
}
