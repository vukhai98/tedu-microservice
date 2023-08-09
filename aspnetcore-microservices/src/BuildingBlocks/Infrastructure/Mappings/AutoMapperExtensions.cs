using AutoMapper;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappings
{
    public static class AutoMapperExtensions
    {
        // Nếu khi update nó sẽ chạy qua các property nếu mà thằng nào ko có nó sẽ bỏ qua ko map
        public static IMappingExpression<TSource, IDestination> IgnoreAllNonExisting<TSource, IDestination>(this IMappingExpression<TSource, IDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(IDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                    expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }
    }
}
