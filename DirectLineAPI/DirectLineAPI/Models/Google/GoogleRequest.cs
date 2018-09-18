using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectLineAPI.Models.Google
{
    public class GoogleRequest
    {
        public string responseID { get; set; }
        public string session { get; set; }
        public QueryResult queryResult { get; set; }
        public object originalDetectIntentRequest { get; set; }


    }
    public class QueryResult
    {
        public string queryText { get; set; }
        public string languageCode { get; set; }
        public float speechRecognitionConfidence { get; set; }
        public string action { get; set; }
        public object parameters { get; set; }
        public bool allRequiredParamsPresent { get; set; }
        public string fulfillmentText { get; set; }
        public List<object> fulfillmentMessages { get; set; }
        public string webhookSource { get; set; }
        public object webhookPayload { get; set; }
        public List<Context> outputContexts { get; set; }
        public Intent intent { get; set; }
        public float intentDetectionConfidence { get; set; }
        public object diagnosticInfo { get; set; }

    }
    public class Intent
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public Types.WebhookState webhookStatse { get; set; }
        public int priority { get; set; }
        public bool isFallback { get; set; }
        public bool mlDisabled { get; set; }
        public List<string> inputContextNames { get; set; }
        public List<string> events { get; set; }
        public List<Types.TrainingPhrase> trainingPhrases { get; }
        public string action { get; set; }
        public List<Context> outputContexts { get; set; }
        public bool resetContexts { get; set; }
        public List<object> messages { get; }
        //public List<Types.Message.Types.Platform> defaultResponsePlatforms { get; }
        public string rootFollowupIntentName { get; set; }
        public string parentFollowupIntentName { get; set; }
        public List<Types.FollowupIntentInfo> followupIntentInfo { get; }

        public static class Types
        {
            public enum WebhookState
            {
                Unspecified = 0,
                Enabled = 1,
                EnabledForSlotFilling = 2
            }
            public sealed class TrainingPhrase
            {
                public List<Types.Part> parts { get; }
                public int timesAddedCount { get; set; }
                public Types.Type type { get; set; }
                public string name { get; set; }

                public static class Types
                {
                    public enum Type
                    {
                        Unspecified = 0,
                        Example = 1,
                        Template = 2
                    }

                    public sealed class Part
                    {
                        public string entityType { get; set; }
                        public string text { get; set; }
                        public bool userDefined { get; set; }
                        public string alias { get; set; }
                    }
                }
            }
            public sealed class Parameters
            {
                public bool mandatory { get; set; }
                public string entityTypeDisplayName { get; set; }
                public string defaultValue { get; set; }
                public string value { get; set; }
                public string displayName { get; set; }
                public string name { get; set; }
                public bool isList { get; set; }
                public List<string> prompts { get; }
            }
            public sealed class Message
            {
                public Types.Platform platform { get; set; }
                public Types.Text text { get; set; }
                public Types.Image image { get; set; }
                public Types.QuickReplies quickReplies { get; set; }
                public Types.Card card { get; set; }
                public object payload { get; set; }
                public Types.Suggestions suggestions { get; set; }
                public Types.LinkOutSuggestion linkOutSuggestion { get; set; }
                public Types.ListSelect listSelect { get; set; }
                public Types.BasicCard basicCard { get; set; }
                public Types.CarouselSelect carouselSelect { get; set; }
                public Types.SimpleResponses simpleResponses { get; set; }


                public static class Types
                {
                    public enum Platform
                    {
                        Unspecified = 0,
                        Facebook = 1,
                        Slack = 2,
                        Telegram = 3,
                        Kik = 4,
                        Skype = 5,
                        Line = 6,
                        Viber = 7,
                        ActionsOnGoogle = 8
                    }
                    public sealed class Text
                    {
                        public List<string> text { get; }
                    }

                    public sealed class Image
                    {
                        public string imageUri { get; set; }
                        public string accessibilityText { get; set; }
                    }
                    public sealed class QuickReplies
                    {
                        public string title { get; set; }
                        public List<string> quickReplies_ { get; }
                    }
                    public sealed class Card
                    {
                        public string imageUri { get; set; }
                        public List<Types.Button> buttons { get; set; }
                        public string subtitle { get; set; }
                        public string title { get; set; }
                        public static class Types
                        {
                            public sealed class Button
                            { 
                                public string text { get; set; }
                                public string postback { get; set; }
                            }
                        }
                    }
                    public sealed class SimpleResponse
                    {
                        public string textToSpeech { get; set; }
                        public string ssml { get; set; }
                        public string displayText { get; set; }

       
                    }
                    public sealed class SimpleResponses
                    {
                        public List<SimpleResponse> simpleResponses { get; set; }
                    }
                    
                    public sealed class BasicCard
                    {
                        public string title { get; set; }
                        public Image image { get; set; }
                        public string formattedText { get; set; }
                        public string subtitle { get; set; }
                        public List<Types.Button> buttons { get; }

                        public static class Types
                        {
                            public sealed class Button
                            {
                                public Types.OpenUriAction openUriAction { get; set; }
                                public string title { get; set; }

                                public static class Types
                                {
                                    public sealed class OpenUriAction
                                    {                       
                                        public string uri { get; set; }
                                    }
                                }
                            }
                        }
                    }
                    public sealed class Suggestion
                    {
                        public string title { get; set; }
                    }

                    public sealed class Suggestions
                    {
                        public List<Suggestion> suggestions { get; set; }

                    }

                    public sealed class LinkOutSuggestion
                    {
                        public string destinationName { get; set; }
                        public string uri { get; set; }
                    }

                    public sealed class ListSelect
                    {
                        public List<Types.Item> items { get; }
                        public string title { get; set; }

                        public static class Types
                        {
                            public sealed class Item
                            {
                                public string title { get; set; }
                                public SelectItemInfo info { get; set; }
                                public Image image { get; set; }
                                public string description { get; set; }
                            }
                        }
                    }
                    public sealed class CarouselSelect
                    {
                        public List<Types.Item> items { get; set; }

                        public static class Types
                        {
                            public sealed class Item
                            {
                                public string title { get; set; }
                                public SelectItemInfo info { get; set; }
                                public Image image { get; set; }
                                public string description { get; set; }
                            }
                        }
                    }
                    public sealed class SelectItemInfo
                    {
                        public string key { get; set; }
                        public List<string> synonyms { get; set; }

                    }
                }
            }
            public sealed class FollowupIntentInfo
            {
                public string followupIntentName { get; set; }
                public string parentFollowupIntentName { get; set; }
            }
        }
    }
    public class Context
    {
        public string name { get; set; }
        public float lifespanCount { get; set; }
        public object parameters { get; set; }
    }

}
