namespace GestionOrdinateurs.Shared;

public class InventaireOrdinateurs
{
    public List<Ordinateur> Ordinateurs { get; set; } = new();

    public Ordinateur? this[int ordinateurId] =>
        Ordinateurs.FirstOrDefault(o => o.Id == ordinateurId);

    public Ordinateur? this[string nom] =>
        Ordinateurs.FirstOrDefault(o =>
            string.Equals(o.Nom, nom, StringComparison.OrdinalIgnoreCase));
}