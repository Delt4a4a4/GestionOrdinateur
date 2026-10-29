using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API.Service;

public interface IUserService
{
    Task<List<User>> GetUsers();
    Task<User?> GetUserParLogin(string login);
}