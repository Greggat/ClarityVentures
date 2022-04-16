using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClarityVentures.Emailer
{
    public interface IEmailProvider
    {
        public Task<bool> SendEmail(string sender, string recipient, string subject, string body);
    }
}
