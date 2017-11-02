using BotDiscord.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BotDiscord
{
    class Program
    {
        private CommandService Commands;
        private DiscordSocketClient Client;
        private IServiceProvider Services;

        //string token = "MzcyMjY0MjAxNzU1OTUxMTA1.DNBpsA.T_rGF62MljARRi5PNnrhXLAAvbc"; development bot
        string Token = "MzczMDIwMzUxOTM4MTY2Nzg2.DNMp0w.Hmp30HTLYj1mkoPt8Hb3eS8Nktc";//zaphkiel
        ulong DefaultChannel = 371949012724744195;
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        public async Task Start()
        {
            Commands = new CommandService();
            Client = new DiscordSocketClient();
            var builder = new ServiceCollection();

            builder.AddTransient<BasicService>();
            Services = builder.BuildServiceProvider();

            await InstallCommands();

            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            await Client.SetGameAsync("God | ?help");

            Client.Connected += Connected;
            Client.LoggedIn += Connected;
            Client.Log += Log;
            Client.UserJoined += UserJoined;
            Client.UserLeft += UserLeft;
            Client.Disconnected += Disconnected;
            
            await Task.Delay(-1);
        }
        public async Task UserJoined(SocketGuildUser user)
        {
            //ID Channel
            var channel = Client.GetChannel(DefaultChannel) as SocketTextChannel;
            await channel.SendMessageAsync($"Hey {user.Mention}! <:snorlax:375495781303582730>");
        }

        public async Task UserLeft(SocketGuildUser user)
        {
            var channel = Client.GetChannel(DefaultChannel) as SocketTextChannel;
            await channel.SendMessageAsync($"Goodbye {user.Mention}, we will miss you <:mewtwo:375496493119045634>");
        }
        public async Task Disconnected(Exception e)
        {
            var channel = Client.GetChannel(DefaultChannel) as SocketTextChannel;
            await channel.SendMessageAsync($"Sayonara <:donaldtrump:375318209014005760>");
        }
        public async Task Connected()
        {
            var channel = Client.GetChannel(DefaultChannel) as SocketTextChannel;
            await channel.SendMessageAsync($"Hi guys, I'm back online <:poke_pika_wink:375316642827337749>");
        }
        public async Task InstallCommands()
        {
            
            Client.MessageReceived += HandleCommand;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }
        public async Task HandleCommand(SocketMessage msgParam)
        {
            var msg = msgParam as SocketUserMessage;
            var prefix = '?';
            var systemPrefix = '!';
            var argPos = 0;

            if (msg == null)
            {
                return;
            }

            if ((msg.HasCharPrefix(systemPrefix, ref argPos))||(msg.HasCharPrefix(prefix, ref argPos) || msg.HasMentionPrefix(Client.CurrentUser, ref argPos)) == false)
            {
                return;
            }
            var context = new CommandContext(Client, msg);
            var result = await Commands.ExecuteAsync(context, argPos, Services);

            var channelLog = Client.GetChannel(372360498177507338) as SocketTextChannel;
            if (result.IsSuccess == false)
            {
                await context.Channel.SendMessageAsync($@"```css
[{result.ErrorReason}]```");

                await channelLog.SendMessageAsync($@"```css
Error Log by {context.User.Username} in #{context.Channel.Name} => {msg.Content}
[{result.ErrorReason}]```");
                //await context.Message.DeleteAsync();
            }
            else
            {
                await channelLog.SendMessageAsync($@"```css
Command Log by {context.User.Username} in #{context.Channel.Name} => {msg.Content}```");
                
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString(null, false));
            return Task.CompletedTask;
        }
    }
}
