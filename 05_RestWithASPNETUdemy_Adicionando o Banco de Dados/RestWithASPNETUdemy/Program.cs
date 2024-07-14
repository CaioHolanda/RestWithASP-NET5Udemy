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
// Registra o serviço IPersonService no contêiner de dependências, especificando que PersonServiceImplementation deve ser usado quando IPersonService for solicitado. AddScoped indica que uma nova instância do serviço será criada para cada solicitação HTTP.
builder.Services.AddScoped<IPersonService,PersonServiceImplementation>();

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
