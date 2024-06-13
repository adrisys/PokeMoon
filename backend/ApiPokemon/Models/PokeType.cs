using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPokemon.Models;

public partial class PokeType
{

    public int Idtype { get; set; }

    public string Typename { get; set; } = "";

    public virtual ICollection<Move> Moves { get; set; } = new List<Move>();

    public virtual ICollection<Pokemon> Idpokes { get; set; } = new List<Pokemon>();

}
