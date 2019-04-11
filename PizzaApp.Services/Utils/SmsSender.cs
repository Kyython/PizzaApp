using PizzaApp.Services.Abstract;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PizzaApp.Services.Utils
{
    public class SmsSender : Sender
    {
        public void SendSms(string phoneNumber)
        {
            const string ACCOUNT_SID = "AC79e8b6906062b55a82552b7fd9d55362";
            const string AUTHENTICATION_TOKEN = "b99e8edb57edbb434512ecb038964748";
            TwilioClient.Init(ACCOUNT_SID, AUTHENTICATION_TOKEN);

            CreateMessage();

            var message = MessageResource.Create(
                body: "Код подтверждения: " + Message,
                from: new Twilio.Types.PhoneNumber("+16514484179"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }
    }
}
