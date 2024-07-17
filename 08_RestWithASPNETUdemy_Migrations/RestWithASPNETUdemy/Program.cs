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


//Cria uma inst�ncia de WebApplication, que � a aplica��o configurada que ser� executada.
var app = builder.Build();

// Configure the HTTP request pipeline.
//Adiciona um middleware para redirecionar todas as requisi��es HTTP para HTTPS, aumentando a seguran�a.
app.UseHttpsRedirection();

//Adiciona o middleware de autoriza��o ao pipeline de requisi��es HTTP, garantindo que as pol�ticas de autoriza��o sejam aplicadas.
app.UseAuthorization();

//Configura o roteamento para que os controladores sejam usados para processar as requisi��es HTTP. Ele mapeia as rotas definidas nos controladores.
app.MapControllers();

//Inicia a aplica��o, colocando-a em execu��o e come�ando a ouvir as requisi��es HTTP.
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