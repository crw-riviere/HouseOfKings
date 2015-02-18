using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseOfKings.Web.Models
{
    public class GameGroup
    {
        private Deck deck;
        private List<Player> players;

        [Key]
        public string Name { get; set; }

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

        [NotMapped]
        public List<Player> Players
        {
            get
            {
                if (this.players == null)
                {
                    this.players = new List<Player>();
                }
                return this.players;
            }
            set
            {
                this.players = value;
            }
        }
    }

    public class Player
    {
        public string Username { get; set; }

        public string ConnectionId { get; set; }
    }
}