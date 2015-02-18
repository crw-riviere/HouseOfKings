using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseOfKings.Web.Models
{
    public class GameGroup
    {
        [Key]
        public string Name { get; set; }

        private Deck deck;

        [NotMapped]
        public Deck Deck
        {
            get
            {
                if (this.deck == null)
                {
                    this.deck = new Deck();
                }
                return this.deck;
            }
            set
            {
                this.deck = value;
            }
        }
    }
}