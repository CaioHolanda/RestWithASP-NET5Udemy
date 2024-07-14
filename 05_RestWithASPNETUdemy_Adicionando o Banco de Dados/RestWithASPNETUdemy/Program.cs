using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 3, 0))));


// Injecao de dependencia
// Registra o servi�o IPersonService no cont�iner de depend�ncias, especificando que PersonServiceImplementation deve ser usado quando IPersonService for solicitado. AddScoped indica que uma nova inst�ncia do servi�o ser� criada para cada solicita��o HTTP.
builder.Services.AddScoped<IPersonService,PersonServiceImplementation>();

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
