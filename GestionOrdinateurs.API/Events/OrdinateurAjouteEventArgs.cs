// event/OrdinateurAjouteEventArgs.cs
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

public class OrdinateurAjouteEventArgs : EventArgs
{   
    public OrdinateurAjouteEventArgs(Ordinateur ordinateur){
        Ordinateur = ordinateur;
    }
    //Ordinateur ajouté
    public Ordinateur Ordinateur { get; }
}