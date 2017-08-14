using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Discord;
using Discord.WebSocket;
using ShareXBot.Models;
using System.IO;
using System.Web;
using System.Web.Http.Results;

namespace ShareXBot.Controllers
{
    public class BotController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Login(string email, string password)
        {
            IReadOnlyCollection<SocketGuild> result = Bot.Login(this, email, password).GetAwaiter().GetResult();
            List<Tuple<string, ulong>> guildInfo = new List<Tuple<string, ulong>>();
            foreach(SocketGuild guild in result)
            {
                guildInfo.Add(new Tuple<string, ulong>(guild.Name, guild.Id));
            }
            return Json(guildInfo);
        }

        [HttpPost]
        public IHttpActionResult Upload(ulong server)
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);
            img.Save(Path.GetTempPath() + file.FileName);
            Bot.PostImage(server, file.FileName);
            return Json("Ok");
        }

    }
}
