using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPokemon.Models;

public partial class Move
{
    public ushort Idmove { get; set; }

    public string Movename { get; set; } = "";

    public int Idtype { get; set; }

    public int Idcat { get; set; }

    public byte? Power { get; set; }

    public byte? Accuracy { get; set; }

    public byte? Pp { get; set; }


    public virtual required Category IdcatNavigation { get; set; }

    public virtual required PokeType IdtypeNavigation { get; set; }

    public virtual ICollection<Pokemon> Idpokes { get; set; } = [];

}
