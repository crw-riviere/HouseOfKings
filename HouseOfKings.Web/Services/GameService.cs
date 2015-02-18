using HouseOfKings.Web.DAL.Repository;
using HouseOfKings.Web.Models;
using HouseOfKings.Web.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
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

        private Deck GetDeck(string groupName)
        {
            var gameGroup = this.GameGroups.GetOrAdd(groupName, new GameGroup() { Name = groupName });

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

        public async Task PickCard(string player, string groupName)
        {
            var deck = this.GetDeck(groupName);
            var card = deck.PickRandomCard();

            if (card != null)
            {
                var getRuleTask = this.RuleRepository.GetAsync(card.Number);

                var cardVM = new CardViewModel() { Player = player, Number = card.Number, Suit = card.Suit, CardCount = deck.CardCount, KingCount = deck.KingCount };

                var rule = await getRuleTask;

                cardVM.Rule.Title = rule.Title;

                this.BroadcastCard(groupName, cardVM);
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