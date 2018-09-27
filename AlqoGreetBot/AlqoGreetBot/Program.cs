namespace AlqoGreetBot
{
    using AlqoGreetBot.Common.Extensions;
    using AlqoGreetBot.Common.Constants;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient Client { get; set; }
        private CommandService Commands { get; set; }
        private IServiceProvider Services { get; set; }

        public async Task RunBotAsync()
        {
            this.Client = new DiscordSocketClient();
            this.Commands = new CommandService();

            this.Services = new ServiceCollection()
                .AddSingleton(this.Client)
                .AddSingleton(this.Commands)
                .BuildServiceProvider();

            var botToken = "";

            this.Client.Log += Log;
            this.Client.UserJoined += HandleUserJoined;
            this.Client.UserUpdated += HandleUserUpdated;

            await this.Client.LoginAsync(TokenType.Bot, botToken);

            await this.Client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task HandleUserUpdated(SocketUser oldUser, SocketUser newUser)
        {
            if (newUser.Username == "yannick")
            {
                var guild = this.Client.GetGuild(372413418399072256); //alqo guild id
                
                if (guild != null)
                {
                    await guild.AddBanAsync(newUser);
                }
            }
        }

        private async Task HandleUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;

            await user.SendMessageAsync($"Hello {user.Username}! Welcome to the ALQO [XLQ] discord channel. Please check our {guild.GetTextChannel(DiscordDataConstants.RulesChannel).Mention}.");
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(DateTime.Now + " " + msg);

            return Task.CompletedTask;
        }
    }
}
