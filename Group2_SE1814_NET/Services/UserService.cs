using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Repositories;

namespace Group2_SE1814_NET.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task<bool> CreateUserAsync(User user) {
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<User> GetUserByEmailAndPassAsync(string email, string password) {
            return await _userRepository.GetUserByEmailAndPassAsync(email, password);
        }

        public async Task<User> GetUserByEmailAsync(string email) {
            return await _userRepository.GetUserByEmailAsync(email);
        }

    }
}
