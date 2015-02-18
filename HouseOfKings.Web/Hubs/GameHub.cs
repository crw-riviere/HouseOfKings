using HouseOfKings.Web.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Ninject;
using System.Threading.Tasks;

namespace HouseOfKings.Web.Hubs
{
    [HubName("game")]
    public class GameHub : Hub
    {
        [Inject]
        public GameService GameService { get; set; }

        public async Task PickCard(string groupName)
        {
            await this.GameService.PickCard(Context.ConnectionId, groupName);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            this.GameService.JoinGroup(groupName);
        }

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    return base.OnDisconnected(stopCalled);
        //    this.GameService.LeaveGroup(Context.ConnectionId);
        //}
    }
}