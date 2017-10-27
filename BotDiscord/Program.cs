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
        private CommandService commands;
        private DiscordSocketClient client;
        private IServiceProvider services;

        //string token = "MzcyMjY0MjAxNzU1OTUxMTA1.DNBpsA.T_rGF62MljARRi5PNnrhXLAAvbc"; development bot
        string token = "MzczMDIwMzUxOTM4MTY2Nzg2.DNMp0w.Hmp30HTLYj1mkoPt8Hb3eS8Nktc";//zaphkiel
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        public async Task Start()
        {
            commands = new CommandService();
            client = new DiscordSocketClient();
            var builder = new ServiceCollection();

            builder.AddTransient<BasicService>();
            services = builder.BuildServiceProvider();
            
            await InstallCommands();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await client.SetGameAsync("God | ?help");
            
            client.Log += Log;
            client.UserJoined += UserJoined;
            client.UserLeft += UserLeft;
            client.Disconnected += Disconnected;
            
            await Task.Delay(-1);
        }
        public async Task UserJoined(SocketGuildUser user)
        {
            //ID Channel
            var channel = client.GetChannel(371949012724744195) as SocketTextChannel;
            await channel.SendMessageAsync($"Hey {user.Mention}! <:AkarinWave:370855004921135104>");
        }

        public async Task UserLeft(SocketGuildUser user)
        {
            var channel = client.GetChannel(371949012724744195) as SocketTextChannel;
            await channel.SendMessageAsync($"Goodbye {user.Mention}, we will miss you");
        }
        public async Task Disconnected(Exception e)
        {
            var channel = client.GetChannel(371949012724744195) as SocketTextChannel;
            await channel.SendMessageAsync($"Sayonara <:AS003:371971289554092046>");
        }
        public async Task InstallCommands()
        {
            //client.LoggedIn
            
            client.MessageReceived += HandleCommand;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }
        public async Task HandleCommand(SocketMessage msgParam)
        {
            var msg = msgParam as SocketUserMessage;
            var prefix = '?';
            var systemPrefix = '!';
            

            if (msg == null)
            {
                return;
            }

            var argPos = 0;
            
            if ((msg.HasCharPrefix(systemPrefix, ref argPos))||(msg.HasCharPrefix(prefix, ref argPos) || msg.HasMentionPrefix(client.CurrentUser, ref argPos)) == false)
            {
                return;
            }
            var context = new CommandContext(client, msg);
            var result = await commands.ExecuteAsync(context, argPos, services);

            var channelLog = client.GetChannel(372360498177507338) as SocketTextChannel;
            if (result.IsSuccess == false)
            {
                await context.Channel.SendMessageAsync($@"```css
[{result.ErrorReason}]```");

                await channelLog.SendMessageAsync($@"```css
Error Log by {context.User.Username} => {msg.Content}
[{result.ErrorReason}]```");
            }
            else
            {
                await channelLog.SendMessageAsync($@"```css
Command Log by {context.User.Username} => {msg.Content}```");
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
