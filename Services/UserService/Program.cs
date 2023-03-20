using ELK;
using MessageBus;
using MessageBus.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.AddElasticSearch();
builder.Services.AddMessageBus<MassTransitConsumer>(builder.Environment.IsDevelopment(), "order-created-event", "Order");

builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.WithOrigins("http://127.0.0.1:5555");
    x.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
