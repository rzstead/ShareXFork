using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShareXBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Models.Bot bot = new Models.Bot();
            //Task.Run(bot.MainAsync);
        }
    }
}
