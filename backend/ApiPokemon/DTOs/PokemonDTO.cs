using ApiPokemon.Models;

namespace ApiPokemon.DTOs
{
    public class PokemonDTO
    {
        public ushort Idpoke { get; set; }

        public string Pokename { get; set; } = "";

        public byte Hp { get; set; }

        public byte Attack { get; set; }

        public byte Defense { get; set; }

        public byte Spattack { get; set; }

        public byte Spdefense { get; set; }

        public byte Speed { get; set; }

        public string? PicURL { get; set; }

        public ICollection<PokeTypeDTO> Types { get; set; } = [];
    }
}
