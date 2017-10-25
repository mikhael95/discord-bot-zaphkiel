using System;
using System.Collections.Generic;
using System.Text;

namespace BotDiscord.Services
{
    public class YoutubeService
    {
        
        public string GetYouTubeResult(string keyword)
        {

            //var listRequest = youtube.Search.List("snippet");
            //listRequest.Q = "Loeb Pikes Peak";
            //listRequest.MaxResults = 5;
            //listRequest.Type = "video";
            //var resp = listRequest.Execute();
            ////foreach (SearchResult result in resp.Items)
            ////{
            ////    Console.WriteLine(result.Snippet.Title);
            ////}
            ////https://www.googleapis.com/youtube/v3/videos?id=7lCDEYXw3mM&key=YOUR_API_KEY
            //&part = snippet,contentDetails,statistics,status
            return $"https://www.youtube.com/results?search_query=" + keyword;
        }
    }
}
