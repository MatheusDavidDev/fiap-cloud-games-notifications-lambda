using FCG.Contracts;
using Microsoft.Extensions.Logging;
using NotificationsLambda.Interfaces;
using NotificationsLambda.Models;

namespace NotificationsLambda.Services;

public class NotificationService : INotificationService
{

    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task ProcessPaymentProcessedAsync(PaymentProcessedEvent @event, string eventId)
    {
        var notification = new Notificacao(
            eventId,
            @event.IdUsuario,
            $"Pagamento do pedido {@event.IdOrdemCompra} " +
            $"processado com status: {@event.Status}.",
            nameof(PaymentProcessedEvent));

        var created = await _repository.CreateAsync(notification);

        if (!created)
        {
            Console.WriteLine(
                $"Evento {eventId} já processado.");
        }
    }

    public async Task ProcessUserCreatedAsync(UserCreatedEvent @event, string eventId)
    {
        var notification = new Notificacao(
            eventId,
            @event.IdUsuario,
            $"Olá {@event.Nome}, bem-vindo(a) ao FIAP Cloud Games!",
            nameof(UserCreatedEvent));

        var created = await _repository.CreateAsync(notification);

        if (!created)
        {
            Console.WriteLine(
                $"Evento {eventId} já processado.");
        }
    }
}
