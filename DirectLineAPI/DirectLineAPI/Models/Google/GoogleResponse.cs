using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectLineAPI.Models.Google
{
    public class GoogleResponse
    {
        public GoogleResponse(string _fulfillmentText)
        {
            fulfillmentText = _fulfillmentText;
        }

        public string fulfillmentText { get; set; }
        public List<Intent.Types.Message> fulfillmentMessages { get; set; }
        public string source { get; set; }
        public object payload { get; set; }
        public Context[] outputContexts { get; set; }
        public EventInput followupEventInput { get; set; }

        public class EventInput
        {
            public string name { get; set; }
            public string languageCode { get; set; }
            public object parameters { get; set; }
        }

        public class Payload
        {
            public Googlec google { get; set; }
        }
        public class Googlec
        {
            public bool expectUserResponse { get; set; }
            public RichResponse richResponse { get; set; }
        }
        public class RichResponse
        {
            public Items[] items { get; set; }
        }
        public class Items
        {
            public SimpleResponse simpleResponse {get; set;}
        }
        public class SimpleResponse
        {
            //string displayText = "Test";
            public string textToSpeech { get; set; } 
            //string ssml = $"<speak>{"Test"}</speak>";
        }
    }
}
/**
 * Just putting this here to reference later
 * 
 * 
 * /
//{
//    fulfillmentText = reply,
//    /*fulfillmentMessages = new List<Intent.Types.Message>(new Intent.Types.Message[]
//    {
//        new Intent.Types.Message()
//        {
//            //card = new Intent.Types.Message.Types.Card()
//            //{
//            //    title = "card title",
//            //    subtitle = "card title",
//            //    imageUri = "....",
//            //    buttons = new List<Intent.Types.Message.Types.Card.Types.Button>(new Intent.Types.Message.Types.Card.Types.Button[]
//            //    {
//            //        new Intent.Types.Message.Types.Card.Types.Button()
//            //        {
//            //            text = "button text",
//            //            postback = "....",
//            //        }
//            //    })
//            //}

//            simpleResponses = new Intent.Types.Message.Types.SimpleResponses()
//            {
//                simpleResponses = new List<Intent.Types.Message.Types.SimpleResponse>(new Intent.Types.Message.Types.SimpleResponse[]
//                    { new Intent.Types.Message.Types.SimpleResponse()
//                        {
//                           displayText = reply,
//                           textToSpeech = reply,
//                           ssml = $"<speak>{reply}</speak>"
//                        }
//                    })
//            }
//            //suggestions = new Intent.Types.Message.Types.Suggestions()
//            //{
//            //    suggestions = new List<Intent.Types.Message.Types.Suggestion>(new Intent.Types.Message.Types.Suggestion[]
//            //    {
//            //        new Intent.Types.Message.Types.Suggestion()
//            //        {
//            //            title = "Test Title"
//            //        }
//            //    })
//            //}

//            //carouselSelect = new Intent.Types.Message.Types.CarouselSelect()
//            //{
//            //    items = new List<Intent.Types.Message.Types.CarouselSelect.Types.Item>(new Intent.Types.Message.Types.CarouselSelect.Types.Item[]
//            //    {
//            //        new Intent.Types.Message.Types.CarouselSelect.Types.Item()
//            //        {
//            //            info = new Intent.Types.Message.Types.SelectItemInfo()
//            //            {
//            //                key = "test",
//            //                synonyms = new List<string>() {"hello"}
//            //            },
//            //            title = "Test Title",
//            //            description = "Test",   
//            //        }
//            //    })
//            //}
//        }
//    }),*/
//};