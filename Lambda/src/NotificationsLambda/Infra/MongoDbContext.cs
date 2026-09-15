using MongoDB.Driver;
using NotificationsLambda.Models;

namespace NotificationsLambda.Infra;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    public IMongoCollection<Notificacao> Notificacoes { get; }

    public MongoDbContext(IMongoClient mongoClient, string databaseName)
    {
        _database = mongoClient.GetDatabase(databaseName);

        Notificacoes = _database.GetCollection<Notificacao>("notificacoes");

        CreateIndexes();
    }

    private void CreateIndexes()
    {
        var indexKeys = Builders<Notificacao>.IndexKeys.Ascending(x => x.EventId);

        var indexOptions = new CreateIndexOptions
        {
            Unique = true,
            Name = "ux_notifications_event_id"
        };

        var indexModel = new CreateIndexModel<Notificacao>(indexKeys,indexOptions);

        Notificacoes.Indexes.CreateOne(indexModel);
    }
}