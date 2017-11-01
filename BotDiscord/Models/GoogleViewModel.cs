using System.Collections.Generic;

namespace BotDiscord.Models
{
    public class GoogleViewModel
    {
        public string Title { get; set; }
        public string Snippet { get; set; }
        public string Link { get; set; }
    }
    public class SearchViewModel
    {
        public List<GoogleViewModel> Items { get; set; }
    }
}
