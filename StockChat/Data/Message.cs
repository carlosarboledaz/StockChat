using System.ComponentModel.DataAnnotations;

namespace StockChat.Data
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public int Text { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public virtual AppUser Sender { get; set; }

    }
}
