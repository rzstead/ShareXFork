using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using ShareXBot.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ShareXBot.Models
{
    public class Bot
    {
        private static DiscordSocketClient _user;
        private static IReadOnlyCollection<SocketGuild> userGuilds;

        private static bool isReady = false;
        public static async Task<IReadOnlyCollection<SocketGuild>> Login(ApiController context, string email, string password)
        {
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://discordapp.com/api/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://discordapp.com/api/auth/login");
            string contentString = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";
            request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
            string token = "";
            HttpResponseMessage message = await client.SendAsync(request).ConfigureAwait(false);
            string json = await message.Content.ReadAsStringAsync();
            token = (string)JObject.Parse(json).GetValue("token");

            _user = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
                WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance
            });

            _user.Ready += GetUserGuilds;

            await _user.LoginAsync(TokenType.User, token);
            await _user.StartAsync();
            while (!isReady)
            {
            }
            return userGuilds;
        }

        private static async Task GetUserGuilds()
        {
            userGuilds = _user.Guilds;
            isReady = true;
        }

        public static async Task PostImage(ulong serverId, string fileName)
        {
            while (_user.ConnectionState != ConnectionState.Connected)
            {
            }
            _user.GetGuild(serverId).DefaultChannel.SendFileAsync(Path.GetTempPath() + fileName, "");
            await _user.StopAsync();
        }

    }
}
