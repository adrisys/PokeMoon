using ApiPokemon.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPokemon.Data;

public partial class PokemonContext : DbContext
{
    public PokemonContext(DbContextOptions<PokemonContext> options) : base(options)
    {
    }

    public virtual DbSet<Ability> Abilities { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Egggroup> Egggroups { get; set; }
    public virtual DbSet<Move> Moves { get; set; }
    public virtual DbSet<Pokemon> Pokemons { get; set; }
    public virtual DbSet<PokeType> Types { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<PokeType>(entity =>
        {
            entity.HasKey(e => e.Idtype);
            entity.Property(e => e.Idtype).HasColumnName("IDtype");
            entity.Property(e => e.Typename).HasColumnName("typename");
        });

        modelBuilder.Entity<Ability>(entity =>
        {
            entity.HasKey(e => e.Idability);
            entity.Property(e => e.Idability).HasColumnName("IDability");
            entity.Property(e => e.Abilityname).HasColumnName("abilityname");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Idcat);
            entity.Property(e => e.Idcat).HasColumnName("IDcat");
            entity.Property(e => e.Category1).HasColumnName("category");
        });

        modelBuilder.Entity<Egggroup>(entity =>
        {
            entity.HasKey(e => e.Idegg);
            entity.Property(e => e.Idegg).HasColumnName("IDegg");
            entity.Property(e => e.Eggname).HasColumnName("eggname");
        });

        modelBuilder.Entity<Move>(entity =>
        {
            entity.HasKey(e => e.Idmove);
            entity.HasIndex(e => e.CatID, "fk_moves_cat");
            entity.HasIndex(e => e.TypeID, "fk_moves_type");
            entity.HasIndex(e => e.Movename, "movename").IsUnique();
            entity.Property(e => e.Idmove).HasColumnName("IDmove");
            entity.Property(e => e.Accuracy).HasColumnName("accuracy");
            entity.Property(e => e.CatID).HasColumnName("IDcat");
            entity.Property(e => e.TypeID).HasColumnName("IDtype");
            entity.Property(e => e.Movename).HasColumnName("movename");
            entity.Property(e => e.Power).HasColumnName("power");
            entity.Property(e => e.Pp).HasColumnName("PP");

            entity.HasOne(m => m.CatNavigation)
                .WithMany(c => c.Moves)
                .HasForeignKey(m => m.CatID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_moves_cat");

            entity.HasOne(m => m.TypeNavigation)
                .WithMany(t => t.Moves)
                .HasForeignKey(t => t.TypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_moves_type");
        });

        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(e => e.Idpoke);
            entity.HasIndex(e => e.Pokename, "pokename").IsUnique();
            entity.Property(e => e.Idpoke).HasColumnName("IDpoke");
            entity.Property(e => e.Pokename).HasColumnName("pokename");
            entity.Property(e => e.Attack).HasColumnName("attack");
            entity.Property(e => e.Defense).HasColumnName("defense");
            entity.Property(e => e.Dualtype).HasColumnName("dualtype");
            entity.Property(e => e.Hp).HasColumnName("HP");
            entity.Property(e => e.Spattack).HasColumnName("spattack");
            entity.Property(e => e.Spdefense).HasColumnName("spdefense");
            entity.Property(e => e.Speed).HasColumnName("speed");
            entity.Property(e => e.PicURL).HasColumnName("PicURL");

            entity.HasMany(p => p.Idtypes)
                .WithMany(t => t.Idpokes)
                .UsingEntity<Dictionary<string, object>>(
                    j => j.HasOne<PokeType>().WithMany().HasForeignKey("IDtype"),
                    j => j.HasOne<Pokemon>().WithMany().HasForeignKey("IDpoke"),
                    j =>
                    {
                        j.ToTable("poke-type");
                        j.HasKey("IDpoke", "IDtype");
                    });

            entity.HasMany(p => p.Ideggs)
                .WithMany(e => e.Idpokes)
                .UsingEntity<Dictionary<string, object>>(
                    j => j.HasOne<Egggroup>().WithMany().HasForeignKey("IDegg"),
                    j => j.HasOne<Pokemon>().WithMany().HasForeignKey("IDpoke"),
                    j =>
                    {
                        j.ToTable("poke-egg");
                        j.HasKey("IDpoke", "IDegg");
                    });

            entity.HasMany(p => p.Idabilities)
                .WithMany(a => a.Idpokes)
                .UsingEntity<Dictionary<string, object>>(
                    j => j.HasOne<Ability>().WithMany().HasForeignKey("IDability"),
                    j => j.HasOne<Pokemon>().WithMany().HasForeignKey("IDpoke"),
                    j =>
                    {
                        j.ToTable("poke-ability");
                        j.HasKey("IDpoke", "IDability");
                    });

            entity.HasMany(p => p.Idmoves)
                .WithMany(m => m.Idpokes)
                .UsingEntity<Dictionary<string, object>>(
                    j => j.HasOne<Move>().WithMany().HasForeignKey("IDmove"),
                    j => j.HasOne<Pokemon>().WithMany().HasForeignKey("IDpoke"),
                    j =>
                    {
                        j.ToTable("poke-move");
                        j.HasKey("IDpoke", "IDmove");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
