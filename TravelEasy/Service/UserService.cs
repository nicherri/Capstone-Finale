using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TravelEasy.Data;
using TravelEasy.Models;
using TravelEasy.Models.DTO;

public class UserService : IUserService
{
    private readonly TravelEasyContext _context;

    public UserService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<UserDTO> Authenticate(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        // Verifica della password usando hashing sicuro
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        return new UserDTO
        {
            Id = user.Id,
            Nome = user.Nome,
            Cognome = user.Cognome,
            Email = user.Email,
            Role = user.Role
        };
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        // Verifica hash della password
        using (var sha256 = SHA256.Create())
        {
            var hashedInputBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedInputStringBuilder = new StringBuilder(64);

            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));

            return hashedInputStringBuilder.ToString() == storedHash;
        }
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        return await _context.Users.Select(user => new UserDTO
        {
            Id = user.Id,
            Nome = user.Nome,
            Cognome = user.Cognome,
            Email = user.Email,
            Role = user.Role
        }).ToListAsync();
    }

    public async Task<UserDTO> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDTO
        {
            Id = user.Id,
            Nome = user.Nome,
            Cognome = user.Cognome,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<UserDTO> CreateUserAsync(UserDTO userDto)
    {
        // Hash della password
        string hashedPassword = HashPassword(userDto.Password);

        var user = new User
        {
            Nome = userDto.Nome,
            Cognome = userDto.Cognome,
            Email = userDto.Email,
            PasswordHash = hashedPassword,
            Role = userDto.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        userDto.Id = user.Id;
        return userDto;
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToUpper();
        }
    }

    public async Task<UserDTO> UpdateUserAsync(int id, UserDTO userDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        user.Nome = userDto.Nome;
        user.Cognome = userDto.Cognome;
        user.Email = userDto.Email;
        user.Role = userDto.Role;

        await _context.SaveChangesAsync();
        return userDto;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
