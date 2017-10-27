using System;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using BotDiscord.Services;

namespace BotDiscord
{
    public class BotController : ModuleBase
    {

        private readonly BasicService BasicService;
        private EmbedBuilder Embed;
        bool TTS = false;
        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public BotController(BasicService basicService)
        {
            this.BasicService = basicService;
        }

        [Command("Help")]
        [Summary("Showing Help Commands")]
        [Alias("h")]
        public async Task Help()
        {
            Embed = new EmbedBuilder()
                .WithColor(Discord.Color.Orange)
                .WithAuthor("Commands")
                .WithDescription("This bot is dedicated to search document, webpage, image, music,or video")
                .AddField("?help or ?h", "show all commands available")
                .AddInlineField("img [keyword]", "search image")
                .AddInlineField("doc [keyword]", "search document")
                .AddInlineField("play [keyword]", "play video")
                .AddInlineField("del [number]", "delete message")
                .AddInlineField("info [mention]", "show user information")
                .AddInlineField("ig", "show guild info")
                .AddInlineField("stop", "stop bot process")
                .AddField("nsfw", "Use at your own risk")
                .WithFooter("This Bot is made by Elmiel")
                .WithCurrentTimestamp();
            await ReplyAsync("", TTS, Embed);
        }

        [Command("SearchImage", RunMode = RunMode.Async)]
        [Summary("Search keyword with google api")]
        [Alias("img", "i")]
        public async Task GoogleImg([Remainder]string keyword)
        {
            var searchResult = await this.BasicService.GoogleSearchImg(keyword);
            if (searchResult == null)
            {
                await ReplyAsync($"Your search - ***{keyword}*** - did not match any documents.");
            }
            else
            {
                //searchResult.Count
                var rand = new Random();
                var result = rand.Next(0, searchResult.Count);
                await ReplyAsync(searchResult[result].Snippet);
                await ReplyAsync(searchResult[result].Link);
            }
        }

        [Command("SearchDoc", RunMode = RunMode.Async)]
        [Summary("Search keyword with google api")]
        [Alias("doc", "s")]
        public async Task GoogleDoc([Remainder]string keyword)
        {
            var searchResult = await this.BasicService.GoogleSearchDoc(keyword);
            if (searchResult == null)
            {
                await ReplyAsync($"Your search - ***{keyword}*** - did not match any documents.");
            }
            else
            {
                //searchResult.Count
                var rand = new Random();
                var result = rand.Next(0, searchResult.Count);
                await ReplyAsync(searchResult[result].Snippet);
                await ReplyAsync(searchResult[result].Link);
            }
        }

        [Command("Play", RunMode = RunMode.Async)]
        [Summary("Play video from youtube")]
        [Alias("p")]
        public async Task PlayVideo([Remainder]string keyword)
        {
            var searchResult = await this.BasicService.GoogleSearchVid(keyword);
            if (searchResult == null)
            {
                await ReplyAsync($"Your search - ***{keyword}*** - did not match any documents.");
            }
            else
            {
                //searchResult.Count
                var rand = new Random();
                var result = rand.Next(0, searchResult.Count);
                await ReplyAsync(searchResult[result].Snippet);
                await ReplyAsync($"{searchResult[result].Link}");
            }
        }

        [Command("Info")]
        [Priority(1)]
        [Summary("Get User Info")]
        public async Task CheckInfo()
        {
            //var server = Context.Guild;
            //var user = await server.GetUserAsync(Context.User.Id);
            await ReplyAsync("", TTS, BasicService.GetInfo(await Context.Guild.GetUserAsync(Context.User.Id)));
        }

        [Command("Info")]
        [Priority(2)]
        [Summary("Get User Info")]
        public async Task CheckInfo(IGuildUser user)
        {
            await ReplyAsync("", TTS, BasicService.GetInfo(user));

        }

        [Command("InfoGuild")]
        [Summary("Get User Info")]
        [Alias("ig")]
        public async Task CheckInfoGuild()
        {
            var m = await ReplyAsync("",TTS,BasicService.GetInfoGuild(Context.Guild));
        }

        [Command("Stop", RunMode = RunMode.Async)]
        [Summary("Stoping bot process")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task ShutDown()
        {
            var client = Context.Client;
            var message = await ReplyAsync("Stoping bot process in ***5*** seconds...");
            for (int i = 4; i > 0; i--)
            {
                await message.ModifyAsync(Q => Q.Content = $"Stoping bot process in ***{i}*** seconds...");
                await Task.Delay(1000);
            }
            await message.DeleteAsync();
            await client.StopAsync();
        }

        [Command("Delete", RunMode = RunMode.Async)]
        [Summary("Deletes the specified amount of messages.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [Alias("del")]
        public async Task DeleteChat(uint amount)
        {
            var messages = await this.Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();

            await this.Context.Channel.DeleteMessagesAsync(messages);

            var m = await this.ReplyAsync("~~Messages deleted~~");
            await Task.Delay(2000);
            await m.DeleteAsync();
        }
        [Command("nsfw", RunMode = RunMode.Async)]
        [Summary("Use at your own risk")]
        [RequireNsfw]
        public async Task NSFW([Remainder]string keyword)
        {
            var searchResult = await this.BasicService.NSFW(keyword);
            if (searchResult == null)
            {
                await ReplyAsync($"Your search - ***{keyword}*** - did not match any documents.");
            }
            else
            {
                //searchResult.Count
                var rand = new Random();
                var result = rand.Next(0, searchResult.Count);
                await ReplyAsync(searchResult[result].Snippet);
                await ReplyAsync(searchResult[result].Link);
            }
        }
    }
}
