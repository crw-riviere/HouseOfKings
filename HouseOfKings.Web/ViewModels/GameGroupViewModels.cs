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
        [JsonProperty("trn")]
        public TurnViewModel CurrentTurn { get; set; }

        [JsonProperty("usrs")]
        public List<PlayerViewModel> Players { get; set; }
    }

    public class PlayerViewModel
    {
        [JsonProperty("i")]
        public string Id { get; set; }

        [JsonProperty("n")]
        public string Username { get; set; }
    }
}