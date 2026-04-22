using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

// Web API uygulamasının temel yapılandırmasını (Builder) oluşturur
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers() // API kontrolcülerini oluşturur
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// PostgreSQL veritabanı bağlantısını sisteme ekler
builder.Services.AddDbContext<FinansDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=loanapp;Username=postgres;Password=201079"));

builder.Services.AddEndpointsApiExplorer();

// Swagger yapılandırmasını başlatır.
builder.Services.AddSwaggerGen(c =>
{
    // Swagger dökümanının başlığını ve versiyon bilgisini tanımlar.
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Finans API", Version = "v1" });

    // Swagger UI'da JWT (JSON Web Token) girişini desteklemek için bir güvenlik tanımı ekler.
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", // HTTP Header'da gönderilecek parametre adı (Authorization).
        Type = SecuritySchemeType.Http, // Güvenlik tipinin HTTP protokolü olduğunu belirtir.
        Scheme = "Bearer", // Kullanılacak kimlik doğrulama şeması (Bearer token).
        BearerFormat = "JWT", // Token formatının JWT olduğunu Swagger'a bildirir.
        In = ParameterLocation.Header, // Kimlik bilgisinin Header içinde aranacağını belirtir.
        Description = "Token değerini girin." // Kullanıcıya gösterilecek açıklama metni.
    });

    // Tanımladığımız bu güvenlik şemasını (Bearer) tüm API uçları (endpoints) için zorunlu kılar.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Yukarıdaki SecurityDefinition'da verilen "Bearer" ismiyle eşleşmeli.
                }
            },
            Array.Empty<string>() // Kapsam (scope) belirtmeye gerek olmadığı için boş bir dizi geçer.
        }
    });
});

// CORS (Cross-Origin Resource Sharing) politikasını yapılandırır.
builder.Services.AddCors(options =>
{
    // Angular uygulaması gibi farklı bir adresten (Origin) gelen isteklere izin verir.
    options.AddPolicy("AllowAngular",
        builder => builder
            .AllowAnyOrigin()   // Herhangi bir web adresinden gelen isteğe izin verir.
            .AllowAnyMethod()   // GET, POST, PUT, DELETE gibi tüm HTTP metodlarına izin verir.
            .AllowAnyHeader()); // Tüm HTTP header (başlık) bilgilerine izin verir.
});

// appsettings.json dosyasından JWT için gereken gizli anahtarı (Secret Key) okur.
var jwtKey = builder.Configuration["Jwt:Key"] 
    ?? throw new InvalidOperationException("JWT Key bulunamadı."); // Anahtar yoksa uygulama hata verip durur.

// String formatındaki anahtarı, şifreleme işlemlerinde kullanılmak üzere byte dizisine çevirir.
var key = Encoding.ASCII.GetBytes(jwtKey);

// JWT ile kimlik doğrulama (Authentication) servisini sisteme kaydeder
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Https zorunluluğunu kaldırır
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Token imzasını doğrular
        IssuerSigningKey = new SymmetricSecurityKey(key), // Anahtarı belirtir
        ValidateIssuer = false, // Doğru olup olmadığına bakar
        ValidateAudience = false // Hangi uygulamada geçerli olup olmadığına bakar.
    };
});

var app = builder.Build(); // Çalışan bir web uygulaması nesnesi oluşturur.

// Uygulama çalışmadan önce veritabanındaki tabloları oluşturur ve örnek verileri (seed) ekler
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinansDbContext>();
    context.Database.EnsureCreated();
    DbSeeder.Seed(context);
}

if (app.Environment.IsDevelopment())
{
    // Swagger JSON dosyasını oluşturur (arka planda çalışır)
    app.UseSwagger();
    // Swagger arayüzünü (HTML/JS) tarayıcıda sunar
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // API kontrolcülerini yönlendirir.

app.Run(); // .NET'i ayağa kaldırır.