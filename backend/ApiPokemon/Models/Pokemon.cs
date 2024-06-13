using System;
using System.Collections.Generic;

namespace ApiPokemon.Models;

public partial class Pokemon
{
    public ushort Idpoke { get; set; }

    public string Pokename { get; set; } = "";

    public byte Hp { get; set; }

    public byte Attack { get; set; }

    public byte Defense { get; set; }

    public byte Spattack { get; set; }

    public byte Spdefense { get; set; }

    public byte Speed { get; set; }

    public bool Dualtype { get; set; }

    public string? PicURL { get; set; } 

    // relaciones sin tabla intermedia definida
    public virtual ICollection<Egggroup> Ideggs { get; set; } = [];

    public virtual ICollection<PokeType> Idtypes { get; set; } = [];

    public virtual ICollection<Ability> Idabilities { get; set; } = [];

    public virtual ICollection<Move> Idmoves { get; set; } = [];
}
