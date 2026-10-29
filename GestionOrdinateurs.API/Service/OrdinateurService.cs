using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using GestionOrdinateurs.API.Logs.Reparations_Logs.Ordinateurs;

namespace GestionOrdinateurs.API;

public class OrdinateurService : IOrdinateurService {


    private readonly ParcInformatiqueContext _context;
    private readonly OrdinateurAbonne _observateur = new();
    private readonly JournalService _journalService = new();

    public event EventHandler<OrdinateurAjouteEventArgs> OrdinateurAjoute;
    public event EventHandler<OrdinateurSupprimeEventArgs> OrdinateurSupprime;
    public event EventHandler<OrdinateurMisAJourEventArgs> OrdinateurMisAJour;

    public OrdinateurService(ParcInformatiqueContext context){
        _context = context;

        // Abonnement des méthodes d'affichage aux trois évènements du service
        OrdinateurAjoute   += _observateur.AfficherMessageAjout;
        OrdinateurSupprime += _observateur.AfficherMessageSuppression;
        OrdinateurMisAJour += _observateur.AfficherMessageMiseAJour;
    }
    
    public async Task<bool> AjouterOrdinateur(Ordinateur ordinateur){
        //vérifie si l'ordinateur avec le même id existe déjà avant de l'ajouter
        if (await GetOrdinateurParId(ordinateur.Id) == null) {
            _context.Ordinateurs.Add(ordinateur);
            await _context.SaveChangesAsync();

        var date = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        _journalService.Ecrire($"{date} - Ordinateur ajouté : {ordinateur.Nom} (ID {ordinateur.Id})");

        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs(ordinateur));
            return true;
        }
        return false;
    }

    public async Task<bool> SupprimerOrdinateur(int id){
        Ordinateur? ordinateur = await GetOrdinateurParId(id);
        //vérifie si l'ordinateur existe avant de le supprimer
        if (ordinateur != null){
            _context.Ordinateurs.Remove(ordinateur);
            await _context.SaveChangesAsync();

            var date = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            _journalService.Ecrire($"{date} - Ordinateur supprimé : {ordinateur.Nom} (ID {ordinateur.Id})");

            OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs(ordinateur));
            return true;
        }
        return false;
    }

    public async Task<bool> MettreAJourOrdinateur(int id, Ordinateur ordinateur){
        // AsNoTracking : copie de l'état avant modification, sinon EF suivrait deux instances de même clé
        Ordinateur? ancienOrdinateur = await _context.Ordinateurs.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
    
        if (ancienOrdinateur != null){
            _context.Ordinateurs.Update(ordinateur);
            await _context.SaveChangesAsync();

            var date = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            _journalService.Ecrire($"{date} - Ordinateur modifié : {ordinateur.Nom} (ID {ordinateur.Id})");

            OrdinateurMisAJour?.Invoke(this, new OrdinateurMisAJourEventArgs(ancienOrdinateur, ordinateur));
            return true;
        }
        return false;
    }

    public async Task<List<Ordinateur>> GetOrdinateurs(){
        return await _context.Ordinateurs.ToListAsync();
    }

     public async Task<Ordinateur?> GetOrdinateurParId(int id){
        return await _context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Ordinateur>> GetOrdinateursParMarque(string marque) {
        return await _context.Ordinateurs
            .Where(o => o.Caracteristiques.Marque.Equals(marque))
            .ToListAsync();
    }

    public async Task<List<Ordinateur>> GetOrdinateursDisponibles(){
        return await _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .ToListAsync();
    }

    public async Task<List<Ordinateur>> GetOrdinateursParPrixDecroissant(){
        var ordinateurs = await _context.Ordinateurs.ToListAsync();

        return ordinateurs
            .OrderByDescending(o => o.Prix)
            .ToList();
    }

    public string Diagnostiquer(Ordinateur ordinateur)
    {
        return ordinateur switch
	    {
		    {
			    Etat: EtatOrdinateur.HorsService
		    }
			    => "Remplacement",

		    {
			    Etat: EtatOrdinateur.Lent,
			    Caracteristiques:
			    {
				    RamGo: < 8
			    }
		    }
			    => "Ajouter RAM",

		_
			    => "Aucune action"
	    };
    }

    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations)
    CalculerStatistiques(){
        var ordinateurs = _context.Ordinateurs.ToList();

        var totalOrdinateurs = ordinateurs.Count;
        var ordinateursDisponibles = ordinateurs.Count(o => o.Stock > 0);
        var valeurStock = ordinateurs.Sum(o => o.Prix * o.Stock);
        var nombreReparations = _context.Reparations.Count();

        return (
            totalOrdinateurs,
            ordinateursDisponibles,
            valeurStock,
            nombreReparations);
    }   
}