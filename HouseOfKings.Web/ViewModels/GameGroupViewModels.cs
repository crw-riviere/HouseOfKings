using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseOfKings.Web.ViewModels
{
    public class GameGroupViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }

    public class GameGroupInfoViewModel
    {
        [JsonProperty("turn")]
        public TurnViewModel CurrentTurn { get; set; }

        [JsonProperty("players")]
        public List<PlayerViewModel> Players { get; set; }
    }

    public class PlayerViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Username { get; set; }
    }
}