using HouseOfKings.Web.DAL.Repository;
using HouseOfKings.Web.Models;
using HouseOfKings.Web.Properties;
using HouseOfKings.Web.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace HouseOfKings.Web.Services
{
    public class GameService
    {
        private RuleRepository ruleRepository;

        private ConcurrentDictionary<string, GameGroup> gameGroups;

        private ConcurrentDictionary<string, GameGroup> GameGroups
        {
            get
            {
                if (this.gameGroups == null)
                {
                    this.gameGroups = new ConcurrentDictionary<string, GameGroup>();
                }

                return this.gameGroups;
            }
            set
            {
                this.gameGroups = value;
            }
        }

        public RuleRepository RuleRepository
        {
            get
            {
                return this.ruleRepository ?? HttpContext.Current.GetOwinContext().Get<RuleRepository>();
            }
            set
            {
                this.ruleRepository = value;
            }
        }

        public static Player CurrentPlayer
        {
            get
            {
                var cookie = HttpContext.Current.Request.Cookies.Get(Resources.CookieName);
                if (cookie == null)
                {
                    throw new CookieException();
                }

                return new Player { Id = cookie[Resources.CookiePlayerId], Username = cookie[Resources.CookieUsername] };
            }
        }

        public void JoinGroup(string connectionId, string groupName)
        {
            var gameGroup = this.GetGameGroup(groupName);

            var player = gameGroup.Players.FirstOrDefault(x => x.Id.Equals(CurrentPlayer.Id));

            if (player == null)
            {
                player = CurrentPlayer;
                player.ConnectionId = connectionId;
                gameGroup.Players.Add(player);
            }
            else
            {
                player.ConnectionId = connectionId;
            }

            List<string> playerNames = gameGroup.Players.Select(x => x.Username).ToList();

            this.Clients.Client(connectionId).drawGroup(new GameGroupInfoViewModel() { PlayerNames = playerNames });
        }

        //public void LeaveGroup(string connectionId)
        //{
        //    var gameGroup = this.GetGameGroup(groupName);
        //    gameGroup.Players.Add(new Player() { ConnectionId = connectionId, Username = connectionId });
        //    Clients.Group(groupName).setAudit(this.GetUsername() + " joined the game");
        //}

        private GameGroup GetGameGroup(string groupName)
        {
            return this.GameGroups.GetOrAdd(groupName, new GameGroup() { Name = groupName });
        }

        public GameService()
        {
        }

        public GameService(IHubConnectionContext<dynamic> clients)
        {
            this.Clients = clients;
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        private static Player GetNextTurn(string currentPlayerId, List<Player> players)
        {
            var currentTurnPlayer = players.FirstOrDefault(x => x.Id.Equals(currentPlayerId));
            int nextIndex = players.IndexOf(currentTurnPlayer) + 1;
            if (nextIndex >= players.Count)
            {
                nextIndex = 0;
            }
            return players[nextIndex];
        }

        public async Task PickCard(string groupName)
        {
            var gameGroup = this.GetGameGroup(groupName);
            var deck = gameGroup.Deck;
            var card = deck.PickRandomCard();

            if (card != null)
            {
                var getRuleTask = this.RuleRepository.GetAsync(card.Number);

                var cardVM = new CardViewModel() { Player = CurrentPlayer.Username, Number = card.Number, Suit = card.Suit, CardCount = deck.CardCount, KingCount = deck.KingCount };

                var rule = await getRuleTask;

                cardVM.Rule.Title = rule.Title;

                this.BroadcastCard(groupName, cardVM);

                var nextPlayer = GetNextTurn(CurrentPlayer.Id, gameGroup.Players);

                this.Clients.Client(nextPlayer.ConnectionId).setTurn();
            }
            else
            {
                this.Clients.Group(groupName).setAudit("No more cards left.");
            }
        }

        private void BroadcastCard(string groupName, CardViewModel cardVM)
        {
            Clients.Group(groupName).showPickedCard(cardVM);
        }
    }
}