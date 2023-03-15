using ELK;
using MessageBus;
using MessageBus.DI;
using SignalRLib.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Host.AddElasticSearch();
builder.Services.AddMessageBus<SignalRConsumer>(builder.Environment.IsDevelopment(), "user-created-event", "User");
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

app.MapHub<PeopleHub>("/peopleHub");

app.MapControllers();

app.Run();
