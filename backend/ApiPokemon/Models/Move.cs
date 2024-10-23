using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPokemon.Models;

public partial class Move
{
    public ushort Idmove { get; set; }

    public string Movename { get; set; } = "";

    public int TypeID { get; set; }

    public int CatID { get; set; }

    public byte? Power { get; set; }

    public byte? Accuracy { get; set; }

    public byte? Pp { get; set; }


    public virtual required Category CatNavigation { get; set; }

    public virtual required PokeType TypeNavigation { get; set; }

    public virtual ICollection<Pokemon> Idpokes { get; set; } = [];

}
