﻿using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IConfiguration _configuration;

        public  CatalogContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration["DatabaseSettings:ConnectionString"]);
            var dataBase = client.GetDatabase(_configuration["DatabaseSettings:DatabaseName"]);
            Products=dataBase.GetCollection<Product>(_configuration["DatabaseSettings:CollectionName"]);
             CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products  { get; }
    }
}
