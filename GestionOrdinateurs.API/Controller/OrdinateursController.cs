using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API;
using Microsoft.AspNetCore.Authorization;

namespace GestionOrdinateurs.API.OrdinateursControllers;

// Contrôleur REST exposé sur /api/ordinateurs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdinateursController : ControllerBase{

    private readonly IOrdinateurService ordinateurService;
    private readonly ILogger<OrdinateursController> logger;
    private readonly IConfiguration configuration; 

    public OrdinateursController(IOrdinateurService ordinateurService, ILogger<OrdinateursController> logger, IConfiguration configuration)
    {
        this.ordinateurService = ordinateurService;
        this.logger = logger;
        this.configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdinateurs()
    {   
        logger.LogInformation("Recuperation de la liste des ordinateurs");
        return Ok(await ordinateurService.GetOrdinateurs());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrdinateurParId(int id)
    {   
        logger.LogInformation("Recuperation de l'ordinateur avec l'id {Id}", id);
        var ordinateur = await  ordinateurService.GetOrdinateurParId(id);
        if (ordinateur == null) {
            logger.LogWarning("Ordinateur avec l'id {Id} non trouvé", id);
            return NotFound();
        }
        return Ok(ordinateur);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AjouterOrdinateur(Ordinateur ordinateur){
        logger.LogInformation("Ajout d'un nouvel ordinateur");
        var ordinateurAjoute = await ordinateurService.AjouterOrdinateur(ordinateur);
        if (!ordinateurAjoute){
            logger.LogWarning("Conflit lors de l'ajout de l'ordinateur avec l'id {Id}", ordinateur.Id);
            return Conflict();
        }
        return CreatedAtAction(nameof(GetOrdinateurParId), new { id = ordinateur.Id }, ordinateur);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MettreAJourOrdinateur(int id, Ordinateur ordinateur){
        logger.LogInformation("Mise à jour de l'ordinateur avec l'id {Id}", id);
        var updated = await ordinateurService.MettreAJourOrdinateur(id, ordinateur);
        if(!updated){
            logger.LogWarning("Ordinateur avec l'id {Id} non trouvé pour la mise à jour", id);
            return NotFound();
        }
        return Ok(ordinateur);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SupprimerOrdinateur(int id)
    {   
        logger.LogInformation("Suppression de l'ordinateur avec l'id {Id}", id);
        var deleted = await ordinateurService.SupprimerOrdinateur(id);
        if(!deleted){
            logger.LogWarning("Ordinateur avec l'id {Id} non trouvé pour la suppression", id);
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("marque/{marque}")]
    public async Task<IActionResult> GetOrdinateursParMarque(string marque)
    {
        logger.LogInformation("Recuperation des ordinateurs de la marque {Marque}", marque);
        return Ok(await ordinateurService.GetOrdinateursParMarque(marque));
    }
    
    [HttpGet("info")]
    public IActionResult GetInfosApplication()
    {
        logger.LogInformation("Consultation des informations de l'application");

        var nom = configuration["Application:Name"];
        var version = configuration["Application:Version"];

        return Ok(new { Nom = nom, Version = version });
    }

    [HttpGet("disponibles")]
    public async Task<IActionResult> GetOrdinateursDisponibles()
    {
        logger.LogInformation("Recuperation des ordinateurs disponibles");
        return Ok(await ordinateurService.GetOrdinateursDisponibles());
    }

    [HttpGet("prix/decroissant")]
    public async Task<IActionResult> GetOrdinateursParPrixDecroissant(){
        logger.LogInformation("Recuperation des ordinateurs par prix decroissant");
        return Ok(await ordinateurService.GetOrdinateursParPrixDecroissant());
    }

    [HttpGet("diagnostic/{id}")]
    public async Task<IActionResult> Diagnostiquer(int id){
        var ordinateur = await ordinateurService.GetOrdinateurParId(id);

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateurService.Diagnostiquer(ordinateur));
    }

    [HttpGet("statistiques")]
    public IActionResult GetStatistiques(){
        var (total, enStock, valeurStock, nbReparations) =
            ordinateurService.CalculerStatistiques();

        return Ok(new
        {
            TotalOrdinateurs = total,
            OrdinateursDisponibles = enStock,
            ValeurStock = valeurStock,
            NombreReparations = nbReparations
        });
    }

    [HttpGet("inventaire/{nom}")]
    public async Task<IActionResult> ChercherDansInventaire(string nom)
    {
        var inventaire = new InventaireOrdinateurs
        {
            Ordinateurs = await ordinateurService.GetOrdinateurs()
        };

        var ordinateur = inventaire[nom];

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }
}