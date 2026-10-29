namespace GestionOrdinateurs.API;

public class StockService
{
    public int StockSSD = 10;
    private readonly object _stockLock = new object(); 

    public async void ConsommerSSD(){
        Task.Delay(10).Wait();
        lock (_stockLock)
        {
        StockSSD--;
        }
    }
}