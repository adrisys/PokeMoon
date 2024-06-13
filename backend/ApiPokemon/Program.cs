using ApiPokemon.Data;
using ApiPokemon.Services;
using Microsoft.EntityFrameworkCore;


// Se crea un builder para la aplicacion web
var builder = WebApplication.CreateBuilder(args);

// Se configura la aplicacion para que pueda acceder a la base de datos sqlserver
builder.Services.AddDbContext<PokemonContext>(options => options
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
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

using (var scope = app.Services.CreateScope())// Se crea un scope para poder usar el servicio de acceso a la base de datos
{
    var services = scope.ServiceProvider;  
    var logger = services.GetRequiredService<ILogger<Program>>();// Obtenemos el logger para mostrar mensajes
    try
    {
        var context = services.GetRequiredService<PokemonContext>(); // Obtenemos el contexto de la base de datos      
        var client = services.GetRequiredService<IHttpClientFactory>().CreateClient();// Creamos un cliente http para hacer las peticiones a la API pokeapi

        // Obtenemos el servicio de carga de datos
        var loadDataService = services.GetRequiredService<LoadDataService>(); //No hace falta declarar los parametros que necesita el servicio en un constructor ya que se cargan automaticamente por inyeccion de dependencias.

        // Creacion de la base de datos

        //context.Database.EnsureDeleted(); // Borramos la base de datos si existe
        //context.Database.EnsureCreated(); // Creamos la base de datos

        // Carga de datos

        //loadDataService.LoadTypes().Wait(); // Cargamos los tipos
        //loadDataService.LoadCategories().Wait(); // Cargamos las categorias
        //loadDataService.LoadAbilities().Wait(); // Cargamos las habilidades
        //loadDataService.LoadMoves().Wait(); // Cargamos los movimientos
        //loadDataService.LoadPokemons().Wait(); // Cargamos los pokemons
        //loadDataService.LoadEgggroups().Wait(); // Cargamos los grupos de huevos
        //loadDataService.LoadPics().Wait();

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

