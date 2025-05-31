using Group2_SE1814_NET.Models;

namespace Group2_SE1814_NET.Services {
    public interface IUserService {
        Task<User> GetUserByEmailAndPassAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);

        Task<bool> CreateUserAsync(User user);

    }
}
