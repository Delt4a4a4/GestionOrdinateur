using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API.Service;

public class UserService : IUserService
{
    private readonly ParcInformatiqueContext _context;

    public UserService(ParcInformatiqueContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetUserParLogin(string login)
    {
        return await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Login == login);
    }
}