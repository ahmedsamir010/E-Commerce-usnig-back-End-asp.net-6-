using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.EmailSettings;
using Talabat.Core.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Talabat.Service
{
    public class SmsService : ISmsServeice
    {
        private readonly TwilloSettings options;

        public SmsService(IOptions<TwilloSettings> options)
        {
            this.options = options.Value;
        }

        public MessageResource Send(SmsMessages sms)
        {
            TwilioClient.Init(options.AccoutsId, options.AuthToken);
            var result = MessageResource.Create(
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(options.TwilloPhoneNumber),
                to: sms.PhoneNumber
                );

            return result;
        }
    }
}
