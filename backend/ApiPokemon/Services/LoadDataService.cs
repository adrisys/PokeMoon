using ApiPokemon.Data;
using ApiPokemon.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;

namespace ApiPokemon.Services
{
    public class LoadDataService(PokemonContext context, HttpClient client)
    {

        public async Task LoadTypes()
        {
            var response = await client.GetAsync("https://pokeapi.co/api/v2/type");
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync(); // Obtenemos el json de la API
                JObject jsonObject = JObject.Parse(json); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                var results = jsonObject["results"].ToObject<List<JObject>>(); // Accedemos a la lista de tipos
                List<PokeType> data = []; // Lista de los tipos que se van a añadir a la base de datos

                //rellenamos la lista de tipos
                foreach (var result in results)
                {
                    try
                    {
                        data.Add(new PokeType { Typename = result["name"].ToString() }); // Creamos los objetos de tipo y los añadimos a la lista
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine("____________________Error al obtener los datos del tipo " + result["name"].ToString() + "____________________");
                        Console.WriteLine("________________________________________");
                    }
                }
                context.Types.AddRange(data); // Añadimos los datos a la base de datos
                await context.SaveChangesAsync(); // Guardamos los cambios en la base de datos
            }
            else
            {
                Console.WriteLine("Error en la llamada a la api PokeApi");
            }
        }

