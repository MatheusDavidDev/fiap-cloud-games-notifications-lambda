using FCG.Contracts;

namespace NotificationsLambda.Interfaces;

public interface INotificationService
{

    Task ProcessUserCreatedAsync(UserCreatedEvent @event, string eventId);

    Task ProcessPaymentProcessedAsync(PaymentProcessedEvent @event, string eventId);
}
