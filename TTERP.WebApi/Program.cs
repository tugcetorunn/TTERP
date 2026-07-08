using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TTERP.Application.CQRS.Announcements.Handlers;
using TTERP.Application.Validators;
using TTERP.Domain.Entities;
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

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();

// mapster ýn global konfigürasyonunu alýyoruz (eðer özel eþleþtirmeler yazarsak buraya eklenecek)
var config = TypeAdapterConfig.GlobalSettings;
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("TTERP.Persistence")));

builder.Services.AddIdentity<Employee, Role>(x => x.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// yapýlandýrma ayarlarýný okuma
// authorization dll projesinde tanýmladýðýmýz bu bilgileri bu projede de configuration dan çekmek için burada tanýmlýyoruz.
// dll projesinde configuration class ýný inject etmiþtik fakar burasý program.cs, burada class method yapýsý olmadýðý için builder burada bunu saðlayacak. 
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]; // bilgilerin þifrelenmesi bu key yapýsýna göre olur. minimum 16 karakter olmalý
var issuer = jwtSettings["Issuer"]; // token ý oluþturan sunucu
var audience = jwtSettings["Audience"]; // token ý kullanacak kiþi, uygulama, site. örneðin bir sitenin kullanýmýna özel bir api projemiz varsa buraya yazarýz.
var expireMinutes = int.Parse(jwtSettings["ExpireMinutes"]!); // api yi tüketme eriþim süresi (dk)
// burada tanýmlamasak da diðer projede tanýmladýðýmýz yerlerden çeker mi teste et...

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // kimlik doðrulamasý için bu þemayý eklememiz gerekiyor.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        // true false atamalarý ihtiyaçlarýmýzý belirler.
        // bu kýsýmlar da core tarafýna taþýnabilir. deðerler verilmiþse true verilmemiþse false döner.
        ValidateIssuer = true, // token ý oluþturan taraf, güvenlik açýsýndan true yaparýz.
        ValidateAudience = false, // token ýn hedef kitlesini ayarlarýz. (apilerimizi kim tüketecek?)
        ValidateLifetime = true, // token süresini kontrol eder. bunlarý yazmazsak default deðer atar.
        ValidateIssuerSigningKey = true, // token ý imzalamak için kullanýlan anahtarýn doðruluðunu kontrol eder. (secretkey) güvenliði saðlamak için zorunlu alandýr.
        ValidIssuer = issuer, // yukarýda tanýmladýk, sonra validasyonunu yaptýk ve artýk geçerli issuer olarak atýyoruz.
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)), // token imzalanmasý için kullanýlan key i veriyoruz. token ýn geçerli olup olmadýðýný kontrol eder.
        ClockSkew = TimeSpan.FromMinutes(30) // token ýn süresi dolduðunda bir miktar daha esneklik saðlar 5 dk ayarlandý.
    };
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Frontend bu adrese baðlanacak (Örn: localhost:5000/notification-hub)
app.MapHub<NotificationHub>("/notification-hub");

app.UseCors("AllowReact");

app.Run();
