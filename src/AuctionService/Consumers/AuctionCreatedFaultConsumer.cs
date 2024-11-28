using System;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine($"Consuming faulty auction: {context.Message.Message.Id}");
        var exception = context.Message.Exceptions.First();

        Console.WriteLine($"Exception: {exception.Message}");
        return Task.CompletedTask;
    }
}
