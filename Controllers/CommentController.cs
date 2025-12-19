using Microsoft.AspNetCore.Mvc;
using IT_08_1_backend.Models;

namespace IT_08_1_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private static List<Comment> _comments = new List<Comment>();

        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine($"GET /api/comment - Returning {_comments.Count} comments");
            return Ok(_comments);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Comment is null");
            }

            comment.CreatedAt = DateTime.Now;
            _comments.Add(comment);

            Console.WriteLine($"POST /api/comment - User: {comment.UserName}, Message: {comment.Message}");
            
            return Ok(comment);
        }

        [HttpDelete]
        public IActionResult DeleteAll()
        {
            _comments.Clear();
            Console.WriteLine("DELETE /api/comment - All comments cleared");
            return NoContent();
        }
    }
}