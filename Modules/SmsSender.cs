using Ideas.Models;
using SMSApi.Api;
using smsapi.Api.Response.REST.Exception;
using Microsoft.AspNetCore.Http;

namespace Ideas.Modules
{
    public class SmsSender
    {
        private readonly string _token;
        private readonly ProductReviewContext _context;

        public SmsSender(string token, ProductReviewContext context)
        {
            _token = token;
            _context = context;
        }

        public void SendMessage(string phoneNumber)
        {
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            DateTimeOffset futureTime = currentTime.AddDays(7);
            long unixTimestamp = futureTime.ToUnixTimeSeconds();
            string dateString = unixTimestamp.ToString();

            try
            {
                var client = new ClientOAuth(_token);
                var smsfactory = new SMSFactory(client);

                var sms = smsfactory.ActionSend()
                    .SetTo(phoneNumber)
                    .SetText("Masz nowa ankiete do wypelnienia")
                    .SetDateSent(dateString)
                    .Execute();
            }
            catch (ValidationException ex)
            {
                var errors = ex.ValidationErrors;
            }
        }

        public void SendPromocode(string phoneNumber)
        {
            try
            {
                var client = new ClientOAuth(_token);
                var smsfactory = new SMSFactory(client);

                var promocodes = new Promocodes(_context);

                var promocode = promocodes.CreatePromocode();

                string formatedString = String.Format("Dziekujemy za wspolprace! Skorzystaj z {0} znizki na wszystkie zakupy, korzystając z kodu {1} na naszej stronie internetowej. Kod jest wazny przez 7 dni.", promocode.Discount, promocode.Code);

                var sms = smsfactory.ActionSend()
                    .SetTo(phoneNumber)
                    .SetText(formatedString)
                    .Execute();
            }
            catch (ValidationException ex)
            {
                var errors = ex.ValidationErrors;
            }
        }
    }
}
