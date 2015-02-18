using HouseOfKings.Web.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HouseOfKings.Web.ViewModels
{
    public class CardViewModel
    {
        public CardViewModel()
        {
            this.Rule = new RuleViewModel();
        }

        [JsonProperty("cardCount")]
        public int CardCount { get; set; }

        [JsonProperty("kingCount")]
        public int KingCount { get; set; }

        [JsonProperty("player")]
        public string Player { get; set; }

        [JsonProperty("number")]
        [Range(1, 13)]
        public int Number { get; set; }

        [JsonProperty("suit")]
        [Required]
        public Suit Suit { get; set; }

        [JsonProperty("rule")]
        public RuleViewModel Rule { get; set; }
    }

    public class RuleViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}