using Discord;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BotDiscord.Models;

namespace BotDiscord.Services
{
    public class BasicService
    {
        public EmbedBuilder GetInfo(IGuildUser user)
        {
            return new EmbedBuilder()
                .WithAuthor(user.ToString(), user.GetAvatarUrl())
                .WithColor(Discord.Color.Teal)
                .AddInlineField("Nickname", user.Nickname??"None")
                .AddInlineField("Id", user.Id)
                .AddInlineField("Bot", user.IsBot)
                .AddInlineField("Game", user.Game?.ToString() ?? "Not playing any game")
                .AddField("Status", user.Status);
            

            //            return $@"```css
            //User: {user.ToString()}
            //NickName: {user.Nickname}
            //User Id: #{user.Id}
            //Bot: {user.IsBot}
            //Game: {user.Game}
            //Status: {user.Status}```";
        }
        public EmbedBuilder GetInfoGuild(IGuild guild)
        {
            return new EmbedBuilder()
                .WithAuthor(guild.ToString(), guild.IconUrl)
                .WithColor(Discord.Color.Gold)
                .WithThumbnailUrl(guild.SplashUrl)
                .AddInlineField("Guild Id", guild.Id)
                .AddInlineField("Region", guild.VoiceRegionId)
                .AddInlineField("DOB", guild.CreatedAt);
//            return $@"```css
//Guild ID: #{guild.Id}
//Guild: {guild.ToString()}
//Created At: {guild.CreatedAt}
//Voice Region ID: {guild.VoiceRegionId}
//```";
        }
        public async Task<List<GoogleViewModel>> GoogleSearchImg(string keyword)
        {
            var client = new HttpClient();
            var content = await client.GetAsync($"https://www.googleapis.com/customsearch/v1?key=AIzaSyDJ9CUdfCsjkDIYId3fmfvKVGjbHk48hR8&cx=010065256725983153448:itwnk8vx56k&q={keyword}&searchType=image");
            var response = JsonConvert.DeserializeObject<SearchViewModel>(await content.Content.ReadAsStringAsync());

            return response.Items;
        }
        public async Task<List<GoogleViewModel>> GoogleSearchDoc(string keyword)
        {
            var client = new HttpClient();
            var content = await client.GetAsync($"https://www.googleapis.com/customsearch/v1?key=AIzaSyDJ9CUdfCsjkDIYId3fmfvKVGjbHk48hR8&cx=010065256725983153448:itwnk8vx56k&q={keyword}&num=5");
            var response = JsonConvert.DeserializeObject<SearchViewModel>(await content.Content.ReadAsStringAsync());

            return response.Items;
        }
        public async Task<List<GoogleViewModel>> GoogleSearchVid(string keyword)
        {
            var client = new HttpClient();
            var content = await client.GetAsync($"https://www.googleapis.com/customsearch/v1?key=AIzaSyDJ9CUdfCsjkDIYId3fmfvKVGjbHk48hR8&cx=010065256725983153448:ryq1o2ocdwe&q={keyword}&num=5");
            var response = JsonConvert.DeserializeObject<SearchViewModel>(await content.Content.ReadAsStringAsync());

            return response.Items;
        }
        public async Task<List<GoogleViewModel>> NSFW(string keyword)
        {
            var client = new HttpClient();
            var content = await client.GetAsync($"https://www.googleapis.com/customsearch/v1?key=AIzaSyDJ9CUdfCsjkDIYId3fmfvKVGjbHk48hR8&cx=010065256725983153448:k-zy4985ad4&q={keyword}&searchType=image");
            var response = JsonConvert.DeserializeObject<SearchViewModel>(await content.Content.ReadAsStringAsync());

            return response.Items;
        }
    }
}
