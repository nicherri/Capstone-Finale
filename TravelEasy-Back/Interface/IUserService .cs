using TravelEasy.Models.DTO;

public interface IUserService
{
    Task<UserDTO> Authenticate(string email, string password);
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<UserDTO> GetUserByIdAsync(int id);
    Task<UserDTO> CreateUserAsync(UserDTO userDto);
    Task<UserDTO> UpdateUserAsync(int id, UserDTO userDto);
    Task<bool> DeleteUserAsync(int id);
}
