using Amazon.Lambda.Core;
using Amazon.Lambda.MQEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NotificationsLambda;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and returns both the upper and lower case version of the string.
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging Repositoriesand describing the Lambda environment.</param>
    /// <returns></returns>
    public Task  FunctionHandler(RabbitMQEvent input)
    {
        foreach (var messages in input.RmqMessagesByQueue)
        {
            Console.WriteLine($"Queue: {messages.Key}");

            foreach (var mensagem in messages.Value)
            {
                Console.WriteLine($"Mensagem recebida: {mensagem}");
            }
        }

        return Task.CompletedTask;
    }
}
