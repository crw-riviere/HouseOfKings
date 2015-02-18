using HouseOfKings.Web.DAL.Repository;
using HouseOfKings.Web.Models;
using HouseOfKings.Web.ViewModels;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace HouseOfKings.Web.Services
{
    public class GameService
    {
        private RuleRepository ruleRepository;

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

        private Dictionary<string, Deck> decks;

        public GameService()
        {
        }

        private Dictionary<string, Deck> Decks
        {
            get
            {
                if (this.decks == null)
                {
                    this.decks = new Dictionary<string, Deck>();
                }

                return this.decks;
            }
            set
            {
                this.decks = value;
            }
        }

        public GameService(IHubConnectionContext<dynamic> clients)
        {
            this.Clients = clients;
        }

        private Deck GetDeck(string groupName)
        {
            Deck deck;

            if (!this.Decks.TryGetValue(groupName, out deck))
            {
                deck = new Deck();
                this.Decks[groupName] = deck;
            }

            return deck;
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