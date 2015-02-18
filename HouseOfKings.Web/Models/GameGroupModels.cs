using System.ComponentModel.DataAnnotations;

namespace HouseOfKings.Web.Models
{
    public class GameGroup
    {
        [Key]
        public string Name { get; set; }
    }
}