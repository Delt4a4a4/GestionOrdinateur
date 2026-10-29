using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Implémentation du contrat IOrdinateurService
public interface IOrdinateurService
{
 Task<List<Ordinateur>> GetOrdinateurs();
 Task<Ordinateur?> GetOrdinateurParId(int id);
 Task<bool> AjouterOrdinateur(Ordinateur ordinateur);
 Task<bool> MettreAJourOrdinateur(int id, Ordinateur ordinateur);
 Task<bool> SupprimerOrdinateur(int id);
 Task<List<Ordinateur>> GetOrdinateursParMarque(string marque);
 Task<List<Ordinateur>> GetOrdinateursDisponibles();
 Task<List<Ordinateur>> GetOrdinateursParPrixDecroissant();
 string Diagnostiquer(Ordinateur ordinateur);
 (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations)CalculerStatistiques();
}