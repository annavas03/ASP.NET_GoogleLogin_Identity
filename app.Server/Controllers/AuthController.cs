using app.Server.Data;
using app.Server.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace app.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleTokenRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.Token,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { "446631909658-vvbdjfbfqhhs0en5inpl4glf6m21uh2m.apps.googleusercontent.com" }
                    }
                );

                var existingUser = _context.Users.FirstOrDefault(u => u.GoogleId == payload.Subject);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        GoogleId = payload.Subject,
                        Email = payload.Email,
                        Name = payload.Name,
                        Picture = payload.Picture
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                return Ok(new
                {
                    email = payload.Email,
                    name = payload.Name,
                    picture = payload.Picture
                });
            }
            catch
            {
                return BadRequest(new { error = "Invalid token" });
            }
        }

        [HttpGet("userphoto")]
        public async Task<IActionResult> GetUserPhoto([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest();

            using var httpClient = new HttpClient();
            try
            {
                var imageBytes = await httpClient.GetByteArrayAsync(url);
                return File(imageBytes, "image/jpeg");
            }
            catch
            {
                return NotFound();
            }
        }

    }

    public class GoogleTokenRequest
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
