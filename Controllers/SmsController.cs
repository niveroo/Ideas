using Microsoft.AspNetCore.Mvc;
using SMSApi.Api;
using smsapi.Api.Response.REST.Exception;

namespace SmsController.Controllers.Sms
{
    [Route("api/[controller]")]
    [ApiController]
    public class MFAController : ControllerBase
    {
        private readonly string _token;

        public MFAController()
        {
            _token = "AKPXqKLGHOc2jIipdQEwJvbKu047T8DOWXVINnXZ";
        }

        [HttpPost("sendMFA")]
        public IActionResult SendMfaCode(string phoneNumber)
        {
            try
            {
                var client = new ClientOAuth(_token);
                var features = new Features(client);
                    
                var mfaCode = features.MFA()
                    .CreateMfaCode(phoneNumber)
                    //.FromSendername("SunTrail")                // DOES NOT WORK WITH DEMO
                    .WithContent("Your code is [%code%]")
                    .Execute();

                return Ok(new
                {
                    mfaCode.Id,
                    mfaCode.Code,
                    mfaCode.PhoneNumber,
                    mfaCode.From
                });
            }
            catch (ValidationException ex)
            {
                var errors = ex.ValidationErrors;
                return BadRequest(errors);
            }
            catch (TooManyRequestsException)
            {
                return StatusCode(429, "Too many requests. Please try again later.");
            }
            catch (ClientException ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

            [HttpPost("verifyMFA")]
            public IActionResult VerifyMfaCode(string phoneNumber, string code)
            {
                try
                {
                    var client = new ClientOAuth(_token);

                    var features = new Features(client);

                    var verificationResult = features.MFA()
                        .VerifyMfaCode(phoneNumber, code)
                        .Execute();

                    return Ok(true);
            }
                catch (ValidationException ex)
                {
                    var errors = ex.ValidationErrors;
                    return BadRequest(errors);
                }
                catch (TooManyRequestsException)
                {
                    return StatusCode(429, "Too many requests. Please try again later.");
                }
                catch (ClientException ex)
                {
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
    }
}
