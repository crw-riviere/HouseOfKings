using System.ComponentModel.DataAnnotations;

namespace HouseOfKings.Web.ViewModels
{
    public class GameGroupViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}