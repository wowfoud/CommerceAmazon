using Commerce.Amazon.Tools.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Tools.Contracts
{
    public interface IMailSender
    {
        void SendMail(IdentityMessage message);
    }
}
