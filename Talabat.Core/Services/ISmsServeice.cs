using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.EmailSettings;
using Twilio.Rest.Api.V2010.Account;

namespace Talabat.Core.Services
{
    public interface ISmsServeice
    {
        MessageResource Send(SmsMessages sms);
    }
}
