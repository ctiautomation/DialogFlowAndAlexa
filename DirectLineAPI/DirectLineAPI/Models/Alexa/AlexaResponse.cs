using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectLineAPI.Models.Alexa;

namespace DirectLineAPI.Models.Alexa
{
    public class AlexaResponse
    {
        public AlexaResponse(string reply)
        {
            version = "1.0"; //Check this
            response = new Response()
            {
                outputSpeech = new Outputspeech
                {
                    type = "SSML",
                    ssml = string.Format("<speak> {0} </speak>", reply),
                    text = reply
                },
                shouldEndSession = false
            };
        }
      
        public string version { get; set; }
        public Header header { get; set; }
        public Response response { get; set; }

    }

    public class AlexaDirective
    {
        public Device device { get; set; }
        public Header header { get; set; }
        public Directive directive { get; set; }
        public Session session { get; set; }
        public Application application { get; set; }
    }

    public class Header
    {
        public string requestId { get; set; }
    }

    public class Response
    {
        public Outputspeech outputSpeech { get; set; }
        public bool shouldEndSession { get; set; }
    }
    public class Directive
    {
        public Outputspeech outputspeech { get; set; }
        public string speech { get; set; }
    }

    public class Outputspeech
    {
        public string type { get; set; }
        public string text { get; set; }
        public string ssml { get; set; }
    }
}
