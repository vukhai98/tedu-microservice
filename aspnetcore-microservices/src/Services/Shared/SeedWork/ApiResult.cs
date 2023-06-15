using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class ApiResult<T>
    {
        public bool IsSucced { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResult() 
        {

        }

        public ApiResult(bool isSucced, string message = null)
        {
            Message = message;
            IsSucced = isSucced;
        }

        public ApiResult(bool isSucced, T data, string message = null)

        {
            Message = message;
            IsSucced = isSucced;
            Data = data;
        }
    }
}
