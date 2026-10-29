using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionOrdinateurs.Tests;

public class OrdinateurServiceTest {

    private ParcInformatiqueContext CreerContexte(){
    var options =
        new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

    return new ParcInformatiqueContext(options);
    }


    [Fact]
    public async Task AjouterOrdinateursAvecNouvelId(){
        var contexte = CreerContexte();
        var ordinateurService = new OrdinateurService(contexte);
        //Test avec un ordinateur portable et un ordinateur fixe
        var ordinateur = new Ordinateur(100, "Zenbook 14", EtatOrdinateur.EnService, 1530.5m, new Caracteristiques(16, 0, TypeOrdinateur.Portable, "Asus", "Intel i7"));
        
        int total = (await ordinateurService.GetOrdinateurs()).Count();
        bool result = await ordinateurService.AjouterOrdinateur(ordinateur);

        Assert.Equal(total + 1, (await ordinateurService.GetOrdinateurs()).Count());
        Assert.True(result);
    }

    [Fact]
    public async Task SupprimerOrdinateurAvecIdExistant(){
        var contexte = CreerContexte();
        var ordinateurService = new OrdinateurService(contexte);
        //Test de la suppression d'ordinateurs fixes et portables avec des IDs existants
        var ordinateur = new Ordinateur(101, "Zenbook 14", EtatOrdinateur.EnService, 1530.5m, new Caracteristiques(16, 0, TypeOrdinateur.Portable, "Asus", "Intel i7"));

        
        await ordinateurService.AjouterOrdinateur(ordinateur);

        int total = (await ordinateurService.GetOrdinateurs()).Count();

        bool result = await ordinateurService.SupprimerOrdinateur(ordinateur.Id);

        Assert.Equal(total - 1, (await ordinateurService.GetOrdinateurs()).Count());
        Assert.True(result);
    }
    
    [Fact]
    public async Task SupprimerOrdinateurAvecIdInexistant(){
        var contexte = CreerContexte();
        var ordinateurService = new OrdinateurService(contexte);

        bool result = await ordinateurService.SupprimerOrdinateur(9999);

        Assert.False(result);
    }

    [Fact]
    public async Task CreerReparationAvecOrdinateur()
    {
        var contexte = CreerContexte();

        var ordinateur = new Ordinateur(114, "Zenbook 14", EtatOrdinateur.EnService, 1530.5m, new Caracteristiques(16, 0, TypeOrdinateur.Portable, "Asus", "Intel i7"));

        var dateDebut = DateTime.UtcNow;

        var reparation = new Reparation
        {
            UnOrdinateur = ordinateur,
            TypeReparation = "Remplacement SSD",
            DateDebut = dateDebut,
            Statut = "En cours",
            Technicien = "Alice"
        };

        contexte.Reparations.Add(reparation);
        await contexte.SaveChangesAsync();

        var reparationEnregistree = await contexte.Reparations
            .Include(r => r.UnOrdinateur)
            .SingleAsync();

        Assert.True(reparationEnregistree.Id > 0);
        Assert.Equal("Remplacement SSD", reparationEnregistree.TypeReparation);
        Assert.Equal(dateDebut, reparationEnregistree.DateDebut);
        Assert.Equal("En cours", reparationEnregistree.Statut);
        Assert.Equal("Alice", reparationEnregistree.Technicien);
        Assert.Equal("Zenbook 14", reparationEnregistree.UnOrdinateur.Nom);
    }
}