using Microsoft.EntityFrameworkCore;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Business.Implementation;
using RestWithASPNETUdemy.Repository;
using MySqlConnector;
using EvolveDb;
using Serilog;
using RestWithASPNETUdemy.Repository.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApiVersioning();
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options=>options.UseMySql(connection,new MySqlServerVersion(new Version(8,3,0))));
// Executa as migrações
if (builder.Environment.IsDevelopment())
{
    MigrateDatabase(connection);
}


builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
builder.Services.AddScoped<IBookBusiness, BookBusinessImplementation>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
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
		Log.Error("Database migration failed",ex);	
		throw;
	}
    
}

 
