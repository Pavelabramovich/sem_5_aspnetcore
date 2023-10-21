using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Domain.Models;

[Serializable]
public class JsonResponseViewModel
{
    public int ResponseCode { get; set; }

    public string ResponseMessage { get; set; } = string.Empty;
}
