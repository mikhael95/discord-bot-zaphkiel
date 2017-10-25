using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using BotDiscord.Services;

namespace BotDiscord
{
    public class BotController : ModuleBase
    {

        private readonly BasicService BasicService;
        private readonly AudioService AudioService;

        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public BotController(BasicService basicService, AudioService audioService)
        {
            this.BasicService = basicService;
            this.AudioService = audioService;
        }

        [Command("Help")]
        [Summary("Showing Help Commands")]
        [Alias("h")]
        public async Task Help()
        {
            await ReplyAsync(@"```css
Commands:
?del [number_of_message] - deleting message 
?help - showing all commands available
?info [mention] - showing info of specific user
?infoGuild / ?ig - showing guild info
?stop - stopping bot process
?tube [keyword] - search music in youtube [NOT WORKING]
?search or s [keyword]- use google
```
");
        }
        
        [Command("Play", RunMode = RunMode.Async)]
        public async Task PlayCmd([Remainder] string song)
        {
            await this.AudioService.SendAudioAsync(Context.Guild, Context.Channel, song);
        }
        
        [Command("Search", RunMode = RunMode.Async)]
        [Summary("Search keyword with google api")]
        [Alias("s")]
        public async Task Google(string keyword)
        {
            var searchResult = await this.BasicService.GoogleSearch(keyword);
            await ReplyAsync(searchResult.Snippet);
            await ReplyAsync(searchResult.Link);
        }

        [Command("Info")]
        [Summary("Get User Info")]
        public async Task CheckInfo(IGuildUser user)
        {
            await ReplyAsync(BasicService.GetInfo(user));
        }

        [Command("Info")]
        [Priority(1)]
        [Summary("Get User Info")]
        public async Task CheckInfo()
        {
            var server = Context.Guild;
            var user = await server.GetUserAsync(Context.User.Id);
            await ReplyAsync(BasicService.GetInfo(user));

        }

        [Command("InfoGuild")]
        [Priority(2)]
        [Summary("Get User Info")]
        [Alias("ig")]
        public async Task CheckInfoGuild()
        {
            await ReplyAsync(BasicService.GetInfoGuild(Context.Guild));
        }

        [Command("Stop", RunMode = RunMode.Async)]
        [Summary("Stoping bot process")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ShutDown()
        {
            var client = Context.Client;
            var message = await ReplyAsync("Stoping bot process in 5 seconds...");
            for (int i = 4; i > 0; i--)
            {
                await message.ModifyAsync(Q => Q.Content = $"Stoping bot process in {i} seconds...");
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

            var m = await this.ReplyAsync($"Delete messages success");
            await Task.Delay(2000);
            await m.DeleteAsync();
        }
    }
}
