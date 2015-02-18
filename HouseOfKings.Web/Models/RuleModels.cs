using System.ComponentModel.DataAnnotations;

namespace HouseOfKings.Web.Models
{
    public class Rule
    {
        [Key]
        [Range(1, 13)]
        public int Number { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}