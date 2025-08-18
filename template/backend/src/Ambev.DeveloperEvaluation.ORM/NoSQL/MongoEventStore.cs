using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

public class MongoEventStore : IEventStore
{
    private readonly IMongoCollection<BsonDocument> _collection;
    public MongoEventStore(IOptions<MongoSettings> settings)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        var mongoSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);
        var client = new MongoClient(mongoSettings);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<BsonDocument>("DomainEvents");
    }

    public async Task SaveAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        var doc = @event.ToBsonDocument();
        await _collection.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }
}

