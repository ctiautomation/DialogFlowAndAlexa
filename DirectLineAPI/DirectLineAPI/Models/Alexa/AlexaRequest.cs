using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirectLineAPI.Models.Alexa
{
    public class AlexaRequest
    {
        public Session session { get; set; }
        public Request request { get; set; }
        public string version { get; set; }
        public Context context { get; set; }
    }
    public class Session
    {
        public string sessionId { get; set; }
        public Application application { get; set; }
        public Attributes attributes { get; set; }
        public User userId { get; set; }
        public bool _new { get; set; }
    }
    public class Request
    {
        public string type { get; set; }
        public string requestId { get; set; }
        public string locale { get; set; }
        public DateTime timestamp { get; set; }
        public Intent intent { get; set; }
        public bool shouldLinkResultBeReturned { get; set; }
    }

    public class Context
    {
        public System system { get; set; }
    }
    public class Application
    {
        public string applicationId { get; set; }
    }

    public class Attributes
    {
    }
    public class User
    {
        public string userId { get; set; }
    }


    public class Intent
    {
        public string name { get; set; }
        public string confirmationStatus { get; set; }
        public Slots slots { get; set; }
    }

    public class Slots
    {
        public Phrase phrase { get; set; }
        public string confirmationStatus { get; set; }
    }

    public class Phrase
    {
        public string name { get; set; }
        public string value { get; set; }
        public Resolutions resolutions { get; set; }
    }

    public class Resolutions
    {
        public List<ResolutionsPerAuthority> resolutionsPerAuthorities { get; set; }
    }

    public class ResolutionsPerAuthority
    {
        public string authority { get; set; }
        public Status status { get; set; }

    }

    public class Status
    {
        public string code { get; set; }
    }
    public class System
    {
        public Application application { get; set; }
        public User user { get; set; }
        public Device device { get; set; }
        public string apiEndpoint { get; set; }
        public string apiAccessToken { get; set; }
    }

    public class Device
    {
        public string deviceId { get; set; }
        public List<string> supportedInteraces { get; set; }
    }

    public class Year
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}