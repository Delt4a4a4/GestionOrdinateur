using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

public class OrdinateurMisAJourEventArgs : EventArgs
{
    public OrdinateurMisAJourEventArgs(Ordinateur ancienOrdinateur, Ordinateur nouvelOrdinateur){
        AncienOrdinateur = ancienOrdinateur;
        NouvelOrdinateur = nouvelOrdinateur;
    }

    //Ordinateur mis à jour
    public Ordinateur AncienOrdinateur { get; }
    public Ordinateur NouvelOrdinateur { get; }

}