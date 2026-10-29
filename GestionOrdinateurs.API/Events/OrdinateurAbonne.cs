// Events/OrdinateurAbonne.cs
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Abonné aux évènements du service : affiche un message à chaque opération
public class OrdinateurAbonne
{
    public void AfficherMessageAjout(object? sender, OrdinateurAjouteEventArgs e)
    {
        Console.WriteLine($"[EVENEMENT] Ordinateur ajouté : {e.Ordinateur.Nom} ({e.Ordinateur.Caracteristiques.Marque})");
    }

    public void AfficherMessageSuppression(object? sender, OrdinateurSupprimeEventArgs e)
    {
        Console.WriteLine($"[EVENEMENT] Ordinateur supprimé : {e.Ordinateur.Nom} (Id {e.Ordinateur.Id})");
    }

    public void AfficherMessageMiseAJour(object? sender, OrdinateurMisAJourEventArgs e)
    {
        Console.WriteLine($"[EVENEMENT] Ordinateur mis à jour : {e.AncienOrdinateur.Nom} -> {e.NouvelOrdinateur.Nom}");
    }
}