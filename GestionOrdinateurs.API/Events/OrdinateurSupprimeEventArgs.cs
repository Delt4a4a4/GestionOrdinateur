// event/OrdinateurSupprimeEventArgs.cs
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

public class OrdinateurSupprimeEventArgs : EventArgs
{
    public OrdinateurSupprimeEventArgs(Ordinateur ordinateur){
        Ordinateur = ordinateur;
    }

    //Ordinateur supprimé
    public Ordinateur Ordinateur { get; }
}