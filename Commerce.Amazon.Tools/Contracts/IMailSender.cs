using Gesisa.SiiCore.Tools.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gesisa.SiiCore.Tools.Contracts
{
    public interface IMailSender
    {
        void SendMail(IdentityMessage message);
        void SendMail(IdentityMessage message, string mailAddressFrom);
    }
}
