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

        [JsonProperty("f")]
        public bool GameOver { get; set; }

        [JsonProperty("cc")]
        public sbyte CardCount { get; set; }

        [JsonProperty("kc")]
        public sbyte KingCount { get; set; }

        [JsonProperty("usr")]
        public PlayerViewModel CurrentPlayer { get; set; }

        [JsonProperty("crd")]
        public CardViewModel Card { get; set; }

        [JsonProperty("rul")]
        public RuleViewModel Rule { get; set; }
    }

    public class CardViewModel
    {
        [JsonProperty("n")]
        [Range(1, 13)]
        public sbyte Number { get; set; }

        [JsonProperty("s")]
        [Required]
        public Suit Suit { get; set; }
    }

    public class RuleViewModel
    {
        [JsonProperty("t")]
        public string Title { get; set; }
    }
}