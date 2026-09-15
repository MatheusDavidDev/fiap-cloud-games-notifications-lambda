using MongoDB.Driver;
using NotificationsLambda.Infra;
using NotificationsLambda.Interfaces;
using NotificationsLambda.Models;

namespace NotificationsLambda.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly MongoDbContext _context;

    public NotificationRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(Notificacao notificacao)
    {
        try
        {
            await _context.Notificacoes.InsertOneAsync(notificacao);

            return true;
        }
        catch (MongoWriteException ex) 
            when (ex.WriteError?.Category ==ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}

