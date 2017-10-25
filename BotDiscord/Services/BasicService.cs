using Discord;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Google.Apis;
using Google.Apis.Services;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BotDiscord.Models;

namespace BotDiscord.Services
{
    public class BasicService
    {
        public string GetInfo(IGuildUser user)
        {
            return $@"```css
User: {user.ToString()}
NickName: {user.Nickname}
User Id: {user.Id}
Bot: {user.IsBot}
Game: {user.Game}
Status: {user.Status}```";
        }
        public string GetInfoGuild(IGuild guild)
        {
            return $@"```css
Guild ID: {guild.Id}
Guild: {guild.ToString()}
Audio Client: {guild.AudioClient}
Created At: {guild.CreatedAt}
Voice Region ID: {guild.VoiceRegionId}
```";
        }
        public async Task<GoogleViewModel> GoogleSearch(string keyword)
        {
            var client = new HttpClient();
            var content = await client.GetAsync($"https://www.googleapis.com/customsearch/v1?key=AIzaSyDJ9CUdfCsjkDIYId3fmfvKVGjbHk48hR8&cx=010065256725983153448:itwnk8vx56k&q={keyword}&num=5&searchType=image");
            var response = JsonConvert.DeserializeObject<SearchViewModel>(await content.Content.ReadAsStringAsync());

            return response.Items[0];
            //var response = await client.GetStringAsync(uri);

        }
    }
}
