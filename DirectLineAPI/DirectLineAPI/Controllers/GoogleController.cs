using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DirectLineAPI.Models;
using DirectLineAPI.Models.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DirectLineAPI.Controllers
{
    [Route("api/[controller]")]
    public class GoogleController : Controller
    {
        private readonly ILogger _logger;
        private IScorpioDirectLineClient _client;

        public GoogleController(ILogger<GoogleController> logger, IScorpioDirectLineClient client )
        {
            _logger = logger;
            _client = client;
        }

        // GET api/google
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Google", "API" };
        }

        // POST api/google
        [HttpPost]
        public ContentResult Post([FromBody] GoogleRequest value)
        {
            var query = value.queryResult.queryText;
            var sessionId = value.session;
            var intent = value.queryResult.intent;

            _client.SendBotMessage(query, sessionId).Wait();

            var replies = _client.ReadBotMessages(sessionId).Result;

            //TODO: This pulls the last message sent by the bot. Could break for multiple messages sent 
            var reply = replies.LastOrDefault();

            var response = new GoogleResponse(reply);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            var res = json.ToString();
            return Content(json.ToString(), "application/json");
        }
    }
}
