using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPokemon.Models;

public partial class Category
{
    public int Idcat { get; set; }

    public string Category1 { get; set; } = "";

    public virtual ICollection<Move> Moves { get; set; } = [];
}
