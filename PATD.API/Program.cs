using BancoPopular.Infraestructura.Docker;
using BancoPopular.Servicios.Servicio;
using BancoPopular.Solicitudes.Solicitud;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PATD.API.Application.AdminTarjeta;
using PATD.API.Application.Gestiones;
using PATD.API.Application.Notificaciones;
using PATD.API.Application.Seguridad;
using PATD.API.DataAccess.Gestiones;
using PATD.API.DataAccess.LogDA;
using PATD.API.DataAccess.Seguridad;
using PATD.API.DataAccess.Volcan;
using PATD.API.Transversal.Helper;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization (bearer)",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TokenKeyPATD"))),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });
builder.Services.AddHttpClient();
builder.Services.AddTransient<IInfraestructuraDocker, InfraestructuraDocker>();
builder.Services.AddTransient<ISolicitud, Solicitud>();
builder.Services.AddTransient<ISeguridadDA, SeguridadDA>();
builder.Services.AddTransient<ISeguridadApp, SeguridadApp>();
builder.Services.AddTransient<IVolcanDA, VolcanDA>();
builder.Services.AddTransient<IGestionesDA, GestionesDA>();
builder.Services.AddTransient<IGestionesAPP, GestionesAPP>();
builder.Services.AddTransient<INotificacionesAPP, NotificacionesAPP>();
builder.Services.AddTransient<IServicio, Servicio>();
builder.Services.AddTransient<IHelper,Helper>();
builder.Services.AddTransient<ILogDA, LogDA>();
builder.Services.AddTransient<IAdminTarjeta, AdminTarjeta>();

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

app.Run();
