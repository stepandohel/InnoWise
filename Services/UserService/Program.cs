using ELK;
using MassTransit;
using MessageBus;
using MessageBus.DI;
using RabbitMQ.Client;
using SignalRLib.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.AddElasticSearch();
builder.Services.AddMessageBus();
builder.Services.AddSignalR();

//await MessageBusExtansion.GetBusControl("user-created-event", "Order").StartAsync(new CancellationToken());

//await MessageBusExtansion.GetBusControlSignalR("order-created-event", "User").StartAsync(new CancellationToken());

//var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
//{
//    cfg.ReceiveEndpoint("order-created-event", e =>
//    {
//        e.ConfigureConsumeTopology = false;
//        e.Bind<People>(x =>
//        {
//            x.ExchangeType = ExchangeType.Direct;
//            x.RoutingKey = "User";
//        });
//       // e.Consumer<SignalRConsumer>();
//        e.ConfigureConsumer<SignalRConsumer>(cfg);
//    });
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.WithOrigins("http://127.0.0.1:5500");
    x.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<PeopleHub>("/peopleHub");


app.MapControllers();


app.Run();
