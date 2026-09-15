namespace NotificationsLambda.Models;

public class Notificacao
{
    public Notificacao(string eventId, Guid idUsuario, string mensagem, string tipoNotificação)
    {
        Id = Guid.NewGuid().ToString();
        EventId = eventId;
        IdUsuario = idUsuario.ToString();
        Mensagem = mensagem;
        TipoNotificação = tipoNotificação;
        CreatedAt = DateTime.UtcNow;
    }

    public string Id { get; private set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public string IdUsuario { get; private set; } = string.Empty;
    public string Mensagem { get; private set; } = string.Empty;
    public string TipoNotificação { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
}
