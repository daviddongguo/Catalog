using David.Catalog.Service.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;

namespace David.Common.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, string connectionString, string serviceName)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(serviceProvider =>
                {
                    var mongoClient = new MongoClient(connectionString);
                    return mongoClient.GetDatabase(serviceName);
                });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                if (database != null)
                {
                    return new MongoRepository<T>(database, collectionName);
                }
                throw new Exception("Error Connecting Database");
            });

            services.AddSingleton<IReadRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                if (database != null)
                {
                    return new MongoReadRepository<T>(database, collectionName);
                }
                throw new Exception("Error Connecting Database");
            });

            return services;
        }
    }
}
