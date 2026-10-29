using System.Globalization;

namespace GestionOrdinateurs.API.Logs.Reparations_Logs.Ordinateurs;

public class JournalService
{   
    public void Ecrire(string message){
        // Crée le répertoire des logs s'il n'existe pas
        Directory.CreateDirectory("Logs/Reparations_Logs/Ordinateurs");
        
        using var writer = new StreamWriter(
            "Logs/Reparations_Logs/Ordinateurs/reparations.log",
            append: true);

        writer.WriteLine(message);
    }
}