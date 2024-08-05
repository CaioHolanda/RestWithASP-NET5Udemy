using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Business.Implementations;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Repository.Implementations;
using EvolveDb;
using Serilog;
using System.Net.Http.Headers;
using RestWithASPNETUdemy.Hypermedia.Filters;
using RestWithASPNETUdemy.Hypermedia.Enricher;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var appName = "Rest API's RESTfull from 0 to Azure with ASP.NET 8 and Docker";
var appVersion = "v1";
var appDescription = $"REST API RESTfull developed in course '{appName}'";

builder.Services.AddRouting(options=>options.LowercaseUrls=true);
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(appVersion, new OpenApiInfo
    {
        Title = appName,
        Version = appVersion,
        Description = appDescription,
        Contact = new OpenApiContact { Name = "Caio Holanda", Url = new Uri("https://github.com/CaioHolanda") }
    });
});
builder.Services.AddApiVersioning();
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 3, 0))));

//Evolve setup
if (builder.Environment.IsDevelopment())
{
    MigrateDatabase(connection);
}

//Configuracao para Content Negotiation (diferentes formatos providos... json, xml, etc)
builder.Services.AddMvc(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml").ToString());
    options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json").ToString());


}).AddXmlSerializerFormatters();

var filterOptions = new HyperMediaFilterOptions();
filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
builder.Services.AddSingleton(filterOptions);
builder.Services.AddApiVersioning();

// Injecao de dependencia
builder.Services.AddScoped<IPersonBusiness,PersonBusinessImplementation>();
builder.Services.AddScoped<IBookBusiness,BookBusinessImplementation>();
builder.Services.AddScoped(typeof(IRepository<>),typeof(GenericRepository<>));


//Cria uma instância de WebApplication, que é a aplicação configurada que será executada.
var app = builder.Build();

// Configure the HTTP request pipeline.
//Adiciona um middleware para redirecionar todas as requisições HTTP para HTTPS, aumentando a segurança.
app.UseHttpsRedirection();
app.UseCors();

// Configuracoes para o Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/{appVersion}/swagger.json", $"{appName}-{appVersion}");
});
var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

//Adiciona o middleware de autorização ao pipeline de requisições HTTP, garantindo que as políticas de autorização sejam aplicadas.
app.UseAuthorization();

//Configura o roteamento para que os controladores sejam usados para processar as requisições HTTP. Ele mapeia as rotas definidas nos controladores.
app.MapControllers();
app.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiversion}/{id?}");

//Inicia a aplicação, colocando-a em execução e começando a ouvir as requisições HTTP.
app.Run();


//Evolve setup - continuacao
void MigrateDatabase(string connection)
{
    try
    {
        var evolveConnection = new MySqlConnection(connection);
        var evolve = new Evolve(evolveConnection, Log.Information)
        {
            Locations = new List<string> { "db/migrations", "db/dataset" },
            IsEraseDisabled = true,
        };
        evolve.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error("Database migration failed", ex);
        throw;
    }
}