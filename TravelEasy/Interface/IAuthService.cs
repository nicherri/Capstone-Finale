using Microsoft.Identity.Client;
using TravelEasy.Models.DTO;

namespace TravelEasy.Interface
{
    public interface IAuthService
    {
        // Metodo per registrare un nuovo utente
        Task<AuthenticationResult> RegisterAsync(UserRegistrationDTO userRegistrationDto);

        // Metodo per effettuare il login
        Task<AuthenticationResult> LoginAsync(UserLoginDTO userLoginDto);

        // Metodo per confermare l'email
        Task<AuthenticationResult> ConfirmEmailAsync(string userId, string token);

        // Metodo per richiedere la reimpostazione della password
        Task<AuthenticationResult> RequestPasswordResetAsync(string email);

        // Metodo per reimpostare la password
        Task<AuthenticationResult> ResetPasswordAsync(PasswordResetDTO passwordResetDto);

        // Metodo per ottenere le informazioni dell'utente loggato
        Task<UserDTO> GetUserAsync(string userId);
    }

}