        public async Task LoadCategories()
        {
            context.AddRange(new Category { Category1 = "physical" }, new Category { Category1 = "special" }, new Category { Category1 = "status" }); // Añadimos las categorias a la base de datos
            await context.SaveChangesAsync(); // Guardamos los cambios en la base de datos

        }
        public async Task LoadAbilities()
        {
            var response = await client.GetAsync("https://pokeapi.co/api/v2/ability?limit=500");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(); // Obtenemos el json de la API
                var jsonObject = JObject.Parse(json); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                var results = jsonObject["results"].ToObject<List<JObject>>(); // Accedemos a la lista de habilidades
                List<Ability> data = []; // Lista de las habilidades que se van a añadir a la base de datos

                //rellenamos la lista de habilidades
                foreach (var result in results)
                {
                    var abilityDetailJson = await client.GetStringAsync(result["url"].ToString()); // Obtenemos los detalles de cada habilidad con otra llamada a la API
                    var abilityDetail = JObject.Parse(abilityDetailJson); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                    try
                    {
                        data.Add(new Ability { Abilityname = abilityDetail["name"]!.ToString() }); // Creamos los objetos de habilidad y los añadimos a la lista
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine("____________________Error al obtener los datos de la habilidad " + result["name"].ToString() + "____________________");
                        Console.Error.WriteLine(e.StackTrace);
                        Console.WriteLine("________________________________________");
                    }
                }
                context.Abilities.AddRange(data); // Añadimos los datos a la base de datos
                await context.SaveChangesAsync(); // Guardamos los cambios en la base de datos
            }
            else
            {
                Console.WriteLine("Error en la llamada a la api PokeApi");
            }
        }


        public async Task LoadMoves()
        {
            var response = await client.GetAsync("https://pokeapi.co/api/v2/move?limit=1000");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(); // Obtenemos el json de la API
                var jsonObject = JObject.Parse(json); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                var results = jsonObject["results"].ToObject<List<JObject>>(); // Accedemos a la lista de movimientos
                List<Move> data = []; // Lista de los movimientos que se van a añadir a la base de datos

                //rellenamos la lista de movimientos
                foreach (var result in results)
                {
                    var moveDetailJson = await client.GetStringAsync(result["url"].ToString()); // Obtenemos los detalles de cada movimiento con otra llamada a la API
                    var moveDetail = JObject.Parse(moveDetailJson); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                    try
                    {
                        // Buscamos el id del tipo en la base de datos, no conocemos el id asi que no podemos usar find directamente, usamos FirstOrDefault
                        var movetypename = moveDetail["type"]["name"].ToString();
                        if (movetypename.Equals("shadow")) movetypename = "ghost";
                        var dbType = context.Types.FirstOrDefault(t => t.Typename == movetypename);
                        if (dbType == null)
                        {
                            throw new Exception("No se ha encontrado el tipo del movimiento " + result["name"].ToString() + " en la base de datos");
                        }
                        // Buscamos el id de la categoria en la base de datos
                        var dbCategory = context.Categories.FirstOrDefault(c => c.Category1 == moveDetail["damage_class"]["name"].ToString());
                        if (dbCategory == null)
                        {
                            throw new Exception("No se ha encontrado la categoria del movimiento " + result["name"].ToString() + " en la base de datos");
                        }

                        var idFromType = dbType!.Idtype;
                        var idFromCategory = dbCategory!.Idcat;


                        context.Moves.Add(new Move
                        {
                            Movename = moveDetail["name"].ToString(),
                            Power = moveDetail["power"].Value<byte?>(),
                            Accuracy = moveDetail["accuracy"].Value<byte?>(),
                            Pp = moveDetail["pp"]!.Value<byte?>(),
                            TypeNavigation = dbType,
                            TypeID = idFromType,
                            CatNavigation = dbCategory,
                            CatID = idFromCategory
                        });
                        
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine("____________________Error al obtener los datos del movimiento " + result["name"].ToString() + "____________________");
                        Console.Error.WriteLine(e.Message);
                        Console.Error.WriteLine(e.StackTrace);
                        Console.WriteLine("________________________________________");
                    }
                    await context.SaveChangesAsync();

                }

                
            }
            else
            {
                Console.WriteLine("Error al obtener los datos de movimientos");
                Console.WriteLine("________________________________________");
            }
        }
        public async Task LoadPokemons()
        {
            var response = await client.GetAsync("https://pokeapi.co/api/v2/pokemon?limit=10000&offset=0");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(); // Obtenemos el json de la API
                var jsonObject = JObject.Parse(json); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                var results = jsonObject["results"].ToObject<List<JObject>>(); // Accedemos a la lista de pokemons
                var data = new List<Pokemon>(); // Lista de los pokemons que se van a añadir a la base de datos

                foreach (var result in results)
                {
                    var pokemonDetailJson = await client.GetStringAsync(result["url"].ToString()); // Obtenemos los detalles de cada pokemon con otra llamada a la API
                    var pokemonDetail = JObject.Parse(pokemonDetailJson); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                    if (pokemonDetail != null)
                    {
                        var PokeData = new Pokemon(); // Creamos un nuevo objeto Pokemon                      
                        PokeData.Pokename = pokemonDetail["name"].ToString(); // Asignamos el nombre del pokemon
                        // Asignamos los datos de los stats
                        PokeData.Hp = (byte)pokemonDetail["stats"][0]["base_stat"];
                        PokeData.Attack = (byte)pokemonDetail["stats"][1]["base_stat"];
                        PokeData.Defense = (byte)pokemonDetail["stats"][2]["base_stat"];
                        PokeData.Spattack = (byte)pokemonDetail["stats"][3]["base_stat"];
                        PokeData.Spdefense = (byte)pokemonDetail["stats"][4]["base_stat"];
                        PokeData.Speed = (byte)pokemonDetail["stats"][5]["base_stat"];
                        PokeData.Dualtype = pokemonDetail["types"].ToObject<List<JObject>>().Count > 1;
                        //tipos
                        foreach (var type in pokemonDetail["types"].ToObject<List<JObject>>())
                        {
                            var dbType = context.Types.FirstOrDefault(t => t.Typename == type["type"]["name"].ToString()); // Buscamos el objeto tipo en la base de datos correspondiente al nombre
                            if (dbType != null)
                            {
                                PokeData.Idtypes.Add(dbType); // Añadimos el tipo al pokemon
                            }
                            else
                            {
                                Console.WriteLine("____________________No se han encontrado algun tipo del pokemon " + result["name"] + " en la base de datos____________________");
                            }
                        }

                        //habilidades
                        foreach (var ability in pokemonDetail["abilities"].ToObject<List<JObject>>())
                        {
                            var dbAbility = context.Abilities.FirstOrDefault(a => a.Abilityname == ability["ability"]["name"].ToString()); // Buscamos el objeto habilidad en la base de datos correspondiente al nombre
                            if (dbAbility != null)
                            {
                                PokeData.Idabilities.Add(dbAbility); // Añadimos la habilidad al pokemon
                            
                            }
                            else
                            {
                                Console.WriteLine("____________________No se han encontrado alguna habilidad del pokemon " + result["name"] + " en la base de datos____________________");
                            }
                        }
                        //movimientos
                        foreach (var move in pokemonDetail["moves"].ToObject<List<JObject>>())
                        {
                            var dbMove = context.Moves.FirstOrDefault(m => m.Movename == move["move"]["name"].ToString()); // Buscamos el objeto movimiento en la base de datos correspondiente al nombre
                            if (dbMove != null)
                            {
                                PokeData.Idmoves.Add(dbMove); // Añadimos el movimiento al pokemon
                            
                            }
                            else
                            {
                                Console.WriteLine("____________________No se han encontrado algun movimiento del pokemon " + result["name"] + " en la base de datos____________________");
                            }
                        }
                        try
                        {
                            data.Add(PokeData); // Añadimos el pokemon a la lista
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine("____________________Error al añadir el pokemon " + result["name"] + " a la base de datos____________________");
                            Console.Error.WriteLine(e.StackTrace);
                            Console.WriteLine("________________________________________");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error al obtener los detalles del pokemon" + result["name"]);
                    }
                }
                context.Pokemons.AddRange(data); // Añadimos los datos a la base de datos
                await context.SaveChangesAsync();

            }
            else
            {
                Console.WriteLine("Error al obtener los datos de la API");
            }
        }

        public async Task LoadEgggroups()
        {
            var response = await client.GetAsync("https://pokeapi.co/api/v2/egg-group");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(); // Obtenemos el json de la API
                var jsonObject = JObject.Parse(json); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                var results = jsonObject["results"].ToObject<List<JObject>>(); // Accedemos a la lista de grupos de huevos
                List<Egggroup> data = []; // Lista de los grupos de huevos que se van a añadir a la base de datos

                foreach (var result in results)
                {
                    var egggroupDetailJson = await client.GetStringAsync(result["url"].ToString()); // Obtenemos los detalles de cada grupo de huevos con otra llamada a la API
                    var egggroupDetail = JObject.Parse(egggroupDetailJson); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos

                    var EggData = new Egggroup(); // Creamos un nuevo objeto Egggroup
                    EggData.Eggname = egggroupDetail["name"].ToString(); // Asignamos el nombre del grupo de huevos
                    EggData.Idpokes = new List<Pokemon>(); // Inicializamos la lista de pokemons
                    Pokemon PokeData;
                    string url;
                    foreach (var pd in egggroupDetail["pokemon_species"].ToObject<List<JObject>>())
                    {
                        url = pd["url"].ToString();
                        string[] parts = url.Split('/');
                        ushort id = ushort.Parse(parts[^2]);

                        PokeData = context.Find<Pokemon>(id); // Buscamos el pokemon en la base de datos
                        if (PokeData == null)
                        {
                            Console.WriteLine("---------------- Error al obtener el pokemon " + pd["name"] + " ----------------");
                            break;
                        }

                        EggData.Idpokes.Add(PokeData); // Añadimos los pokemons al grupo de huevos
                    }

                    try
                    {
                        data.Add(EggData); // Añadimos el grupo de huevos a la lista
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine("____________________Error al obtener los datos del grupo de huevos " + result["name"].ToString() + "____________________");
                        Console.Error.WriteLine(e.StackTrace);
                        Console.WriteLine("________________________________________");
                    }
                }
                context.Egggroups.AddRange(data); // Añadimos los datos a la base de datos
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Error al obtener los datos de grupos de huevos");
            }
        }

        public async Task LoadPics()
        {
            try
            {
                foreach (var pok in context.Pokemons)
                {
                    var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{pok.Idpoke}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync(); // Obtenemos el json de la API
                        var jsonObject = JObject.Parse(json); // Parseamos el json a un objeto de tipo JObject para poder acceder a los datos
                        var pic = jsonObject["sprites"]?["other"]?["official-artwork"]?["front_default"]?.ToString();
                        if (pic != null)
                        {
                            pok.PicURL = pic;
                            context.Entry(pok).State = EntityState.Modified; // Marca el objeto Pokemon como modificado
                            Console.WriteLine("Añadida foto al pokemon: " + pok.Pokename);
                        }
                        else
                        {
                            Console.Error.WriteLine("Error al cargar la foto del pokemon: " + pok.Pokename);
                            throw new Exception("Error al cargar la foto del pokemon " + pok.Pokename);
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.Error.WriteLine("Pokemon no encontrado en la API: " + pok.Pokename);
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine("CAMBIOS GUARDADOS");
            }
            catch (HttpRequestException e)
            {
                Console.Error.WriteLine("Error de red al intentar acceder a la API.");
                Console.Error.WriteLine(e.Message);
                throw;
            }
            catch (Exception e)
            {
                // Si ocurre un error, deshacemos los cambios
                Console.Error.WriteLine("Error genérico");
                Console.Error.WriteLine(e.StackTrace);
                throw; // Lanzamos la excepción para que pueda ser manejada en un nivel superior
            }
        }






    }
}
