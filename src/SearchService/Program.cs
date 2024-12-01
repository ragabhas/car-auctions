using MassTransit;
using SearchService.Consumers;
using SearchService.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x =>
{
  x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
  x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
  x.UsingRabbitMq((context, cfg) =>
  {
    cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
          h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
          h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

    cfg.ReceiveEndpoint("seatch-auction-created", e =>
    {
      e.UseMessageRetry(r => r.Interval(5, 1000));
      e.Consumer<AuctionCreatedConsumer>(context);
    });

    cfg.ReceiveEndpoint("seatch-auction-updated", e =>
    {
      e.UseMessageRetry(r => r.Interval(5, 1000));
      e.Consumer<AuctionUpdatedConsumer>(context);
    });

    cfg.ReceiveEndpoint("seatch-auction-deleted", e =>
    {
      e.UseMessageRetry(r => r.Interval(5, 1000));
      e.Consumer<AuctionDeletedConsumer>(context);
    });


    cfg.ConfigureEndpoints(context);
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await DbInitializer.InitDb(app);

app.Run();
