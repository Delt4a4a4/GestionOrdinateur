using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionOrdinateurs.Shared;

[Table("Ordinateur")]
public class Ordinateur{
    
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public EtatOrdinateur Etat { get; set; }

    public int Stock { get; set; } = 1;

    public decimal Prix { get; set; } = 0;

    public Caracteristiques Caracteristiques { get; set; } = null!;

    [JsonIgnore]
    public List<Reparation> Reparations { get; set; } = new();

    public bool Vendu { get; set; } = false;

    public Ordinateur()
    {
    }

    public Ordinateur(
        int id,
        string nom,
        EtatOrdinateur etat,
        decimal prix,
        Caracteristiques caracteristiques)
    {
        Id = id;
        Nom = nom;
        Etat = etat;
        Prix = prix;
        Caracteristiques = caracteristiques;
    }

    public virtual string RecupererDetails()
    {
        return $"L'ordinateur est : {Id}, {Caracteristiques.Marque}, {Nom}, " +
               $"{Caracteristiques.Processeur}, {Caracteristiques.RamGo}, {Etat}, {Stock}, {Prix}";
    }
}