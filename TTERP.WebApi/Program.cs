using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TTERP.Application.CQRS.Announcements.Handlers;
using TTERP.Persistence.Contexts;
using TTERP.WebApi.Extensions;
using TTERP.WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAnnouncementQueryHandler).Assembly));

builder.Services.AddSignalR();

// mapster ýn global konfigürasyonunu alýyoruz (eðer özel eþleþtirmeler yazarsak buraya eklenecek)
var config = TypeAdapterConfig.GlobalSettings;
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("TTERP.Persistence")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Frontend bu adrese baðlanacak (Örn: localhost:5000/notification-hub)
app.MapHub<NotificationHub>("/notification-hub");

app.Run();
