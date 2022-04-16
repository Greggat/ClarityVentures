using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClarityVentures
{
    public enum SendAttemptStatus
    {
        Sending,
        Sent,
        Failed
    }
    public class Email
    {
        [Key]
        public int EmailId { get; set; }
        public string Sender { get; set; } = null!;
        public string Recipient { get; set; } = null!;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public DateTime Date { get; set; } = default;
        public SendAttemptStatus Status { get; set; } = SendAttemptStatus.Sending;
    }
}
