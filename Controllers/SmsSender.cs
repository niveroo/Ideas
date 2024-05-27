using SMSApi.Api;
using smsapi.Api.Response.REST.Exception;

namespace Ideas.Controllers
{
    public class SmsSender
    {
        private readonly string _token;

        public SmsSender(string token)
        {
            _token = token;
        }
        public void SendMessage(string phoneNumber)
        {
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            DateTimeOffset futureTime = currentTime.AddDays(7);
            long unixTimestamp = futureTime.ToUnixTimeSeconds();
            Console.WriteLine("Unix timestamp after adding 7 days: " + unixTimestamp);
            string dateString = unixTimestamp.ToString();

            try
            {
                var client = new ClientOAuth(_token);
                var smsfactory = new SMSFactory(client);

                var sms = smsfactory.ActionSend()
                    .SetTo(phoneNumber)
                    //.SetSender("SunTrail")                // DOES NOT WORK WITH DEMO
                    .SetText("Masz nowa ankiete do wypelnienia")
                    .SetDateSent(dateString)
                    .Execute();
            }
            catch (ValidationException ex)
            {
                var errors = ex.ValidationErrors;
            }
        }
    }
}
