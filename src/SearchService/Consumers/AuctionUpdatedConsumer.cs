using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine($"Consuming Auction Updated: {context.Message.Id}");

        var item = _mapper.Map<Item>(context.Message);

        var result = await DB.Update<Item>()
       .Match(i => i.ID == context.Message.Id)
       .ModifyOnly(i => new
       {
           i.Make,
           i.Model,
           i.Year,
           i.Color
       }, item).ExecuteAsync();

        if (!result.IsAcknowledged) throw new MessageException(typeof(AuctionUpdated), "Update failed");
    }
}
