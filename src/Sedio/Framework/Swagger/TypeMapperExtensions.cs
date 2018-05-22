using System.Collections.Generic;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace Sedio.Server.Framework.Swagger
{
    public static class TypeMapperExtensions
    {
        public static ICollection<ITypeMapper> MapTo<T>(this ICollection<ITypeMapper> typeMappers,JsonObjectType type = JsonObjectType.String)
        {
            typeMappers.Add(new PrimitiveTypeMapper(typeof(T),schema => schema.Type = type));
            return typeMappers;
        }
    }
}