using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPokemon.Models;

public partial class Ability
{
    public ushort Idability { get; set; }

    public string Abilityname { get; set;} = "";

    public virtual ICollection<Pokemon> Idpokes { get; set; } = [];

}
