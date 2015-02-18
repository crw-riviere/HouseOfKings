using HouseOfKings.Web.DAL.Repository;
using HouseOfKings.Web.Models;
using HouseOfKings.Web.Properties;
using HouseOfKings.Web.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        public void JoinGroup(string connectionId, string groupName)
        {
            var gameGroup = this.GetGameGroup(groupName);
            gameGroup.Players.Add(new Player() { ConnectionId = connectionId, Username = connectionId });
            Clients.Group(groupName).setAudit(this.GetUsername() + " joined the game");
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

        private Deck GetDeck(string groupName)
        {
            var gameGroup = this.GetGameGroup(groupName);

            return gameGroup.Deck;
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

        public GameService()
        {
        }

        public GameService(IHubConnectionContext<dynamic> clients)
        {
            this.Clients = clients;
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        private string GetUsername()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(Resources.CookieName);
            if (cookie != null && !string.IsNullOrEmpty(cookie[Resources.CookieUsername]))
            {
                return cookie[Resources.CookieUsername];
            }

            return null;
        }

        private Player GetNextTurn(string connectionId, List<Player> players)
        {
            int nextIndex = players.IndexOf(players.FirstOrDefault(x => x.ConnectionId.Equals(connectionId)));
            if (nextIndex >= players.Count)
            {
                nextIndex = 0;
            }
            return players[nextIndex];
        }

        public async Task PickCard(string connectionId, string groupName)
        {
            var gameGroup = this.GetGameGroup(groupName);
            var deck = gameGroup.Deck;
            var card = deck.PickRandomCard();

            if (card != null)
            {
                var getRuleTask = this.RuleRepository.GetAsync(card.Number);

                var cardVM = new CardViewModel() { Player = connectionId, Number = card.Number, Suit = card.Suit, CardCount = deck.CardCount, KingCount = deck.KingCount };

                var rule = await getRuleTask;

                cardVM.Rule.Title = rule.Title;

                this.BroadcastCard(groupName, cardVM);

                var nextPlayer = this.GetNextTurn(connectionId, gameGroup.Players);

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