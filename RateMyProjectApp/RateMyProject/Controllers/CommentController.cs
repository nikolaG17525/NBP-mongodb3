using Microsoft.AspNetCore.Mvc;
using RateMyProject.Models;
using RateMyProject.Services;

namespace RateMyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("dodajKomentar")]
        public async Task<IActionResult> Create(Comment comment)
        {
            try
            {
                await _commentService.CreateAsync(comment);
                return Ok(new { Success = true, Message = "Comment created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while creating the comment.", Error = ex.Message });
            }
        }

        [HttpGet("vratiKomentare/{postId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsByPostId(string postId)
        {
            var comments = await _commentService.GetCommentsByPostIdAsync(postId);
            return Ok(comments);
        }

        [HttpDelete("obrisiKomentar/{commentId}")]
        public async Task<IActionResult> Delete(string commentId)
        {
            var comment = await _commentService.GetAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            await _commentService.RemoveAsync(commentId);
            return NoContent();
        }

        [HttpPut("izmeniKomentar/{commentId}")]
        public async Task<IActionResult> UpdateCommentText(string commentId, [FromBody] string newText)
        {
            try
            {
                await _commentService.UpdateTextAsync(commentId, newText);
                return Ok("Komentar uspesno izmenjen.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}
