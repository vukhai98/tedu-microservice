using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class ApiSuccessedResult<T> : ApiResult<T>
    {
        [JsonConstructor]
        public ApiSuccessedResult(T data) : base(true, data)
        {

        }

        public ApiSuccessedResult(T data, string message) : base(true, data, message)
        {

        }
    }

}
