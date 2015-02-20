using AutoMapper;
using HouseOfKings.Web.DAL.Repository;
using HouseOfKings.Web.Models;
using HouseOfKings.Web.Properties;
using HouseOfKings.Web.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR.Hubs;
using System;
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

        public async Task JoinGroup(string connectionId, string groupName)
        {
            var gameGroup = this.GetGameGroup(groupName);

            var player = gameGroup.Players.FirstOrDefault(x => x.Id.Equals(CurrentPlayer.Id));

            if (player == null)
            {
                player = CurrentPlayer;
                player.ConnectionId = connectionId;
                gameGroup.Players.Add(player);
                this.Clients.Group(groupName, connectionId).addPlayer(new PlayerViewModel() { Id = CurrentPlayer.Id, Username = CurrentPlayer.Username });
            }
            else
            {
                player.ConnectionId = connectionId;
            }

            List<PlayerViewModel> players = (from p in gameGroup.Players
                                             select new PlayerViewModel() { Id = p.Id, Username = p.Username }).ToList();

            this.Clients.Client(connectionId).drawGroup(new GameGroupInfoViewModel() { Players = players, CurrentTurn = await this.GetTurnInfo(groupName) });

            if (gameGroup.Players.Count <= 1)
            {
                this.SetTurn(player, gameGroup);
            }
        }

        public void LeaveGroup(string connectionId)
        {
            GameGroup gameGroup = this.GameGroups.Select(x => x.Value).FirstOrDefault(x => x.Players.Any(p => p.ConnectionId.Equals(connectionId)));
            if (gameGroup != null)
            {
                Player player = gameGroup.Players.FirstOrDefault(x => x.ConnectionId.Equals(connectionId));
                if (player != null)
                {
                    if (gameGroup.CurrentTurn.Equals(player))
                    {
                        var nextPlayer = GetNextTurn(player.Id, gameGroup.Players);
                        this.SetTurn(nextPlayer, gameGroup);
                    }

                    gameGroup.Players.Remove(player);

                    this.Clients.Group(gameGroup.Name).removePlayer(new PlayerViewModel() { Id = player.Id, Username = player.Username });

                    if (gameGroup.Players.Count == 0)
                    {
                        this.GameGroups.TryRemove(gameGroup.Name, out gameGroup);
                        gameGroup = null;
                    }
                }
            }
        }

        private GameGroup GetGameGroup(string groupName)
        {
            var gameGroup = this.GameGroups.GetOrAdd(groupName, new GameGroup() { Name = groupName });
            if (gameGroup.Deck.KingCount == 0)
            {
                gameGroup.Deck.Cards = Deck.BuildDeck().ToList();
            }

            return gameGroup;
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

        private async Task<TurnViewModel> GetTurnInfo(string groupName)
        {
            var gameGroup = this.GetGameGroup(groupName);

            if (gameGroup != null)
            {
                var deck = gameGroup.Deck;
                var card = deck.CurrentCard;

                var turnVM = new TurnViewModel()
                            {
                                CurrentPlayer = AutoMapper.Mapper.Map<Player, PlayerViewModel>(gameGroup.CurrentTurn),
                                CardCount = deck.CardCount,
                                KingCount = deck.KingCount
                            };

                if (card != null)
                {
                    var getRuleTask = this.RuleRepository.GetAsync(card.Number);

                    turnVM.Card = new CardViewModel() { Number = card.Number, Suit = card.Suit };

                    var rule = await getRuleTask;

                    turnVM.Rule = new RuleViewModel() { Title = rule.Title };
                }

                return turnVM;
            }

            return null;
        }

        public async Task PickCard(string groupName)
        {
            try
            {
                var gameGroup = this.GetGameGroup(groupName);
                var deck = gameGroup.Deck;
                var card = deck.PickRandomCard();

                if (card != null)
                {
                    var getRuleTask = this.RuleRepository.GetAsync(card.Number);

                    var turnVM = new TurnViewModel()
                    {
                        CurrentPlayer = Mapper.Map<Player, PlayerViewModel>(CurrentPlayer),
                        Card = new CardViewModel() { Number = card.Number, Suit = card.Suit },
                        CardCount = deck.CardCount,
                        KingCount = deck.KingCount,
                        GameOver = deck.KingCount == 0
                    };

                    var rule = await getRuleTask;

                    turnVM.Rule.Title = rule.Title;

                    this.BroadcastTurn(groupName, turnVM);

                    var nextPlayer = GetNextTurn(CurrentPlayer.Id, gameGroup.Players);

                    gameGroup.CurrentTurn = nextPlayer;

                    this.SetTurn(nextPlayer, gameGroup);
                }
                else
                {
                    this.SetGameover(CurrentPlayer, groupName);
                }
            }
            catch (Exception ex)
            {
                var i = 0;
            }
        }

        private void SetTurn(Player player, GameGroup gameGroup)
        {
            gameGroup.CurrentTurn = player;

            this.Clients.Client(player.ConnectionId).setTurn();
        }

        private void SetGameover(Player player, string groupName)
        {
            var playerVM = Mapper.Map<Player, PlayerViewModel>(player);
            this.Clients.Group(groupName).setGameover(playerVM);
        }

        private void BroadcastTurn(string groupName, TurnViewModel turnVM)
        {
            Clients.Group(groupName).drawTurn(turnVM);
        }
    }
}