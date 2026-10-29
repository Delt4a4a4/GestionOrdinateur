using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Contexte EF Core : représente la base SQLite du parc informatique
public class ParcInformatiqueContext : DbContext {

    public DbSet<Ordinateur> Ordinateurs { get; set; }
    public DbSet<Reparation> Reparations { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=parcinfo.db");
        }
    }

    public ParcInformatiqueContext(DbContextOptions<ParcInformatiqueContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ordinateur>().HasKey(o => o.Id);
        modelBuilder.Entity<Ordinateur>().OwnsOne(o => o.Caracteristiques);
        modelBuilder.Entity<Ordinateur>()
            .HasMany(o => o.Reparations)
            .WithOne(r => r.UnOrdinateur);
        // Garantit l'unicité du login au niveau de la base de données.
        modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
    }
}