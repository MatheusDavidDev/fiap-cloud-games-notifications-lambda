using NotificationsLambda.Models;

namespace NotificationsLambda.Interfaces;

public interface INotificationRepository
{
    Task<bool> CreateAsync(Notificacao notificacao);
}
