using System;
using System.Collections.Generic;
using System.Text;

namespace BotDiscord.Models
{
    public class GoogleViewModel
    {
        public string Snippet { get; set; }
        public string Link { get; set; }
    }
    public class SearchViewModel
    {
        public List<GoogleViewModel> Items { get; set; }
    }
}
