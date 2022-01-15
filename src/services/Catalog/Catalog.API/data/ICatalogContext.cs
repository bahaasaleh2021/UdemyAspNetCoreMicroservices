using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
