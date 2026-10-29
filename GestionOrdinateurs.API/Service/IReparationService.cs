using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;


public interface IReparationService{
    Task SimulerReparationAsync(Ordinateur ordinateur);
    Task<List<Reparation>> GetReparationsAsync();
    Task<List<ReparationService.HistoriqueReparationRecord>> GetHistoriqueAsync();
}