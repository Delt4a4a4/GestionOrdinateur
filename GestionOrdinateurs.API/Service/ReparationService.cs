using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using GestionOrdinateurs.API.Logs.Reparations_Logs.Ordinateurs;

namespace GestionOrdinateurs.API;

public class ReparationService : IReparationService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService _journalService;

    public ReparationService(ParcInformatiqueContext context)
    {
        _context = context;
        _journalService = new JournalService();
    }

    public async Task SimulerReparationAsync(Ordinateur ordinateur){
        var reparation = await CreerReparationAsync(ordinateur, "Test", "En cours");
	    await Task.Delay(2000);
	    reparation.Statut = "Terminée";
        await _context.SaveChangesAsync();
        var date = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        _journalService.Ecrire($"{date} - {ordinateur.Nom} réparé");
    }

    private async Task<Reparation> CreerReparationAsync(Ordinateur ordinateur, string typeReparation, string statut){
    var reparation = new Reparation
    {
        UnOrdinateur = ordinateur,
        TypeReparation = typeReparation,
        DateDebut = DateTime.Now,
        Statut = statut
    };

    _context.Reparations.Add(reparation);
    await _context.SaveChangesAsync();

    return reparation;}


    public async Task<List<Reparation>> GetReparationsAsync(){
        return await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .ToListAsync();
    }

    public record HistoriqueReparationRecord(
        int OrdinateurId,
        string NomOrdinateur,
        string Technicien,
        string TypeReparation,
        DateTime DateIntervention
    );

    public async Task<List<HistoriqueReparationRecord>> GetHistoriqueAsync()
{
    return await _context.Reparations
        .Select(r => new HistoriqueReparationRecord(
            r.UnOrdinateur.Id,
            r.UnOrdinateur.Nom,
            r.Technicien,
            r.TypeReparation,
            r.DateDebut))
        .ToListAsync();
}
}