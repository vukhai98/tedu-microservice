﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Mappings
{
    public class MappingProfile : Profile
    { 
        public MappingProfile() 
        {
            AppplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }
        private void AppplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);

            const string mappingMethodName = nameof(IMapFrom<object>.Mapping);

            bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

            var types = assembly.GetExportedTypes()
                                .Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

            var argumentTypes = new Type[] { typeof(Profile) };

            foreach ( var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(mappingMethodName);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
                else
                {
                    var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                    if (interfaces.Count <= 0)
                    {
                        continue;
                    }

                    foreach (var @interface in interfaces)
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }

        }
    }
}
