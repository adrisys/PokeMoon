using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPokemon.Models;

public partial class Egggroup
{
    public ushort Idegg { get; set; }

    public string Eggname { get; set; } = "";

    public virtual ICollection<Pokemon> Idpokes { get; set; } = [];
}
