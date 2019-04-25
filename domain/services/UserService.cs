using System;
using System.Threading.Tasks;
using domain.entities;
using domain.repositories;

namespace domain.services
{
    public class UserService : ServiceBase<UserEntity>
    {
        private UserRepository _userRepository;
        public UserService(UserRepository repository) : base(repository)
        {
            _userRepository = (UserRepository)repository;
        }

        // For Testing only
        public async Task SeedDatabase(){
            await _userRepository.SeedDatabase();
        }

        public async Task<UserEntity> AuthenticateAsync(string username, string password)
        {
            return await _userRepository.FindByUsernameAndPasswordAsync(username, password);
        }

        public async Task UpdateAsync(UserEntity user)
        {
            var oldUser = await _userRepository.FindByIdAsync(user.Id);
            if (oldUser.Name != user.Name)
                oldUser.Name = user.Name;
            
            if (oldUser.Email != user.Email)
                oldUser.Email = user.Email;

            if (oldUser.Password != user.Password)
                oldUser.Password = user.Password;

            oldUser.Validate(false);

            await _userRepository.UpdateAsync(oldUser);
            await _userRepository.SaveChangesAsync();
        }
    }
}