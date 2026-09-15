using Amazon.Lambda.Core;
using Amazon.Lambda.MQEvents;
using FCG.Contracts;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NotificationsLambda.Infra;
using NotificationsLambda.Interfaces;
using NotificationsLambda.Repositories;
using NotificationsLambda.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NotificationsLambda;

public class Function
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly JsonSerializerOptions JsonOptions = 
        new(){
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

    public Function()
    {
        var services = new ServiceCollection();

        var mongoConnectionString =Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") 
            ?? throw new InvalidOperationException("MONGODB_CONNECTION_STRING não configurada.");

        var mongoDatabase =Environment.GetEnvironmentVariable("MONGODB_DATABASE") 
            ?? "fiap-cloud-games";

        services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

        services.AddSingleton<MongoDbContext>(provider =>new MongoDbContext(provider.GetRequiredService<IMongoClient>(),mongoDatabase));

        services.AddScoped<INotificationRepository,NotificationRepository>();

        services.AddScoped<INotificationService,NotificationService>();

        _serviceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    /// A simple function that takes a string and returns both the upper and lower case version of the string.
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging Repositoriesand describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandler(RabbitMQEvent input)
    {
        using var scope = _serviceProvider.CreateScope();

        var notificationService = scope.ServiceProvider .GetRequiredService<INotificationService>();

        foreach (var messages in input.RmqMessagesByQueue)
        {
            Console.WriteLine($"Queue: {messages.Key}");

            foreach (var mensagem in messages.Value)
            {
                Console.WriteLine("Mensagem recebida.");

                await ProcessMessageAsync(mensagem.Data, notificationService);
            }
        }

        await Task.CompletedTask;
    }

    private static async Task ProcessMessageAsync(string data, INotificationService notificationService)
    {
        var jsonBytes = Convert.FromBase64String(data);

        var json = Encoding.UTF8.GetString(jsonBytes);

        Console.WriteLine($"Mensagem decodificada: {json}");

        using var document = JsonDocument.Parse(json);

        var root = document.RootElement;

        if (!root.TryGetProperty("messageId", out var messageIdProperty))
        {
            Console.WriteLine("messageId não encontrado.");
            return;
        }

        var eventId = messageIdProperty.GetString();

        if (string.IsNullOrWhiteSpace(eventId))
        {
            Console.WriteLine("messageId inválido.");
            return;
        }

        if (!root.TryGetProperty("messageType", out var messageTypes))
        {
            Console.WriteLine("messageType não encontrado.");
            return;
        }

        var eventType = messageTypes.EnumerateArray()
            .Select(x => x.GetString())
            .FirstOrDefault(x =>x?.Contains(nameof(UserCreatedEvent)) == true ||
             x?.Contains(nameof(PaymentProcessedEvent)) == true);

        if (eventType is null)
        {
            Console.WriteLine("Evento não suportado.");
            return;
        }

        if (!root.TryGetProperty("message",out var message))
        {
            Console.WriteLine("message não encontrado.");
            return;
        }

        if (eventType.Contains(nameof(UserCreatedEvent)))
        {
            var userCreatedEvent =message.Deserialize<UserCreatedEvent>(JsonOptions);

            if (userCreatedEvent is not null)
            {
                await notificationService.ProcessUserCreatedAsync(userCreatedEvent,eventId);
            }

            return;
        }

        if (eventType.Contains(nameof(PaymentProcessedEvent)))
        {
            var paymentProcessedEvent =message.Deserialize<PaymentProcessedEvent>(JsonOptions);

            if (paymentProcessedEvent is not null)
            {
                await notificationService.ProcessPaymentProcessedAsync(paymentProcessedEvent,eventId);
            }
        }
    }
}
