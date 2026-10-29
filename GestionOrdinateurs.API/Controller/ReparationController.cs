using GestionOrdinateurs.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GestionOrdinateurs.API.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReparationController : ControllerBase{

    private readonly IReparationService _reparationService;
    private readonly IOrdinateurService _ordinateurService;
    private readonly StockService _stockService;

    public ReparationController(IReparationService reparationService, IOrdinateurService ordinateurService, StockService stockService){
        _reparationService = reparationService;
        _ordinateurService = ordinateurService;
        _stockService = stockService;
    }

    [HttpPost("SimulerReparation")]
    public async Task<IActionResult> SimulerReparation()
    {
        var listOrdinateurs = await _ordinateurService.GetOrdinateurs();

        foreach (var ordinateur in listOrdinateurs.Take(3))
        {
            await _reparationService.SimulerReparationAsync(ordinateur);
        }
        return Ok();
    }

    [HttpGet("GetReparations")]
    public async Task<IActionResult> GetReparations(){
        return Ok(await _reparationService.GetReparationsAsync());
    }

    [HttpGet("GetStockSSD")]
    public IActionResult GetStockSSD(){
        Parallel.For(0, 10, i => _stockService.ConsommerSSD());

        return Ok(_stockService.StockSSD);
    }
    [HttpGet("~/api/reparations/historique")]
    public async Task<IActionResult> GetHistorique()
    {
        var historique = await _reparationService.GetHistoriqueAsync();
        return Ok(historique);
    }
}