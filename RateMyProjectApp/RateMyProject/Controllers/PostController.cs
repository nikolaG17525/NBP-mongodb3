using Microsoft.AspNetCore.Mvc;
using RateMyProject.Models;
using RateMyProject.Services;

namespace RateMyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly CommentService _commentService; // Ukoliko je potrebno za brisanje komentara vezanih za post

        public PostController(PostService postService, CommentService commentService)
        {
            _postService = postService;
            _commentService = commentService;
        }

        [HttpGet("getUnrated")]
        public async Task<ActionResult<List<Post>>> GetUnrated()
        {
            var unratedPosts = await _postService.GetUnratedPostsAsync();
            return Ok(unratedPosts);
        }

        [HttpGet("getRated")]
        public async Task<ActionResult<List<Post>>> GetRated()
        {
            var ratedPosts = await _postService.GetRatedPostsAsync();
            return Ok(ratedPosts);
        }

        [HttpGet("getPostById/{postId}")]
        public async Task<ActionResult<Post>> GetPostById(string postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound("Nonexistent post.");
            }
            return Ok(post);
        }

        [HttpGet("getPostsByCategory/{postCategory}")]
        public async Task<ActionResult<Post>> GetPostsByCategory(string postCategory)
        {
            var ratedPosts = await _postService.GetPostsByCategoryAsync(postCategory);
            return Ok(ratedPosts);
        }

        [HttpPost("createPost")]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            try
            {
                await _postService.CreatePostAsync(post);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePost([FromQuery] string id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                // Ako je potrebno, obrisati i komentare vezane za post
                // await _commentService.DeleteCommentsByPostIdAsync(id);
                return Ok("Post successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("updatePost")]
        public async Task<IActionResult> UpdatePost(string id, int rating, string professorsName)
        {
            try
            {
                await _postService.UpdatePostAsync(id, rating, professorsName);
                return Ok("Post successfully updated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
