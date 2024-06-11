using MongoDB.Driver;
using RateMyProject.Models;

namespace RateMyProject.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService(IMongoDatabase database)
        {
            _posts = database.GetCollection<Post>("posts");
        }

        public async Task<List<Post>> GetUnratedPostsAsync() =>
            await _posts.Find(post => post.Rating == 0).ToListAsync();

        public async Task<List<Post>> GetRatedPostsAsync() =>
            await _posts.Find(post => post.Rating != 0).ToListAsync();

        public async Task<List<Post>> GetPostsByCategoryAsync(string postCategory) =>
            await _posts.Find(post => post.Category == postCategory).ToListAsync();

        public async Task<Post?> GetPostByIdAsync(string postId) =>
            await _posts.Find(post => post.Id == postId).FirstOrDefaultAsync();

        public async Task CreatePostAsync(Post post) =>
            await _posts.InsertOneAsync(post);

        public async Task DeletePostAsync(string postId) =>
            await _posts.DeleteOneAsync(post => post.Id == postId);

        public async Task UpdatePostAsync(string postId, int rating, string professorsName)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var update = Builders<Post>.Update
                .Set(p => p.Rating, rating)
                .Set(p => p.ProfessorsName, professorsName);

            await _posts.UpdateOneAsync(filter, update);
        }
    }
}
