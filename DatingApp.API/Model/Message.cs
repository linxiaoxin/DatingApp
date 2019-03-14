using System;

namespace DatingApp.API.Model
{
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int RecipientId { get; set; }
        public User Sender { get; set; }
        public User Recipient { get; set; }

        public string Content { get; set; }
        public bool isRead { get; set; }
        public DateTime? dateRead { get; set; }
        public DateTime DateSent { get; set; }
        public bool senderDelete { get; set; }
        public bool recipientDelete { get; set; }
    }
}