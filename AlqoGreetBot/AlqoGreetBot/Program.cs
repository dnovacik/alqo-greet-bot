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
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient Client { get; set; }
        private CommandService Commands { get; set; }
        private IServiceProvider Services { get; set; }
        private List<SocketGuildUser> TeamMembers { get; set; }
            = new List<SocketGuildUser>();
        private SocketGuild DesignatedGuild { get; set; }

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
            this.Client.Connected += HandleClientConnected;

            await this.Client.LoginAsync(TokenType.Bot, botToken);

            await this.Client.StartAsync();

            await Task.Delay(-1);
        }

        private Task HandleClientConnected()
        {
            if (this.Client.ConnectionState == ConnectionState.Connected)
            {
                this.DesignatedGuild = this.Client.GetGuild(372413418399072256); //alqo guild id

                this.TeamMembers = this.DesignatedGuild?.Users
                    .Where(u => u.Roles.Any(r => r.Name.Equals("founder") || r.Name.Equals("enthusiast")))
                    .ToList();
            }

            return Task.CompletedTask;
        }

        private async Task HandleUserUpdated(SocketUser oldUser, SocketUser newUser)
        {
            if (this.IsUsersNameProhibited(newUser.Username))
            {
                await this.DesignatedGuild.AddBanAsync(newUser);

            }
        }

        private bool IsUsersNameProhibited(string userName)
        {
            return this.TeamMembers.Any(m => m.Username == userName);
        }

        private async Task HandleUserJoined(SocketGuildUser user)
        {
            if (this.IsUsersNameProhibited(user.Username))
            {
                await user.SendMessageAsync("Sorry, you are being banned for trying to impersonate a team member!");
                await this.DesignatedGuild.AddBanAsync(user);
            }
            else
            {
                await user.SendMessageAsync($"Hello {user.Username}! Welcome to the ALQO [XLQ] discord channel. Please check our {this.DesignatedGuild.GetTextChannel(DiscordDataConstants.RulesChannel).Mention}.");
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(DateTime.Now + " " + msg);

            return Task.CompletedTask;
        }
    }
}
