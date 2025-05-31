using Group2_SE1814_NET.Models;

namespace Group2_SE1814_NET.Repositories {
    public interface IUserRepository {
        Task<User> GetUserByEmailAndPassAsync(string email, string pass);

        Task<User> GetUserByEmailAsync(string email);

        Task<bool> CreateUserAsync(User user);
    }
}
