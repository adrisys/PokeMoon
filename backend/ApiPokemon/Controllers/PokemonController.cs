using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPokemon.Models;
using ApiPokemon.Data;
using ApiPokemon.DTOs;

namespace ApiPokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController(PokemonContext context) : ControllerBase
    {

        // GET: api/Pokemons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PokemonDTO>>> GetPokemons()
        {
            List<Pokemon> pokemons = await context.Pokemons.ToListAsync();
            List<PokemonDTO> pokemonsDTOs = pokemons.Select(p => new PokemonDTO
            {
                Idpoke = p.Idpoke,
                Pokename = p.Pokename,
                Hp = p.Hp,
                Attack = p.Attack,
                Defense = p.Defense,
                Spattack = p.Spattack,
                Spdefense = p.Spdefense,
                Speed = p.Speed,
                Types = p.Idtypes.Select(t => new PokeTypeDTO { Idtype = t.Idtype, Typename = t.Typename }).ToList(),
                PicURL = p.PicURL
              
            }).ToList();

            return pokemonsDTOs;
        }

        // GET: api/Pokemons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PokemonDTO>> GetPokemon(ushort id)
        {
            var pokemon = await context.Pokemons.FindAsync(id);

            if (pokemon == null)
            {
                return NotFound();
            }
            return new PokemonDTO
            {
                Idpoke = pokemon.Idpoke,
                Pokename = pokemon.Pokename,
                Hp = pokemon.Hp,
                Attack = pokemon.Attack,
                Defense = pokemon.Defense,
                Spattack = pokemon.Spattack,
                Spdefense = pokemon.Spdefense,
                Speed = pokemon.Speed,
                Types = pokemon.Idtypes.Select(t => new PokeTypeDTO { Idtype = t.Idtype, Typename = t.Typename }).ToList(),
                PicURL = pokemon.PicURL
            };
        }

        [HttpGet("range/{number}/{offset}")]
        public async Task<ActionResult<IEnumerable<PokemonDTO>>> GetRangePokemons(ushort number, ushort offset)
        {
            List<Pokemon> pokemons = await context.Pokemons.Skip(offset).Take(number).ToListAsync();
            List<PokemonDTO> pokemonsDTOs = pokemons.Select(p => new PokemonDTO
            {
                Idpoke = p.Idpoke,
                Pokename = p.Pokename,
                Hp = p.Hp,
                Attack = p.Attack,
                Defense = p.Defense,
                Spattack = p.Spattack,
                Spdefense = p.Spdefense,
                Speed = p.Speed,
                Types = p.Idtypes.Select(t => new PokeTypeDTO { Idtype = t.Idtype, Typename = t.Typename }).ToList(),
                PicURL = p.PicURL
            }).ToList();

            return pokemonsDTOs;
        }

        // GET: api/Pokemons/5/types
        [HttpGet("{id}/types")]
        public async Task<ActionResult<IEnumerable<PokeTypeDTO>>> GetTypes(ushort id)
        {
            var pokemon = await context.Pokemons.FindAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }
            else
            {
                return pokemon.Idtypes.Select(t => new PokeTypeDTO { Idtype = t.Idtype, Typename = t.Typename }).ToList();
            }

        }

        // GET: api/Pokemons/5/abilities
        [HttpGet("{id}/abilities")]
        public async Task<ActionResult<IEnumerable<Ability>>> GetAbilities(ushort id)
        {
            var pokemon = await context.Pokemons.FindAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }
            else
            {
                return pokemon.Idabilities.ToList();
            }

        }

        //GET: api/Pokemons/5/moves
        [HttpGet("{id}/moves")]
        public async Task<ActionResult<IEnumerable<Move>>> GetMoves(ushort id)
        {
            var pokemon = await context.Pokemons.FindAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }
            else
            {
                return pokemon.Idmoves.ToList();
            }

        }

        //GET: api/Pokemons/5/egggroups
        [HttpGet("{id}/egggroups")]
        public async Task<ActionResult<IEnumerable<Egggroup>>> GetEggGroups(ushort id)
        {
            var pokemon = await context.Pokemons.FindAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }
            else
            {
                return pokemon.Ideggs.ToList();
            }

        }

        //GET: api/Pokemons/"NombreDelPokemon"/id

        [HttpGet("{poke}/id")]
        public async Task<ActionResult<ushort>> GetPokemonId(string poke)
        {
            var pokemon = await context.Pokemons.FirstOrDefaultAsync(p => p.Pokename.Equals(poke));
            if (pokemon == null) { return NotFound(); }
            return pokemon.Idpoke;

        }

        [HttpGet("howMany")]
        public int GetHowMany()
        {
            return context.Pokemons.Count();
        }





        // PUT: api/Pokemons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPokemon(ushort id, Pokemon pokemon)
        {
            if (id != pokemon.Idpoke)
            {
                return BadRequest();
            }

            context.Entry(pokemon).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PokemonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pokemons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pokemon>> PostPokemon(Pokemon pokemon)
        {
            context.Pokemons.Add(pokemon);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PokemonExists(pokemon.Idpoke))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPokemon", new { id = pokemon.Idpoke }, pokemon);
        }

        // DELETE: api/Pokemons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePokemon(ushort id)
        {
            var pokemon = await context.Pokemons.FindAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }

            context.Pokemons.Remove(pokemon);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool PokemonExists(ushort id)
        {
            return context.Pokemons.Any(e => e.Idpoke == id);
        }
    }
}
