using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using CorporateIdentityManager.Persistence.Context;
using CorporateIdentityManager.Application.Services;
var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ActiveDirectoryDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    ));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Simulaçao API AZURE",
        Version = "Simulação",
        Description = "Enterprise Azure Entra ID / Active Directory / Intune Simulation"
    });
});
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<OrganizacaoService>();
builder.Services.AddScoped<GrupoService>();
builder.Services.AddScoped<LicenciamentoService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

#endregion

var app = builder.Build();

#region Middleware Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();