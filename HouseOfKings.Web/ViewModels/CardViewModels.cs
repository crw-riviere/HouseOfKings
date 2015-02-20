using HouseOfKings.Web.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HouseOfKings.Web.ViewModels
{
    public class TurnViewModel
    {
        public TurnViewModel()
        {
            this.Rule = new RuleViewModel();
        }

        [JsonProperty("cardCount")]
        public int CardCount { get; set; }

        [JsonProperty("kingCount")]
        public int KingCount { get; set; }

        [JsonProperty("player")]
        public PlayerViewModel CurrentPlayer { get; set; }

        [JsonProperty("card")]
        public CardViewModel Card { get; set; }

        [JsonProperty("rule")]
        public RuleViewModel Rule { get; set; }
    }

    public class CardViewModel
    {
        [JsonProperty("number")]
        [Range(1, 13)]
        public int Number { get; set; }

        [JsonProperty("suit")]
        [Required]
        public Suit Suit { get; set; }
    }

    public class RuleViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}