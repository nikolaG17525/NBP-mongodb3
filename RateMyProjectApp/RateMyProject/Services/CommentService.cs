using MongoDB.Driver;
using RateMyProject.Models;

namespace RateMyProject.Services
{
    public class CommentService
    {
        private readonly IMongoCollection<Comment> _comments;

        public CommentService(IMongoDatabase database)
        {
            _comments = database.GetCollection<Comment>("comments");
        }

        public async Task CreateAsync(Comment comment) =>
            await _comments.InsertOneAsync(comment);

        public async Task<List<Comment>> GetCommentsByPostIdAsync(string postId) =>
            await _comments.Find(comment => comment.PostId == postId).ToListAsync();

        public async Task<Comment?> GetAsync(string commentId) =>
            await _comments.Find(comment => comment.Id == commentId).FirstOrDefaultAsync();

        public async Task UpdateTextAsync(string commentId, string newText)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", commentId);
            var update = Builders<Comment>.Update.Set("Text", newText);

            await _comments.UpdateOneAsync(filter, update);
        }


        public async Task RemoveAsync(string commentId) =>
            await _comments.DeleteOneAsync(comment => comment.Id == commentId);
    }
}
