using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DirectLineAPI.Models.Alexa;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DirectLineAPI.Models;
using Microsoft.Extensions.Logging;

namespace DirectLineAPI.Controllers
{
    [Route("api/[controller]")]
    public class AlexaController : Controller
    {
        private readonly ILogger _logger;
        private IScorpioDirectLineClient _client;

        public AlexaController(ILogger<AlexaController> logger, IScorpioDirectLineClient client)
        {
            _logger = logger;
            _client = client;
        }

        // GET api/alexa
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Alexa", "API" };
        }

        [HttpPost]
        public async Task<ContentResult> Post([FromBody] AlexaRequest alexaRequestObject)
        {
            var intent = alexaRequestObject?.request?.intent?.name;
            var query = alexaRequestObject?.request?.intent?.slots?.phrase?.value;
            var sessionId = alexaRequestObject?.session?.sessionId;

            //Should change this. 
            //When Alexa is invocted, it sends a null intent so we change this to a welcome message
            if (intent == null && query == null)
                query = "Welcome";

            _client.SendBotMessage(query, sessionId).Wait();
            var replies = _client.ReadBotMessages(sessionId).Result;

            //TODO: This pulls the last message sent by the bot. Could break for multiple messages sent 
            var reply = replies.LastOrDefault();

            var response = new AlexaResponse(reply);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            var res = json.ToString();
            return Content(json.ToString(), "application/json");
        }
    }
}