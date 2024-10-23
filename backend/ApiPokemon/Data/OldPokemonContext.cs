using System;
using System.Collections.Generic;
using ApiPokemon.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPokemon.Data;

public partial class OldPokemonContext(DbContextOptions<OldPokemonContext> options) : DbContext(options)
{
    public virtual DbSet<Ability> Abilities { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Egggroup> Egggroups { get; set; }

    public virtual DbSet<Move> Moves { get; set; }

    public virtual DbSet<Pokemon> Pokemons { get; set; }

    public virtual DbSet<PokeType> Types { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<PokeType>(entity =>
        {
            entity.HasKey(e => e.Idtype);

            entity.Property(e => e.Idtype)
                .HasColumnType("int")
                .HasColumnName("IDtype")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Typename)
                .HasMaxLength(50)
                .HasColumnName("typename")
                .IsRequired();
        });

        modelBuilder.Entity<Ability>(entity =>
        {
            entity.HasKey(e => e.Idability);

            entity.Property(e => e.Idability)
                .HasColumnType("int")
                .HasColumnName("IDability")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Abilityname)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("abilityname");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Idcat);

            entity.Property(e => e.Idcat)
                .HasColumnType("int")
                .HasColumnName("IDcat")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Category1)
                .HasMaxLength(50)
                .HasColumnName("category")
                .IsRequired();
        });

        modelBuilder.Entity<Egggroup>(entity =>
        {
            entity.HasKey(e => e.Idegg);


            entity.Property(e => e.Idegg)
                .HasColumnType("int")
                .HasColumnName("IDegg")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Eggname)
                .HasMaxLength(50)
                .HasColumnName("eggname")
                .IsRequired();

        });


        modelBuilder.Entity<Move>(entity =>
        {
            entity.HasKey(e => e.Idmove);

            entity.HasIndex(e => e.Idcat, "fk_moves_cat");

            entity.HasIndex(e => e.Idtype, "fk_moves_type");

            entity.HasIndex(e => e.Movename, "movename").IsUnique();

            entity.Property(e => e.Idmove)
                .HasColumnType("int")
                .HasColumnName("IDmove")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Accuracy)
                .HasColumnType("int")
                .HasColumnName("accuracy");
            entity.Property(e => e.Idcat)
                .HasColumnType("int")
                .HasColumnName("IDcat");
            entity.Property(e => e.Idtype)
                .HasColumnType("int")
                .HasColumnName("IDtype");
            entity.Property(e => e.Movename)
                .HasMaxLength(50)
                .HasColumnName("movename");
            entity.Property(e => e.Power)
                .HasColumnType("int")
                .HasColumnName("power");
            entity.Property(e => e.Pp)
                .HasColumnType("int")
                .HasColumnName("PP");

            entity
                .HasOne(m => m.IdcatNavigation)
                .WithMany(c => c.Moves)
                .HasForeignKey(m => m.Idcat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_moves_cat");

            entity
                .HasOne(m => m.IdtypeNavigation)
                .WithMany(t => t.Moves)
                .HasForeignKey(m => m.Idtype)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_moves_type");
        });


        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(e => e.Idpoke);


            entity.HasIndex(e => e.Pokename, "pokename").IsUnique();

            entity.Property(e => e.Idpoke)
                .HasColumnType("int")
                .HasColumnName("IDpoke")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Pokename)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("pokename");

            entity.Property(e => e.Attack)
                .HasColumnType("int")
                .HasColumnName("attack");
            entity.Property(e => e.Defense)
                .HasColumnType("int")
                .HasColumnName("defense");
            entity.Property(e => e.Dualtype)
                .HasColumnType("bit")
                .HasColumnName("dualtype");
            entity.Property(e => e.Hp)
                .HasColumnType("int")
                .HasColumnName("HP");
            entity.Property(e => e.Spattack)
                .HasColumnType("int")
                .HasColumnName("spattack");
            entity.Property(e => e.Spdefense)
                .HasColumnType("int")
                .HasColumnName("spdefense");
            entity.Property(e => e.Speed)
                .HasColumnType("int")
                .HasColumnName("speed");
            entity.Property(e => e.PicURL)
                .HasColumnName("PicURL");

            // Relacion n:m pokemons-tipos
            entity.HasMany(p => p.Idtypes)
            .WithMany(t => t.Idpokes)
            .UsingEntity<Dictionary<string, object>>(
                j => j
                .HasOne<PokeType>()
                    .WithMany()
                    .HasForeignKey("IDtype"),
                j => j
                .HasOne<Pokemon>()
                    .WithMany()
                    .HasForeignKey("IDpoke"),
                j =>
                {
                    j.ToTable("poke-type");
                    j.HasKey("IDpoke", "IDtype");
                }

                );
            // Relacion n:m pokemons-egggroups
            entity.HasMany(p => p.Ideggs)
            .WithMany(e => e.Idpokes)
            .UsingEntity<Dictionary<string, object>>(
                j => j
                .HasOne<Egggroup>()
                    .WithMany()
                    .HasForeignKey("IDegg"),
                j => j
                .HasOne<Pokemon>()
                    .WithMany()
                    .HasForeignKey("IDpoke"),
                j =>
                {
                    j.ToTable("poke-egg");
                    j.HasKey("IDpoke", "IDegg");
                }
                );

            // Relacion n:m pokemons-abilities
            entity.HasMany(p => p.Idabilities)
            .WithMany(a => a.Idpokes)
            .UsingEntity<Dictionary<string, object>>(
                j => j
                .HasOne<Ability>()
                    .WithMany()
                    .HasForeignKey("IDability"),
                j => j
                .HasOne<Pokemon>()
                    .WithMany()
                    .HasForeignKey("IDpoke"),
                j =>
                {
                    j.ToTable("poke-ability");
                    j.HasKey("IDpoke", "IDability");
                }
                );
            // Relacion n:m pokemons-moves
            entity.HasMany(p => p.Idmoves)
            .WithMany(m => m.Idpokes)
            .UsingEntity<Dictionary<string, object>>(
                j => j
                .HasOne<Move>()
                    .WithMany()
                    .HasForeignKey("IDmove"),
                j => j
                .HasOne<Pokemon>()
                    .WithMany()
                    .HasForeignKey("IDpoke"),
                j =>
                {
                    j.ToTable("poke-move");
                    j.HasKey("IDpoke", "IDmove");
                }
                );



               
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
