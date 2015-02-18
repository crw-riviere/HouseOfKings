using System.ComponentModel.DataAnnotations;

namespace HouseOfKings.Web.ViewModels
{
    public class GameViewModel
    {
        public string GroupName { get; set; }

        [Required]
        [StringLength(10)]
        public string TempUsername { get; set; }
    }
}