using ApiPokemon.Data;
using ApiPokemon.Services;
using Microsoft.EntityFrameworkCore;


// Se crea un builder para la aplicacion web
var builder = WebApplication.CreateBuilder(args);

// Se configura la aplicacion para que pueda acceder a la base de datos sqlserver
builder.Services.AddDbContext<PokemonContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    })
.EnableSensitiveDataLogging() // Se activa el logging de datos sensibles
.EnableDetailedErrors() // Se activan los errores detallados
.UseLazyLoadingProxies() // Se activan los proxies de carga diferida
);
// Se a�ade la politica de CORS para permitir el acceso desde el frontend
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowMyFrontend",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200") 
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });



// Se a�aden los servicios necesarios para crear la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<LoadDataService>(); // Se a�ade el servicio de carga de datos
builder.Services.AddHttpClient(); // Se a�ade el cliente http para hacer peticiones a la API pokeapi
builder.Services.AddLogging(); // Se a�ade el servicio de logging

// Se construye la aplicacion con los servicios y la configuracion
var app = builder.Build();

// Las operaciones que se hacen aqui se corresponden con el metodo main fuera de la logica de peticiones http

using (var scope = app.Services.CreateScope()) // Se crea un scope para poder usar el servicio de acceso a la base de datos
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>(); // Obtenemos el logger para mostrar mensajes
    try
    {
        var context = services.GetRequiredService<PokemonContext>(); // Obtenemos el contexto de la base de datos
        var client = services.GetRequiredService<IHttpClientFactory>().CreateClient(); // Creamos un cliente http para hacer las peticiones a la API pokeapi

        // Obtenemos el servicio de carga de datos
        //var loadDataService = services.GetRequiredService<LoadDataService>();

        // Creación de la base de datos
        // context.Database.EnsureDeleted(); // Borramos la base de datos si existe
        // context.Database.EnsureCreated(); // Creamos la base de datos

        // Estrategia de ejecución
        //var strategy = context.Database.CreateExecutionStrategy();

        // Ejecuta las operaciones dentro de la estrategia de ejecución
        //await strategy.ExecuteAsync(async () =>
        //{
        //    using (var transaction = await context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            // Carga de datos
        //            //await loadDataService.LoadTypes();
        //            //await loadDataService.LoadCategories();
        //            //await loadDataService.LoadAbilities();
        //            //await loadDataService.LoadMoves();
        //            //await loadDataService.LoadPokemons();
        //            //await loadDataService.LoadEgggroups();
        //            //await loadDataService.LoadPics();

        //            // Confirma la transacción
        //            await transaction.CommitAsync();
        //        }
        //        catch
        //        {
        //            // Si ocurre un error, revierte la transacción
        //            await transaction.RollbackAsync();
        //            throw;
        //        }
        //    }
        //});
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while accessing the database.");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMyFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();

