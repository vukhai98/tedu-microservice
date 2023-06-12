using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Exceptions
{
    public class IsValidEntityException : ApplicationException
    {
        public IsValidEntityException(string entity, Type type) : base($" Entity \"{entity}\"  not support type {type}")
        {

        }
    }
}
