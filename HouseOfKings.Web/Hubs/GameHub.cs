using HouseOfKings.Web.Properties;
using HouseOfKings.Web.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Ninject;
using System.Threading.Tasks;
using System.Web;

namespace HouseOfKings.Web.Hubs
{
    [HubName("game")]
    public class GameHub : Hub
    {
        [Inject]
        public GameService GameService { get; set; }

        public void PickCard(string groupName)
        {
            this.GameService.PickCard(this.GetUsername(), groupName);
        }

        private string GetUsername()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(Resources.CookieName);
            if (cookie != null && !string.IsNullOrEmpty(cookie[Resources.CookieUsername]))
            {
                return cookie[Resources.CookieUsername];
            }

            return Context.ConnectionId;
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            Clients.Group(groupName).setAudit(this.GetUsername() + " joined the game");
        }
    }
}