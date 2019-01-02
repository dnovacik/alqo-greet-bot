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
    using AlqoGreetBot.Services;
    using AlqoGreetBot.Services.Interfaces;

    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private string BotToken => "";
        private DiscordSocketClient Client { get; set; }
        private CommandService Commands { get; set; }
        private IServiceProvider Services { get; set; }

        public async Task RunBotAsync()
        {
            if (this.Client != null)
            {
                this.Client.Dispose();
            }

            this.Client = new DiscordSocketClient();
            this.Commands = new CommandService();

            this.Services = new ServiceCollection()
                .AddSingleton(this.Client)
                .AddSingleton(this.Commands)
                .AddScoped<IExplorerModuleService, ExplorerModuleService>()
                .BuildServiceProvider();

            this.Client.Log += Log;
            this.Client.UserJoined += this.HandleUserJoined;
            this.Client.Disconnected += this.HandleDisconnected;
            this.Client.MessageReceived += this.HandleCommand;

            await this.RegisterCommandsAsync();
            await this.Client.LoginAsync(TokenType.Bot, this.BotToken);
            await this.Client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task HandleUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;

            await user.SendMessageAsync($"Hello {user.Username}! Welcome to the ALQO [XLQ] discord channel. Please check our {guild.GetTextChannel(DiscordDataConstants.RulesTestChannel).Mention}.");
        }

        private async Task HandleDisconnected(Exception e)
        {
            if (this.Client.ConnectionState == ConnectionState.Disconnected)
            {
                await this.RunBotAsync();
            }
        }

        private async Task HandleCommand(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage msg) || msg.Author.IsBot)
            {
                return;
            }

            var argPos = 0;

            if (msg.HasStringPrefix("//", ref argPos) || msg.HasMentionPrefix(this.Client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(this.Client, msg);
                var result = await this.Commands.ExecuteAsync(context, argPos, this.Services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    await msg.Channel.SendMessageAsync($"{msg.Author.Mention} Sorry dude, unknown command");
                }
            }
        }

        public async Task RegisterCommandsAsync()
        {
            await this.Commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(DateTime.Now + " " + msg);

            return Task.CompletedTask;
        }
    }
}
