using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Business.Implementations;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Repository.Implementations;
using EvolveDb;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning();
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 3, 0))));

//Evolve setup
if (builder.Environment.IsDevelopment())
{
    MigrateDatabase(connection);
}

// Injecao de dependencia
builder.Services.AddScoped<IPersonBusiness,PersonBusinessImplementation>();
builder.Services.AddScoped<IPersonRepository,PersonRepositoryImplementation>();
builder.Services.AddScoped<IBookBusiness,BookBusinessImplementation>();
builder.Services.AddScoped<IBookRepository,BookRepositoryImplementation>();


//Cria uma instância de WebApplication, que é a aplicação configurada que será executada.
var app = builder.Build();

// Configure the HTTP request pipeline.
//Adiciona um middleware para redirecionar todas as requisições HTTP para HTTPS, aumentando a segurança.
app.UseHttpsRedirection();

//Adiciona o middleware de autorização ao pipeline de requisições HTTP, garantindo que as políticas de autorização sejam aplicadas.
app.UseAuthorization();

//Configura o roteamento para que os controladores sejam usados para processar as requisições HTTP. Ele mapeia as rotas definidas nos controladores.
app.MapControllers();

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