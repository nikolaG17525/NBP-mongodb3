using MongoDB.Driver;
using RateMyProject.Models;

namespace RateMyProject.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public async Task<List<User>> GetAllUsersAsync() =>
            await _users.Find(user => true).ToListAsync();

        public async Task<User?> GetUserByCredentialsAsync(string username, string password) =>
            await _users.Find(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();

        public async Task<User?> GetUserByIdAsync(string id) =>
            await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task CreateMultipleUsersAsync(List<User> users) =>
            await _users.InsertManyAsync(users);

        public async Task DeleteUserByUsernameAsync(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            await _users.DeleteOneAsync(filter);
        }


        public async Task UpdatePasswordAsync(string id, string newPassword)
        {
            var update = Builders<User>.Update.Set(user => user.Password, newPassword);
            await _users.UpdateOneAsync(user => user.Id == id, update);
        }
    }
}
