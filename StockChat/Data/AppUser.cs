using Microsoft.AspNetCore.Identity;

namespace StockChat.Data
{
    public class AppUser: IdentityUser
    {
        public virtual ICollection<Message> Messages { get; set; }

        public AppUser()
        {
            Messages = new HashSet<Message>();
        }

    }
}
