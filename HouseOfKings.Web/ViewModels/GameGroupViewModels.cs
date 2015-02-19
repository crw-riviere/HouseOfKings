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
        [JsonProperty("playerNames")]
        public List<string> PlayerNames { get; set; }
    }
}