using Microsoft.AspNetCore.Identity;

namespace app.Server.Models
{
    public class User: IdentityUser
    {
        public string GoogleId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
    }
}
