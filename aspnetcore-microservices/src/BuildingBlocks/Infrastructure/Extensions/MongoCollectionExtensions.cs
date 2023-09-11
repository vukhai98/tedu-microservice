using MongoDB.Driver;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class MongoCollectionExtensions
    {
        public static Task<PageList<TDestinnation>> PaginatedListAsync<TDestinnation>(
            this IMongoCollection<TDestinnation> collection, FilterDefinition<TDestinnation> filter, int pageNumber, int pageSize) where TDestinnation : class
        {
            return PageList<TDestinnation>.ToPagedList(collection, filter, pageNumber, pageSize);
        }
    }
}
